using AutoMapper;
using PokeLeague.Application.DTOs;
using PokeLeague.Application.Services.Interfaces;
using PokeLeague.Infraestructure.Models;
using PokeLeague.Infraestructure.Repository.Implementations;
using PokeLeague.Infraestructure.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeLeague.Application.Services.Implementations
{
    public class ServiceAuction : IServiceAuction
    {
        private readonly IRepositoryAuction _repository;
        private readonly IMapper _mapper;

        public ServiceAuction(IRepositoryAuction repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<int> AddAsync(AuctionDTO auctionDto)
        {
            var auction = _mapper.Map<Auction>(auctionDto);
            var id = await _repository.AddAsync(auction);

            return id;
        }

        public async Task<AuctionDTO?> FindByIdAsync(int id)
        {
            var auction = await _repository.FindByIdAsync(id);

            if (auction == null)
                return null;

            var auctionDTO = _mapper.Map<AuctionDTO>(auction);

            auctionDTO.Status = ResolveStatusAsync(auctionDTO);

            return auctionDTO;
        }

        public async Task<ICollection<AuctionDTO>> ListAsync()
        {
            var auctions = await _repository.ListAsync();
            var collection = _mapper.Map<ICollection<AuctionDTO>>(auctions);

            foreach (var auction in collection) 
            {
                auction.Status = ResolveStatusAsync(auction);
            }
            //TODO: Fill Status
            return collection;
        }

        public async Task UpdateAsync(AuctionDTO auctionDto)
        {
            var auction = _mapper.Map<Auction>(auctionDto);
            auction.Id = auctionDto.Id;
            await _repository.UpdateAsync(auction);
        }

        public async Task<AuctionDTO?> FindActiveByCardIdAsync(int cardId)
        {
            var auction = await _repository.FindActiveByCardIdAsync(cardId);
            if (auction == null)
                return null;

            var auctionDTO = _mapper.Map<AuctionDTO>(auction);
            auctionDTO.Status = ResolveStatusAsync(auctionDTO);

            return auctionDTO;
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }

        private string ResolveStatusAsync(AuctionDTO auction)
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

        public async Task<ICollection<AuctionDTO>> ListActiveAsync() 
        {
            var auction = await ListAsync();

            return auction
                .Where(a => a.Status =="Scheduled" || a.Status =="In Progress")
                .ToList();

        }

        public async Task<ICollection<AuctionDTO>> ListClosedAsync()
        {
            var auction = await ListAsync();

            return auction
                .Where(a => a.Status == "Finished" || a.Status == "Canceled")
                .ToList();

        }
    }
}
