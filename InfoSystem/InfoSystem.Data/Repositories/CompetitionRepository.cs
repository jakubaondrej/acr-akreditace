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
    public class CompetitionRepository : RepositoryBase, ICompetitionRepository
    {
        public CompetitionRepository(ApplicationDbContext applicationDbContext)
            : base(applicationDbContext)
        {

        }

        public async Task<int> CreateCompetition(CompetitionCreateData data)
        {
            var entity = new Competition()
            {
                Name = data.Name,
                Description = data.Description,
                ChampionshipId = data.ChampionshipId,
                ImageUri = data.ImageUrl
            };
            Db.Competition.Add(entity);
            await Db.SaveChangesAsync();
            return entity.CompetitionId;
        }

        public async Task DeleteById(int id)
        {
            var entity = Db.Competition
               .Where(c => c.CompetitionId == id)
               .SingleOrDefault();
            Db.Competition.Remove(entity);
            await Db.SaveChangesAsync();
        }

        public async Task<int> EditCompetition(int id, CompetitionCreateData data)
        {
            var entity = await Db.Competition
                .Where(c => c.CompetitionId == id)
                .SingleOrDefaultAsync();
            entity.Name = data.Name;
            entity.ChampionshipId = data.ChampionshipId;
            entity.Description = data.Description;
            entity.ImageUri = data.ImageUrl;
            Db.Competition.Update(entity);
            await Db.SaveChangesAsync();
            return entity.CompetitionId;
        }

        public async Task<List<CompetitionListing>> GetAllCompetitions()
        {
            var list = await Db.Competition
                .Select(c => new CompetitionListing()
                {
                    Name = c.Name,
                    CompetitionId  =c.CompetitionId,
                    ImageUrl = c.ImageUri
                })
                .ToListAsync();
            return list;
        }

        public async Task<CompetitionDetails> GetCompetitionById(int id)
        {
            return await Db.Competition
                .Where(c => c.CompetitionId == id)
                .Select(c => new CompetitionDetails()
                {
                    Name = c.Name,
                    Championship = new ChampionshipListing()
                    {
                        Id = c.Championship.ChampionshipId,
                        Name = c.Championship.Name,
                        ImageUrl= c.Championship.ImageUrl
                    },
                    ImageUrl = c.ImageUri,
                    CompetitionId = c.CompetitionId,
                    Description = c.Description,
                    CompetitionSeasons = c.CompetitionSeasons.Select(cs => new CompetitionSeasonListing()
                    {
                        CompetitionSeasonId = cs.CompetitionSeasonId,
                        Season = new SeasonListing()
                        {
                            SeasonId = cs.Season.SeasonId,
                            Year = cs.Season.Year
                        },
                        OfficialSeasonName = cs.OfficialSeasonName,
                        PictureUrl = cs.PictureStoreUri.ToString()
                    }).ToList()
                })
                .SingleOrDefaultAsync();
        }

        public async Task<CompetitionDetails> GetCompetitionByName(string name)
        {
            return await Db.Competition
                .Where(c => c.Name == name)
                .Select(c => new CompetitionDetails()
                {
                    Name = c.Name,
                    Championship = new ChampionshipListing()
                    {
                        Id = c.Championship.ChampionshipId,
                        Name = c.Championship.Name
                    },
                    ImageUrl = c.ImageUri,
                    CompetitionId = c.CompetitionId,
                    Description = c.Description,
                    CompetitionSeasons = c.CompetitionSeasons.Select(cs => new CompetitionSeasonListing()
                    {
                        CompetitionSeasonId = cs.CompetitionSeasonId,
                        Season = new SeasonListing()
                        {
                            SeasonId = cs.Season.SeasonId,
                            Year = cs.Season.Year
                        },
                        OfficialSeasonName = cs.OfficialSeasonName,
                        PictureUrl= cs.PictureStoreUri.ToString()
                    }).ToList()
                })
                .SingleOrDefaultAsync();
        }

        public async Task<CompetitionSelection> GetCompetitionSelectionByChampionshipId(int id)
        {
            return await Db.Championships.Where(ch => ch.ChampionshipId == id)
                .Select(ch => new CompetitionSelection()
                {
                    Championship = new ChampionshipListing()
                    {
                        Id = ch.ChampionshipId,
                        Name = ch.Name
                    },
                    Sport = new SportListing()
                    {
                        Name = ch.Sport.Name,
                        Id = ch.Sport.SportId
                    },
                    Competitions = ch.Competitions.Select(c => new CompetitionListing()
                    {
                        Name = c.Name,
                        CompetitionId = c.CompetitionId,
                        ImageUrl = c.ImageUri
                    }).ToList()
                })
                .SingleOrDefaultAsync();
        }
    }
}
