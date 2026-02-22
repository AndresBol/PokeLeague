using PokeLeague.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeLeague.Application.Services.Interfaces
{
    public interface IServiceImage
    {
        Task<ICollection<ImageDTO>> ListAsync();
        Task<ImageDTO> FindByIdAsync(int id);
        Task<int> AddAsync(ImageDTO imageDto);
        Task UpdateAsync(ImageDTO imageDto);
        Task DeleteAsync(int id);
    }
}
