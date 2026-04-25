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
    public class ServiceCard : IServiceCard
    {
        private readonly IRepositoryCard _repository;
        private readonly IServiceAuction _serviceAuction;
        private readonly IMapper _mapper;

        public ServiceCard(IRepositoryCard repository, IServiceAuction serviceAuction, IMapper mapper)
        {
            _repository = repository;
            _serviceAuction = serviceAuction;
            _mapper = mapper;
        }

        public async Task<int> AddAsync(CardDTO cardDto)
        {
            var card = _mapper.Map<Card>(cardDto);
            var id = await _repository.AddAsync(card);

            return id;
        }

        public async Task<CardDTO> FindByIdAsync(int id)
        {
            var card = await _repository.FindByIdAsync(id);

            if (card == null)
                throw new Exception("Card not found");

            var cardDTO = _mapper.Map<CardDTO>(card);

            var activeAuction = await _serviceAuction.FindActiveByCardIdAsync(id);
            cardDTO.AuctionStatus = activeAuction?.Status ?? "Prepared";
            //TODO: Search if exists a better way
            if (cardDTO.Auction != null)
            {
                foreach (var auction in cardDTO.Auction)
                {
                    var resolvedAuction = await _serviceAuction.FindByIdAsync(auction.Id);
                    if (resolvedAuction != null)
                    {
                        auction.Status = resolvedAuction.Status;
                    }
                }
            }

            return cardDTO;
        }

        public async Task<ICollection<CardDTO>> ListAsync()
        {
            var cards = await _repository.ListAsync();
            var collection = _mapper.Map<ICollection<CardDTO>>(cards);
            //TODO: Fill Status
            return collection;
        }

        public async Task<ICollection<CardDTO>> ListByUserIdAsync(int userId)
        {
            var cards = await _repository.ListByUserIdAsync(userId);
            var collection = _mapper.Map<ICollection<CardDTO>>(cards);
            return collection;
        }

        public async Task UpdateAsync(CardDTO cardDto)
        {
            var card = _mapper.Map<Card>(cardDto);
            card.Id = cardDto.Id;
            await _repository.UpdateAsync(card);
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }

        public async Task ToggleActiveAsync(int id) 
        {
            var card = await _repository.FindByIdAsync(id);

            if(card == null)
            
                throw new Exception("Card not found");

                card.IsActive = !card.IsActive;

            await _repository.UpdateAsync(card);
            
        }
    }
}
