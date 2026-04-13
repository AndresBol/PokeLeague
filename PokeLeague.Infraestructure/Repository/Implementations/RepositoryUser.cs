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
            var strategy = _context.Database.CreateExecutionStrategy();

            return await strategy.ExecuteAsync(async () =>
            {
                using var transaction = await _context.Database.BeginTransactionAsync();
                try
                {
                    await _context.Set<User>().AddAsync(user);
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    return user.Id;
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw new Exception($"Error adding user: {ex.Message}");
                }
            });
        }

        public async Task<User> FindByIdAsync(int id)
        {
            var user = await _context.Set<User>()
                .AsNoTracking()
                .Include(u => u.Role)
                .Include(u => u.Card)
                .Include(u => u.Auction)
                    .ThenInclude(a => a.Card)
                .Include(u => u.Auction)
                    .ThenInclude(a => a.AuctionBid)
                .Include(u => u.Auction)
                    .ThenInclude(a => a.PurchaseOrder)
                .Include(u => u.AuctionBid)
                .FirstOrDefaultAsync(u => u.Id == id && u.IsActive);
            return user!;
        }
        public async Task<ICollection<User>> ListAsync()
        {
            var users = await _context.Set<User>()
                .AsNoTracking()
                .Include(u => u.Role)
                .Include(u => u.Card)
                .Include(u => u.Auction)
                    .ThenInclude(a => a.Card)
                .Include(u => u.Auction)
                    .ThenInclude(a => a.AuctionBid)
                .Include(u => u.Auction)
                    .ThenInclude(a => a.PurchaseOrder)
                .Include(u => u.AuctionBid)
                .Where(u => u.IsActive)
                .OrderBy(u => u.Id)
                .ToListAsync();
            return users!;
        }

        public async Task<ICollection<User>> ListByRoleIdAsync(int roleId)
        {
            var users = await _context.Set<User>()
                .AsNoTracking()
                .Include(u => u.Role)
                .Include(u => u.Card)
                .Include(u => u.Auction)
                    .ThenInclude(a => a.Card)
                .Include(u => u.Auction)
                    .ThenInclude(a => a.AuctionBid)
                .Include(u => u.Auction)
                    .ThenInclude(a => a.PurchaseOrder)
                .Include(u => u.AuctionBid)
                .Where(u => u.IsActive && u.RoleId == roleId)
                .OrderBy(u => u.Id)
                .ToListAsync();
            return users!;
        }

        public async Task UpdateAsync(User user)
        {
            var strategy = _context.Database.CreateExecutionStrategy();

            await strategy.ExecuteAsync(async () =>
            {
                using var transaction = await _context.Database.BeginTransactionAsync();
                try
                {
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

                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw new Exception($"Error updating user: {ex.Message}");
                }
            });
        }

        public async Task DeleteAsync(int id)
        {
            var strategy = _context.Database.CreateExecutionStrategy();

            await strategy.ExecuteAsync(async () =>
            {
                using var transaction = await _context.Database.BeginTransactionAsync();
                try
                {
                    var user = await _context.Set<User>().FirstOrDefaultAsync(u => u.Id == id);

                    if (user == null)
                    {
                        throw new Exception($"User with ID {id} not found.");
                    }

                    user.IsActive = false;
                    await _context.SaveChangesAsync();

                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw new Exception($"Error deleting user: {ex.Message}");
                }
            });
        }

        public async Task UpdateProfileAsync(int id, string username, string email)
        {
            try
            {
                //await _context.Database.BeginTransactionAsync();

                var user = await _context.Set<User>()
                    .FirstOrDefaultAsync(u => u.Id == id);

                if (user == null)
                    throw new Exception($"User with ID {id} not found.");

                user.Username = username;
                user.Email = email;

                await _context.SaveChangesAsync();
                //await _context.Database.CommitTransactionAsync();
            }
            catch (Exception ex)
            {
                //await _context.Database.RollbackTransactionAsync();
                throw new Exception($"Error updating profile: {ex.Message}");
            }
        }

        public async Task ToggleBlockAsync(int id)
        {
            try
            {
                //await _context.Database.BeginTransactionAsync();

                var user = await _context.Set<User>()
                    .FirstOrDefaultAsync(u => u.Id == id);

                if (user == null)
                    throw new Exception($"User with ID {id} not found.");

                user.IsBlocked = !user.IsBlocked;

                await _context.SaveChangesAsync();
                //await _context.Database.CommitTransactionAsync();
            }
            catch (Exception ex)
            {
                //await _context.Database.RollbackTransactionAsync();
                throw new Exception($"Error toggling block: {ex.Message}");
            }
        }

        public async Task<User> LoginAsync(string email, string passwordHash)
        {
            var user = await _context.Set<User>()
                .Include(u => u.Role)
                .Where(u => u.Email == email && u.PasswordHash == passwordHash && u.IsActive && !u.IsBlocked)
                .FirstOrDefaultAsync();

            return user!;
        }
    }
}
