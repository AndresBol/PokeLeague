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
        private readonly IRepositoryAuction _auctionRepository;
        private readonly IRepositoryUser _userRepository;
        private readonly IMapper _mapper;

        public ServiceAuctionBid(IRepositoryAuctionBid repository,IRepositoryAuction auctionRepository, IMapper mapper, IRepositoryUser repositoryUser)
        {
            _repository = repository;
            _auctionRepository = auctionRepository;
            _mapper = mapper;
            _userRepository = repositoryUser;
        }

        //public async Task<int> AddAsync(AuctionBidDTO auctionBidDto)
        //{

        //    var auctionBid = _mapper.Map<AuctionBid>(auctionBidDto);
        //    var id = await _repository.AddAsync(auctionBid);

        //    return id;
        //}

        public async Task<AuctionBidViewDTO> AddAsync(AuctionBidDTO auctionBidDTO, int userId)
        {
            var auction = await _auctionRepository.GetAuctionWithBids(auctionBidDTO.AuctionId);

            if (auction == null)
                throw new Exception("Auction not found");

            if(auction.IsCanceled)
                throw new Exception("Auction is canceled");

            if (DateTime.Now < auction.StartDate)
                throw new Exception("Auction has not started");

            if (DateTime.Now > auction.EndDate)
                throw new Exception("Auction has ended");

            var currentPrice = GetCurrentPrice(auction);

            if (auctionBidDTO.BidAmount < currentPrice + auction.MinIncrease)
                throw new Exception($"Minimum allowed bid is {(currentPrice + auction.MinIncrease):F2}");

            var bid = _mapper.Map<AuctionBid>(auctionBidDTO);

            bid.UserId = userId;
            bid.BidDate = DateTime.Now;


            var latestAuction = await _auctionRepository.GetAuctionWithBids(auctionBidDTO.AuctionId);

            var latestPrice = GetCurrentPrice(latestAuction);

            if (auctionBidDTO.BidAmount < latestPrice + latestAuction.MinIncrease)
                throw new Exception("Another bid was placed. Try again!");

            await _repository.AddAsync(bid);

            var user = await _userRepository.FindByIdAsync(userId);

            if (user == null)
                throw new Exception("User not found");

            return new AuctionBidViewDTO
            {
                Id = bid.Id,
                BidAmount = bid.BidAmount,
                BidDate = bid.BidDate,
                Username =user.Username

            };

        }

        private decimal GetCurrentPrice(Auction auction) 
        {
            return auction.AuctionBid.Any()
                ? auction.AuctionBid.Max(b => b.BidAmount)
                : auction.BasePrice;
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
