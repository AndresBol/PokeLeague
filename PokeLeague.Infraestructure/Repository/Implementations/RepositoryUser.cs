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
    public class RepositoryUser : IRepositoryUser
    {
        private readonly PokeLeagueContext _context;

        public RepositoryUser(PokeLeagueContext context)
        {
            _context = context;
        }

        public async Task<int> AddAsync(User user)
        {
            try
            {
                await _context.Database.BeginTransactionAsync();
                await _context.Set<User>().AddAsync(user);
                await _context.SaveChangesAsync();
                await _context.Database.CommitTransactionAsync();

                return user.Id;
            }
            catch (Exception ex)
            {
                await _context.Database.RollbackTransactionAsync();
                throw new Exception($"Error adding user: {ex.Message}");
            }
        }

        public async Task<User> FindByIdAsync(int id)
        {
            var user = await _context.Set<User>()
                .AsNoTracking()
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Id == id && u.IsActive);
            return user!;
        }
        public async Task<ICollection<User>> ListAsync()
        {
            var users = await _context.Set<User>()
                .AsNoTracking()
                .Where(u => u.IsActive)
                .OrderBy(u => u.Id)
                .ToListAsync();
            return users!;
        }

        public async Task UpdateAsync(User user)
        {
            try
            {
                await _context.Database.BeginTransactionAsync();
                
                var existingUser = await _context.Set<User>()
                    .FirstOrDefaultAsync(u => u.Id == user.Id);
                
                if (existingUser == null)
                {
                    throw new Exception($"User with ID {user.Id} not found.");
                }

                existingUser.RoleId = user.RoleId;
                existingUser.Username = user.Username;
                existingUser.Email = user.Email;
                existingUser.PasswordHash = user.PasswordHash;
                existingUser.IsBlocked = user.IsBlocked;
                existingUser.SignupDate = user.SignupDate;
                existingUser.IsActive = user.IsActive;
                
                await _context.SaveChangesAsync();
                
                await _context.Database.CommitTransactionAsync();
            }
            catch (Exception ex)
            {
                await _context.Database.RollbackTransactionAsync();
                throw new Exception($"Error updating user: {ex.Message}");
            }
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                await _context.Database.BeginTransactionAsync();
                var user = await _context.Set<User>().FirstOrDefaultAsync(u => u.Id == id);
                
                if (user == null)
                {
                    throw new Exception($"User with ID {id} not found.");
                }

                user.IsActive = false;
                await _context.SaveChangesAsync();
                
                await _context.Database.CommitTransactionAsync();
            }
            catch (Exception ex)
            {
                await _context.Database.RollbackTransactionAsync();
                throw new Exception($"Error deleting user: {ex.Message}");
            }
        }
    }
}
