using PokeLeague.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeLeague.Application.Services.Interfaces
{
    public interface IServiceCategoryCard
    {
        Task<ICollection<CategoryCardDTO>> ListAsync();
        Task<CategoryCardDTO> FindByIdAsync(int id);
        Task<int> AddAsync(CategoryCardDTO categoryCardDto);
        Task UpdateAsync(CategoryCardDTO categoryCardDto);
        Task DeleteAsync(int id);
    }
}
