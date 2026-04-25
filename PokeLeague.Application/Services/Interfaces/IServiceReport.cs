using PokeLeague.Application.DTOs;

namespace PokeLeague.Application.Services.Interfaces
{
    public interface IServiceReport
    {
        Task<ICollection<AuctionDTO>> GetAuctionsByFilterAsync(DateTime? startDate, DateTime? endDate, string? status);
        Task<byte[]> GenerateAuctionSalesReportPdfAsync(ICollection<AuctionDTO> auctions, string logoPath);
    }
}
