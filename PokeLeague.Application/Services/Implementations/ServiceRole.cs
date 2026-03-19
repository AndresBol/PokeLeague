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
    public class ServiceRole : IServiceRole
    {
        private readonly IRepositoryRole _repository;
        private readonly IMapper _mapper;

        public ServiceRole(IRepositoryRole repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<int> AddAsync(RoleDTO roleDto)
        {
            var role = _mapper.Map<Role>(roleDto);
            var id = await _repository.AddAsync(role);

            return id;
        }

        public async Task<RoleDTO> FindByIdAsync(int id)
        {
            var role = await _repository.FindByIdAsync(id);
            var roleDTO = _mapper.Map<RoleDTO>(role);

            return roleDTO;
        }

        public async Task<RoleDTO> FindByNameAsync(string name)
        {
            var role = await _repository.FindByNameAsync(name);
            var roleDTO = _mapper.Map<RoleDTO>(role);

            return roleDTO;
        }

        public async Task<ICollection<RoleDTO>> ListAsync()
        {
            var roles = await _repository.ListAsync();
            var collection = _mapper.Map<ICollection<RoleDTO>>(roles);

            return collection;
        }

        public async Task UpdateAsync(RoleDTO roleDto)
        {
            var role = _mapper.Map<Role>(roleDto);
            role.Id = roleDto.Id;
            await _repository.UpdateAsync(role);
        }
        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}
