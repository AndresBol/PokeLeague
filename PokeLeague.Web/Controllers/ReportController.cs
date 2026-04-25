using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PokeLeague.Application.Services.Interfaces;
using PokeLeague.Web.ViewModels;

namespace PokeLeague.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ReportController : Controller
    {
        private readonly IServiceReport _serviceReport;
        private readonly IWebHostEnvironment _env;

        public ReportController(IServiceReport serviceReport, IWebHostEnvironment env)
        {
            _serviceReport = serviceReport;
            _env = env;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var model = new ViewModelReport();
            await PopulateChartData(model);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(ViewModelReport model)
        {
            await PopulateChartData(model);
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> DownloadPdf(DateTime? startDate, DateTime? endDate, string? status)
        {
            var auctions = await _serviceReport.GetAuctionsByFilterAsync(startDate, endDate, status);

            var logoPath = Path.Combine(_env.WebRootPath, "media", "light", "imagotipo.png");

            var bytes = await _serviceReport.GenerateAuctionSalesReportPdfAsync(auctions, logoPath);

            return File(bytes, "application/pdf", "AuctionSalesReport.pdf");
        }

        private async Task PopulateChartData(ViewModelReport model)
        {
            var auctions = await _serviceReport.GetAuctionsByFilterAsync(
                model.StartDate, model.EndDate, model.Status);

            if (auctions == null || auctions.Count == 0)
            {
                model.Mensaje = "No auctions found for the selected filters.";
                return;
            }

            foreach (var auction in auctions.OrderBy(a => a.Id))
            {
                var bidCount = auction.AuctionBid?.Count ?? 0;
                model.Etiquetas.Add($"Auction #{auction.Id}");
                model.Valores.Add(bidCount);
            }

            model.TituloGrafico = "Bids per Auction";
        }
    }
}
