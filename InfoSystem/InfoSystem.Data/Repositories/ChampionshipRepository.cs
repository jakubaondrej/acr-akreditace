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
    public class ChampionshipRepository : RepositoryBase, IChampionshipRepository
    {
        public ChampionshipRepository(ApplicationDbContext applicationDbContext)
            : base(applicationDbContext)
        {

        }

        public async Task<int> CreateChampionship(ChampionshipCreateData data)
        {
            var entity = new Championship()
            {
                Name = data.Name,
                SportId = data.SportId,
                ImageUrl = data.ImageUrl
            };
            Db.Championships.Add(entity);
            await Db.SaveChangesAsync();
            return entity.ChampionshipId;
        }

        public async Task Delete(int id)
        {
            var champ = await Db.Championships.Where(s => s.ChampionshipId == id)
                .SingleOrDefaultAsync();
            if (champ == null)
            {
                throw new Exception($"Chamionship Id = {id} does not exist");
            }
            Db.Championships.Remove(champ);
            await Db.SaveChangesAsync();
        }

        public async Task<int> EditChampionship(int id, ChampionshipCreateData data)
        {
            var champ = await Db.Championships.Where(c => c.ChampionshipId == id)
                .SingleOrDefaultAsync();
            if (champ == null)
            {
                throw new Exception($"Chamionship Id = {id} does not exist");
            }
            champ.Name = data.Name;
            champ.SportId = data.SportId;
            champ.ImageUrl = data.ImageUrl;
            Db.Championships.Update(champ);
            return await Db.SaveChangesAsync();
        }

        public async Task<List<ChampionshipListing>> GetAllChampionships()
        {
            return await Db.Championships.Select(r => new ChampionshipListing()
            {
                Id = r.ChampionshipId,
                Name = r.Name,
                ImageUrl = r.ImageUrl
            })
               .ToListAsync();
        }

        public async Task<ChampionshipDetails> GetChampionshipById(int id)
        {
            return await Db.Championships.Where(c => c.ChampionshipId == id)
                .Select(c => new ChampionshipDetails()
                {
                    Id = c.ChampionshipId,
                    Name = c.Name,
                    Sport = new SportListing()
                    {
                        Id = c.Sport.SportId,
                        Name = c.Sport.Name,
                    },
                    ImageUrl=c.ImageUrl
                })
                .SingleOrDefaultAsync();
        }

        public async Task<ChampionshipDetails> GetChampionshipByName(string name)
        {
            return await Db.Championships.Where(c => c.Name == name)
                .Select(c => new ChampionshipDetails()
                {
                    Id = c.ChampionshipId,
                    Name = c.Name,
                    Sport = new SportListing()
                    {
                        Id = c.Sport.SportId,
                        Name = c.Sport.Name
                    },
                    ImageUrl = c.ImageUrl
                })
                .SingleOrDefaultAsync();
        }

        public async Task<ChampionshipSelection> GetChampionshipSelectionBySportId(int id)
        {
            return await Db.Sport.Where(s => s.SportId == id)
                .Select(s => new ChampionshipSelection()
                {
                    Sport = new SportListing()
                    {
                        Id = s.SportId,
                        Name = s.Name
                    },
                    Championships = s.Championships.Select(c => new ChampionshipListing()
                    {
                        Id = c.ChampionshipId,
                        Name = c.Name,
                        ImageUrl= c.ImageUrl
                    }).ToList()
                })
                .SingleOrDefaultAsync();
        }
    }
}
