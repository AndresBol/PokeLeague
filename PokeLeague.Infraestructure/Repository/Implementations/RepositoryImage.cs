using Microsoft.EntityFrameworkCore;
using PokeLeague.Infraestructure.Data;
using PokeLeague.Infraestructure.Models;
using PokeLeague.Infraestructure.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokeLeague.Infraestructure.Repository.Implementations
{
    public class RepositoryImage : IRepositoryImage
    {
        private readonly PokeLeagueContext _context;

        public RepositoryImage(PokeLeagueContext context)
        {
            _context = context;
        }

        public async Task<int> AddAsync(Image image)
        {
            try
            {
                await _context.Database.BeginTransactionAsync();
                await _context.Set<Image>().AddAsync(image);
                await _context.SaveChangesAsync();
                await _context.Database.CommitTransactionAsync();

                return image.Id;
            }
            catch (Exception ex)
            {
                await _context.Database.RollbackTransactionAsync();
                throw new Exception($"Error adding image: {ex.Message}");
            }
        }

        public async Task<Image> FindByIdAsync(int id)
        {
            var image = await _context.Set<Image>()
                .AsNoTracking()
                .FirstOrDefaultAsync(i => i.Id == id && i.IsActive);
            return image!;
        }

        public async Task<ICollection<Image>> ListAsync()
        {
            var images = await _context.Set<Image>()
                .AsNoTracking()
                .Where(i => i.IsActive)
                .OrderBy(i => i.Id)
                .ToListAsync();
            return images!;
        }

        public async Task UpdateAsync(Image image)
        {
            try
            {
                await _context.Database.BeginTransactionAsync();

                var existingImage = await _context.Set<Image>()
                    .FirstOrDefaultAsync(i => i.Id == image.Id);

                if (existingImage == null)
                {
                    throw new Exception($"Image with ID {image.Id} not found.");
                }

                existingImage.CardId = image.CardId;
                existingImage.ImageData = image.ImageData;
                existingImage.IsActive = image.IsActive;

                await _context.SaveChangesAsync();

                await _context.Database.CommitTransactionAsync();
            }
            catch (Exception ex)
            {
                await _context.Database.RollbackTransactionAsync();
                throw new Exception($"Error updating image: {ex.Message}");
            }
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                await _context.Database.BeginTransactionAsync();
                var image = await _context.Set<Image>().FirstOrDefaultAsync(i => i.Id == id);

                if (image == null)
                {
                    throw new Exception($"Image with ID {id} not found.");
                }

                image.IsActive = false;
                await _context.SaveChangesAsync();

                await _context.Database.CommitTransactionAsync();
            }
            catch (Exception ex)
            {
                await _context.Database.RollbackTransactionAsync();
                throw new Exception($"Error deleting image: {ex.Message}");
            }
        }
    }
}
