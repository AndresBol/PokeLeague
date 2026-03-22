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
    public class ServiceCategoryCard : IServiceCategoryCard
    {
        private readonly IRepositoryCategoryCard _repository;
        private readonly IMapper _mapper;

        public ServiceCategoryCard(IRepositoryCategoryCard repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<int> AddAsync(CategoryCardDTO categoryCardDto)
        {
            var categoryCard = _mapper.Map<CategoryCard>(categoryCardDto);
            var id = await _repository.AddAsync(categoryCard);

            return id;
        }

        public async Task<CategoryCardDTO> FindByIdAsync(int id)
        {
            var categoryCard = await _repository.FindByIdAsync(id);
            var categoryCardDTO = _mapper.Map<CategoryCardDTO>(categoryCard);

            return categoryCardDTO;
        }

        public async Task<ICollection<CategoryCardDTO>> ListAsync()
        {
            var categoryCards = await _repository.ListAsync();
            var collection = _mapper.Map<ICollection<CategoryCardDTO>>(categoryCards);

            return collection;
        }

        public async Task UpdateAsync(CategoryCardDTO categoryCardDto)
        {
            var categoryCard = _mapper.Map<CategoryCard>(categoryCardDto);
            categoryCard.Id = categoryCardDto.Id;
            await _repository.UpdateAsync(categoryCard);
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }

        public async Task DeleteByCardIdAsync(int cardId) 
        {
            await _repository.DeleteByCardIdAsync(cardId);
        }
    }
}
