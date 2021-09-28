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
    public class SportRepository : RepositoryBase, ISportRepository
    {
        public SportRepository(ApplicationDbContext applicationDbContext)
            : base(applicationDbContext)
        {

        }

        public async Task<int> CreateSport(SportCreateData data)
        {
            var sport = new Sport()
            {
                Championships = new List<Championship>(),
                Name = data.Name,
                ImageUri = data.ImageUri
            };
            Db.Sport.Add(sport);
            return await Db.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var sport = await Db.Sport.Where(s => s.SportId == id)
                .SingleOrDefaultAsync();
            Db.Sport.Remove(sport);
            await Db.SaveChangesAsync();
        }

        public async Task<int> EditSport(int id, SportCreateData data)
        {
            var sport = new Sport()
            {
                SportId = id,
                Championships = new List<Championship>(),
                Name = data.Name,
                ImageUri = data.ImageUri
            };
            Db.Sport.Update(sport);
            return await Db.SaveChangesAsync();
        }

        public async Task<List<SportListing>> GetAllSports()
        {
            return await Db.Sport
                .Select(s=> new SportListing()
                {
                    Id = s.SportId,
                    Name = s.Name,
                    ImageUri = s.ImageUri
                })
                .ToListAsync();
        }

        public async Task<SportDetail> GetSportById(int id)
        {
            return await Db.Sport.Where(s => s.SportId == id)
                .Select(s=> new SportDetail()
                {
                    Id = s.SportId,
                    Name = s.Name,
                    ImageUri = s.ImageUri,
                    Championships = s.Championships.Select(c => new ChampionshipListing()
                    {
                        Id = c.ChampionshipId,
                        Name = c.Name
                    }).ToList()
                })
                .SingleOrDefaultAsync();
        }

        public async Task<SportDetail> GetSportByName(string name)
        {
            return await Db.Sport.Where(s => s.Name == name)
                .Select(s => new SportDetail()
                {
                    Id = s.SportId,
                    Name = s.Name,
                    ImageUri = s.ImageUri,
                    Championships = s.Championships.Select(c => new ChampionshipListing()
                    {
                        Id = c.ChampionshipId,
                        Name = c.Name
                    }).ToList()
                })
                .SingleOrDefaultAsync();
        }

        public async Task<List<SportListing>> GetSportsBySeasonId(int seasonId)
        {
            return await Db.Sport
                .Where(s=>s.Championships
                    .Any(ch=>ch.Competitions
                    .Any(c=>c.CompetitionSeasons
                    .Any(cs=>cs.SeasonId ==seasonId))))
                .Select(s => new SportListing()
                {
                    Id = s.SportId,
                    Name = s.Name,
                    ImageUri = s.ImageUri
                })
                .ToListAsync();
        }
    }
}
