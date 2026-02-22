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
        private readonly IMapper _mapper;

        public ServiceCard(IRepositoryCard repository, IMapper mapper)
        {
            _repository = repository;
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
            var cardDTO = _mapper.Map<CardDTO>(card);

            return cardDTO;
        }

        public async Task<ICollection<CardDTO>> ListAsync()
        {
            var cards = await _repository.ListAsync();
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
    }
}
