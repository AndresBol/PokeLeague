using AutoMapper;
using PokeLeague.Application.DTOs;
using PokeLeague.Application.Services.Interfaces;
using PokeLeague.Infraestructure.Models;
using PokeLeague.Infraestructure.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeLeague.Application.Services.Implementations
{
    public class ServiceAuctionBid : IServiceAuctionBid
    {
        private readonly IRepositoryAuctionBid _repository;
        private readonly IMapper _mapper;

        public ServiceAuctionBid(IRepositoryAuctionBid repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<int> AddAsync(AuctionBidDTO auctionBidDto)
        {
            var auctionBid = _mapper.Map<AuctionBid>(auctionBidDto);
            var id = await _repository.AddAsync(auctionBid);

            return id;
        }

        public async Task<AuctionBidDTO> FindByIdAsync(int id)
        {
            var auctionBid = await _repository.FindByIdAsync(id);
            var auctionBidDTO = _mapper.Map<AuctionBidDTO>(auctionBid);

            return auctionBidDTO;
        }

        public async Task<ICollection<AuctionBidDTO>> ListAsync()
        {
            var auctionBids = await _repository.ListAsync();
            var collection = _mapper.Map<ICollection<AuctionBidDTO>>(auctionBids);

            return collection;
        }

        public async Task UpdateAsync(AuctionBidDTO auctionBidDto)
        {
            var auctionBid = _mapper.Map<AuctionBid>(auctionBidDto);
            auctionBid.Id = auctionBidDto.Id;
            await _repository.UpdateAsync(auctionBid);
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}
