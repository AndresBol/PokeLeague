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
    public class ServiceLanguage : IServiceLanguage
    {
        private readonly IRepositoryLanguage _repository;
        private readonly IMapper _mapper;

        public ServiceLanguage(IRepositoryLanguage repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<string> AddAsync(LanguageDTO languageDto)
        {
            var language = _mapper.Map<Language>(languageDto);
            var code = await _repository.AddAsync(language);

            return code;
        }

        public async Task<LanguageDTO> FindByIdAsync(string languageCode)
        {
            var language = await _repository.FindByIdAsync(languageCode);
            var languageDTO = _mapper.Map<LanguageDTO>(language);

            return languageDTO;
        }

        public async Task<ICollection<LanguageDTO>> ListAsync()
        {
            var languages = await _repository.ListAsync();
            var collection = _mapper.Map<ICollection<LanguageDTO>>(languages);

            return collection;
        }

        public async Task UpdateAsync(LanguageDTO languageDto)
        {
            var language = _mapper.Map<Language>(languageDto);
            language.LanguageCode = languageDto.LanguageCode;
            await _repository.UpdateAsync(language);
        }

        public async Task DeleteAsync(string languageCode)
        {
            await _repository.DeleteAsync(languageCode);
        }
    }
}
