using PokeLeague.Infraestructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeLeague.Infraestructure.Repository.Interfaces
{
    public interface IRepositoryCategoryCard
    {
        Task<ICollection<CategoryCard>> ListAsync();
        Task<CategoryCard> FindByIdAsync(int id);
        Task<int> AddAsync(CategoryCard categoryCard);
        Task UpdateAsync(CategoryCard categoryCard);
        Task DeleteAsync(int id);
    }
}
