using PokeLeague.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeLeague.Application.Services.Interfaces
{
    public interface IServiceCategory
    {
        Task<ICollection<CategoryDTO>> ListAsync();
        Task<CategoryDTO> FindByIdAsync(int id);
        Task<int> AddAsync(CategoryDTO categoryDto);
        Task UpdateAsync(CategoryDTO categoryDto);
        Task DeleteAsync(int id);
    }
}
