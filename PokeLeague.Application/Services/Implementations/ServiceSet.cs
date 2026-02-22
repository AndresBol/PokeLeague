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
    public class ServiceSet : IServiceSet
    {
        private readonly IRepositorySet _repository;
        private readonly IMapper _mapper;

        public ServiceSet(IRepositorySet repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<string> AddAsync(SetDTO setDto)
        {
            var set = _mapper.Map<Set>(setDto);
            var id = await _repository.AddAsync(set);

            return id;
        }

        public async Task<SetDTO> FindByIdAsync(string id)
        {
            var set = await _repository.FindByIdAsync(id);
            var setDTO = _mapper.Map<SetDTO>(set);

            return setDTO;
        }

        public async Task<ICollection<SetDTO>> ListAsync()
        {
            var sets = await _repository.ListAsync();
            var collection = _mapper.Map<ICollection<SetDTO>>(sets);

            return collection;
        }

        public async Task UpdateAsync(SetDTO setDto)
        {
            var set = _mapper.Map<Set>(setDto);
            set.Id = setDto.Id;
            await _repository.UpdateAsync(set);
        }

        public async Task DeleteAsync(string id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}
