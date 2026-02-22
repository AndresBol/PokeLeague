using PokeLeague.Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeLeague.Infraestructure.Repository.Interfaces
{
    public interface IRepositoryImage
    {
        Task<ICollection<Image>> ListAsync();
        Task<Image> FindByIdAsync(int id);
        Task<int> AddAsync(Image image);
        Task UpdateAsync(Image image);
        Task DeleteAsync(int id);
    }
}
