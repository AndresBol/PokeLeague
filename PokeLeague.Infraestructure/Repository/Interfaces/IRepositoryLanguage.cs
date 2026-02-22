using PokeLeague.Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeLeague.Infraestructure.Repository.Interfaces
{
    public interface IRepositoryLanguage
    {
        Task<ICollection<Language>> ListAsync();
        Task<Language> FindByIdAsync(string languageCode);
        Task<string> AddAsync(Language language);
        Task UpdateAsync(Language language);
        Task DeleteAsync(string languageCode);
    }
}
