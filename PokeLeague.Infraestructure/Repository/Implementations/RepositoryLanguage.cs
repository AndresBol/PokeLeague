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
    public class RepositoryLanguage : IRepositoryLanguage
    {
        private readonly PokeLeagueContext _context;

        public RepositoryLanguage(PokeLeagueContext context)
        {
            _context = context;
        }

        public async Task<string> AddAsync(Language language)
        {
            var strategy = _context.Database.CreateExecutionStrategy();

            return await strategy.ExecuteAsync(async () =>
            {
                using var transaction = await _context.Database.BeginTransactionAsync();
                try
                {
                    await _context.Set<Language>().AddAsync(language);
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    return language.LanguageCode;
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw new Exception($"Error adding language: {ex.Message}");
                }
            });
        }

        public async Task<Language> FindByIdAsync(string languageCode)
        {
            var language = await _context.Set<Language>()
                .AsNoTracking()
                .FirstOrDefaultAsync(l => l.LanguageCode == languageCode && l.IsActive);
            return language!;
        }

        public async Task<ICollection<Language>> ListAsync()
        {
            var languages = await _context.Set<Language>()
                .AsNoTracking()
                .Where(l => l.IsActive)
                .OrderBy(l => l.LanguageCode)
                .ToListAsync();
            return languages!;
        }

        public async Task UpdateAsync(Language language)
        {
            var strategy = _context.Database.CreateExecutionStrategy();

            await strategy.ExecuteAsync(async () =>
            {
                using var transaction = await _context.Database.BeginTransactionAsync();
                try
                {
                    var existingLanguage = await _context.Set<Language>()
                        .FirstOrDefaultAsync(l => l.LanguageCode == language.LanguageCode);

                    if (existingLanguage == null)
                    {
                        throw new Exception($"Language with code {language.LanguageCode} not found.");
                    }

                    existingLanguage.LanguageName = language.LanguageName;
                    existingLanguage.IsActive = language.IsActive;

                    await _context.SaveChangesAsync();

                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw new Exception($"Error updating language: {ex.Message}");
                }
            });
        }

        public async Task DeleteAsync(string languageCode)
        {
            var strategy = _context.Database.CreateExecutionStrategy();

            await strategy.ExecuteAsync(async () =>
            {
                using var transaction = await _context.Database.BeginTransactionAsync();
                try
                {
                    var language = await _context.Set<Language>().FirstOrDefaultAsync(l => l.LanguageCode == languageCode);

                    if (language == null)
                    {
                        throw new Exception($"Language with code {languageCode} not found.");
                    }

                    language.IsActive = false;
                    await _context.SaveChangesAsync();

                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    throw new Exception($"Error deleting language: {ex.Message}");
                }
            });
        }
    }
}
