using PokeLeague.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeLeague.Application.Services.Interfaces
{
    public interface IServiceLanguage
    {
        Task<ICollection<LanguageDTO>> ListAsync();
        Task<LanguageDTO> FindByIdAsync(string languageCode);
        Task<string> AddAsync(LanguageDTO languageDto);
        Task UpdateAsync(LanguageDTO languageDto);
        Task DeleteAsync(string languageCode);
    }
}
