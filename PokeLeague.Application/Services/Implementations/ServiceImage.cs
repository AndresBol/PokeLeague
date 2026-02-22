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
    public class ServiceImage : IServiceImage
    {
        private readonly IRepositoryImage _repository;
        private readonly IMapper _mapper;

        public ServiceImage(IRepositoryImage repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<int> AddAsync(ImageDTO imageDto)
        {
            var image = _mapper.Map<Image>(imageDto);
            var id = await _repository.AddAsync(image);

            return id;
        }

        public async Task<ImageDTO> FindByIdAsync(int id)
        {
            var image = await _repository.FindByIdAsync(id);
            var imageDTO = _mapper.Map<ImageDTO>(image);

            return imageDTO;
        }

        public async Task<ICollection<ImageDTO>> ListAsync()
        {
            var images = await _repository.ListAsync();
            var collection = _mapper.Map<ICollection<ImageDTO>>(images);

            return collection;
        }

        public async Task UpdateAsync(ImageDTO imageDto)
        {
            var image = _mapper.Map<Image>(imageDto);
            image.Id = imageDto.Id;
            await _repository.UpdateAsync(image);
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}
