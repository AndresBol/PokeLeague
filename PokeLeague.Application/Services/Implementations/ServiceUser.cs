using AutoMapper;
using Microsoft.Extensions.Options;
using PokeLeague.Application.Config;
using PokeLeague.Application.DTOs;
using PokeLeague.Application.Services.Interfaces;
using PokeLeague.Application.Utils;
using PokeLeague.Infraestructure.Models;
using PokeLeague.Infraestructure.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeLeague.Application.Services.Implementations
{
    public class ServiceUser : IServiceUser
    {
        private readonly IRepositoryUser _repository;
        private readonly IMapper _mapper;
        private readonly IOptions<AppConfig> _options;

        public ServiceUser(IRepositoryUser repository, IMapper mapper, IOptions<AppConfig> options)
        {
            _repository = repository;
            _mapper = mapper;
            _options = options;
        }

        public async Task<int> AddAsync(UserDTO userDto)
        {
            var user = _mapper.Map<User>(userDto);
            var id = await _repository.AddAsync(user);

            return id;
        }

        public async Task<UserDTO> FindByIdAsync(int id)
        {
            var user = await _repository.FindByIdAsync(id);
            var userDTO = _mapper.Map<UserDTO>(user);

            return userDTO;
        }

        public async Task<ICollection<UserDTO>> ListAsync()
        {
            var users = await _repository.ListAsync();
            var collection = _mapper.Map<ICollection<UserDTO>>(users);

            return collection;
        }
        public async Task<ICollection<UserDTO>> ListByRoleAsync(RoleDTO roleDto)
        {
            var users = await _repository.ListByRoleIdAsync(roleDto.Id);
            var collection = _mapper.Map<ICollection<UserDTO>>(users);

            return collection;
        }

        public async Task UpdateAsync(UserDTO userDto)
        {
            var user = _mapper.Map<User>(userDto);
            user.Id = userDto.Id;
            await _repository.UpdateAsync(user);
        }
        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }

        public async Task UpdateProfileAsync(int id, string username, string email)
        {

            await _repository.UpdateProfileAsync(id,username,email);
        }

        public async Task UpdatePasswordAsync(int id, string newPassword)
        {
            string secret = _options.Value.Crypto.Secret;
            string passwordEncrypted = Cryptography.Encrypt(newPassword, secret);
            await _repository.UpdatePasswordAsync(id, passwordEncrypted);
        }

        public async Task ToggleBlockAsync(int id) 
        {
            
            await _repository.ToggleBlockAsync(id);
        
        }

        public async Task<UserDTO> LoginAsync(string email, string password)
        {
            UserDTO userDTO = null!;

            string secret = _options.Value.Crypto.Secret;
            string passwordEncrypted = Cryptography.Encrypt(password, secret);

            var user = await _repository.LoginAsync(email, passwordEncrypted);

            if (user != null)
            {
                userDTO = _mapper.Map<UserDTO>(user);
            }

            return userDTO;
        }
    }
}
