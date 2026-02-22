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

        public async Task<AuctionDTO> FindByIdAsync(int id)
        {
            var auction = await _repository.FindByIdAsync(id);
            var auctionDTO = _mapper.Map<AuctionDTO>(auction);

            return auctionDTO;
        }

        public async Task<ICollection<AuctionDTO>> ListAsync()
        {
            var auctions = await _repository.ListAsync();
            var collection = _mapper.Map<ICollection<AuctionDTO>>(auctions);

            return collection;
        }

        public async Task UpdateAsync(AuctionDTO auctionDto)
        {
            var auction = _mapper.Map<Auction>(auctionDto);
            auction.Id = auctionDto.Id;
            await _repository.UpdateAsync(auction);
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}
