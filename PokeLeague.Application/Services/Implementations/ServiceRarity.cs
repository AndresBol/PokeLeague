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
    public class ServiceRarity : IServiceRarity
    {
        private readonly IRepositoryRarity _repository;
        private readonly IMapper _mapper;

        public ServiceRarity(IRepositoryRarity repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<string> AddAsync(RarityDTO rarityDto)
        {
            var rarity = _mapper.Map<Rarity>(rarityDto);
            var id = await _repository.AddAsync(rarity);

            return id;
        }

        public async Task<RarityDTO> FindByIdAsync(string id)
        {
            var rarity = await _repository.FindByIdAsync(id);
            var rarityDTO = _mapper.Map<RarityDTO>(rarity);

            return rarityDTO;
        }

        public async Task<ICollection<RarityDTO>> ListAsync()
        {
            var raritys = await _repository.ListAsync();
            var collection = _mapper.Map<ICollection<RarityDTO>>(raritys);

            return collection;
        }

        public async Task UpdateAsync(RarityDTO rarityDto)
        {
            var rarity = _mapper.Map<Rarity>(rarityDto);
            rarity.Id = rarityDto.Id;
            await _repository.UpdateAsync(rarity);
        }
        public async Task DeleteAsync(string id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}
