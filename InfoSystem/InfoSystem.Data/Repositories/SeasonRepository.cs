using InfoSystem.Core.DataAbstraction;
using InfoSystem.Data.Entities;
using InfoSystem.Web.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfoSystem.Data.Repositories
{
    public class SeasonRepository : RepositoryBase, ISeasonRepository
    {
        public SeasonRepository(ApplicationDbContext applicationDbContext)
            : base(applicationDbContext)
        {

        }

        public async Task<int> CreateSeason(SeasonCreateData data)
        {
            var entity = new Season()
            {
                Year = data.Year
            };
            Db.Season.Add(entity);
             await Db.SaveChangesAsync();
            return entity.SeasonId;
        }

        public async Task Delete(int id)
        {
            var entity = Db.Season
                .Where(s => s.SeasonId == id)
                .SingleOrDefault();
            Db.Season.Remove(entity);
            await Db.SaveChangesAsync();
        }

        public Task<int> EditSeason(int id, SeasonCreateData data)
        {
            throw new NotImplementedException();
        }

        public async Task<List<SeasonListing>> GetAllSeasons()
        {
            return await Db.Season.Select(s => new SeasonListing()
            {
                SeasonId = s.SeasonId,
                Year = s.Year
            })
                .ToListAsync();
        }

        public async Task<SeasonDetails> GetSeasonById(int id)
        {
            return await Db.Season
                .Where(s => s.SeasonId == id)
                .Select(s => new SeasonDetails()
                {
                    SeasonId = s.SeasonId,
                    Year = s.Year,
                    Competitions = s.Competitions.Select(c => new CompetitionListing()
                    {
                        CompetitionId = c.CompetitionId,
                        Name = c.Name
                    }).ToList()
                })
                .SingleOrDefaultAsync();
        }

        public async Task<SeasonDetails> GetSeasonByYear(int year)
        {
            return await Db.Season
                .Where(s => s.Year == year)
                .Select(s => new SeasonDetails()
                {
                    SeasonId = s.SeasonId,
                    Year = s.Year,
                    Competitions = s.Competitions.Select(c => new CompetitionListing()
                    {
                        CompetitionId = c.CompetitionId,
                        Name = c.Name
                    }).ToList()
                })
                .SingleOrDefaultAsync();
        }
    }
}
