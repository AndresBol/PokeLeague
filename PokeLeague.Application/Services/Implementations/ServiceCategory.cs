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
    public class ServiceCategory : IServiceCategory
    {
        private readonly IRepositoryCategory _repository;
        private readonly IMapper _mapper;

        public ServiceCategory(IRepositoryCategory repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<int> AddAsync(CategoryDTO categoryDto)
        {
            var category = _mapper.Map<Category>(categoryDto);
            var id = await _repository.AddAsync(category);

            return id;
        }

        public async Task<CategoryDTO> FindByIdAsync(int id)
        {
            var category = await _repository.FindByIdAsync(id);
            var categoryDTO = _mapper.Map<CategoryDTO>(category);

            return categoryDTO;
        }

        public async Task<ICollection<CategoryDTO>> ListAsync()
        {
            var categories = await _repository.ListAsync();
            var collection = _mapper.Map<ICollection<CategoryDTO>>(categories);

            return collection;
        }

        public async Task UpdateAsync(CategoryDTO categoryDto)
        {
            var category = _mapper.Map<Category>(categoryDto);
            category.Id = categoryDto.Id;
            await _repository.UpdateAsync(category);
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}
