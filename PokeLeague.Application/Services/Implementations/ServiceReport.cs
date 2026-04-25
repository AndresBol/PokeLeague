using AutoMapper;
using Microsoft.Extensions.Logging;
using PokeLeague.Application.DTOs;
using PokeLeague.Application.Services.Interfaces;
using PokeLeague.Infraestructure.Repository.Interfaces;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace PokeLeague.Application.Services.Implementations
{
    public class ServiceReport : IServiceReport
    {
        private readonly IRepositoryAuction _repositoryAuction;
        private readonly IMapper _mapper;
        private readonly ILogger<ServiceReport> _logger;

        public ServiceReport(
            IRepositoryAuction repositoryAuction,
            IMapper mapper,
            ILogger<ServiceReport> logger)
        {
            _repositoryAuction = repositoryAuction;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ICollection<AuctionDTO>> GetAuctionsByFilterAsync(DateTime? startDate, DateTime? endDate, string? status)
        {
            var auctions = await _repositoryAuction.ListAsync();
            var dtos = _mapper.Map<ICollection<AuctionDTO>>(auctions);

            foreach (var dto in dtos)
            {
                dto.Status = ResolveStatus(dto);
            }

            var filtered = dtos.AsEnumerable();

            if (startDate.HasValue)
                filtered = filtered.Where(a => a.EndDate >= startDate.Value);

            if (endDate.HasValue)
                filtered = filtered.Where(a => a.StartDate <= endDate.Value);

            if (!string.IsNullOrWhiteSpace(status))
                filtered = filtered.Where(a => a.Status.Equals(status, StringComparison.OrdinalIgnoreCase));

            return filtered.OrderBy(a => a.Id).ToList();
        }

        public async Task<byte[]> GenerateAuctionSalesReportPdfAsync(ICollection<AuctionDTO> auctions, string logoPath)
        {
            try
            {
                var finishedWithBids = auctions
                    .Where(a => a.Status == "Finished" && a.AuctionBid != null && a.AuctionBid.Any())
                    .OrderBy(a => a.Id)
                    .ToList();

                QuestPDF.Settings.License = LicenseType.Community;

                byte[]? logoBytes = null;
                if (File.Exists(logoPath))
                {
                    logoBytes = await File.ReadAllBytesAsync(logoPath);
                }

                var pdfBytes = Document.Create(document =>
                {
                    document.Page(page =>
                    {
                        page.Size(PageSizes.Letter);
                        page.Margin(25);
                        page.PageColor(Colors.White);

                        page.Header().Column(header =>
                        {
                            header.Item().Row(row =>
                            {
                                if (logoBytes != null)
                                {
                                    row.ConstantItem(120).Height(50).Image(logoBytes).FitArea();
                                }

                                row.RelativeItem().AlignRight().Column(col =>
                                {
                                    col.Item().Text("Auction Sales Report")
                                        .SemiBold()
                                        .FontSize(14);

                                    col.Item().Text($"Generated: {DateTime.Now:MM/dd/yyyy HH:mm}")
                                        .FontSize(10)
                                        .FontColor(Colors.Grey.Darken1);
                                });
                            });

                            header.Item().PaddingTop(5).LineHorizontal(1).LineColor(Colors.Grey.Lighten1);
                        });

                        page.Content().PaddingVertical(10).Column(content =>
                        {
                            content.Spacing(10);

                            content.Item().Text("Card Sale Amounts (Winning Bids)")
                                .Bold()
                                .FontSize(14);

                            if (!finishedWithBids.Any())
                            {
                                content.Item().PaddingTop(20).AlignCenter()
                                    .Text("No auction sales found for the selected filters.")
                                    .FontSize(12)
                                    .FontColor(Colors.Grey.Darken1);
                            }
                            else
                            {
                                content.Item().Table(table =>
                                {
                                    table.ColumnsDefinition(columns =>
                                    {
                                        columns.RelativeColumn(1);   // Auction #
                                        columns.RelativeColumn(3);   // Card Name
                                        columns.RelativeColumn(2);   // Winner
                                        columns.RelativeColumn(2);   // Winning Bid
                                        columns.RelativeColumn(2);   // End Date
                                    });

                                    table.Header(header =>
                                    {
                                        static IContainer HeaderCell(IContainer container) =>
                                            container
                                                .Background(Colors.Yellow.Medium)
                                                .Padding(5);

                                        header.Cell().Element(HeaderCell).Text("Auction").FontColor(Colors.Black).Bold();
                                        header.Cell().Element(HeaderCell).Text("Card Name").FontColor(Colors.Black).Bold();
                                        header.Cell().Element(HeaderCell).Text("Winner").FontColor(Colors.Black).Bold();
                                        header.Cell().Element(HeaderCell).AlignRight().Text("Winning Bid").FontColor(Colors.Black).Bold();
                                        header.Cell().Element(HeaderCell).AlignCenter().Text("End Date").FontColor(Colors.Black).Bold();
                                    });

                                    foreach (var auction in finishedWithBids)
                                    {
                                        var winningBid = auction.AuctionBid
                                            .OrderByDescending(b => b.BidAmount)
                                            .First();

                                        table.Cell().BorderBottom(0.5f).BorderColor(Colors.Grey.Lighten2).Padding(5)
                                            .Text($"{auction.Id}").FontSize(10);

                                        table.Cell().BorderBottom(0.5f).BorderColor(Colors.Grey.Lighten2).Padding(5)
                                            .Text(auction.Card?.Name ?? "N/A").FontSize(10);

                                        table.Cell().BorderBottom(0.5f).BorderColor(Colors.Grey.Lighten2).Padding(5)
                                            .Text(winningBid.User?.Username ?? "N/A").FontSize(10);

                                        table.Cell().BorderBottom(0.5f).BorderColor(Colors.Grey.Lighten2).Padding(5)
                                            .AlignRight()
                                            .Text(winningBid.BidAmount.ToString("$ #,##0.00")).FontSize(10);

                                        table.Cell().BorderBottom(0.5f).BorderColor(Colors.Grey.Lighten2).Padding(5)
                                            .AlignCenter()
                                            .Text(auction.EndDate.ToString("MM/dd/yyyy")).FontSize(10);
                                    }
                                });

                                var totalSales = finishedWithBids.Sum(a =>
                                    a.AuctionBid.Max(b => b.BidAmount));

                                content.Item().PaddingTop(10).Row(row =>
                                {
                                    row.RelativeItem().AlignRight().Text($"Total Sales: {totalSales:$ #,##0.00}")
                                        .Bold()
                                        .FontSize(12);
                                });
                            }
                        });

                        page.Footer()
                            .AlignRight()
                            .Text(text =>
                            {
                                text.Span("Page ").FontSize(10);
                                text.CurrentPageNumber().FontSize(10);
                            });
                    });
                }).GeneratePdf();

                return pdfBytes;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating auction sales PDF report");
                throw;
            }
        }

        private string ResolveStatus(AuctionDTO auction)
        {
            if (auction.IsCanceled)
                return "Canceled";

            var now = DateTime.Now;

            if (auction.StartDate > now)
                return "Scheduled";

            if (auction.StartDate <= now && auction.EndDate >= now)
                return "In Progress";

            return "Finished";
        }
    }
}
