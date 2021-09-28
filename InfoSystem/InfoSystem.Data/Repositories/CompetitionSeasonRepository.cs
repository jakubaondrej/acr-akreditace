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
    public class CompetitionSeasonRepository : RepositoryBase, ICompetitionSeasonRepository
    {
        public CompetitionSeasonRepository(ApplicationDbContext applicationDbContext)
           : base(applicationDbContext)
        {

        }

        public async Task<int> CreateCompetitionSeason(CompetitionSeasonCreateData data)
        {
            var entity = new CompetitionSeason()
            {
                CompetitionId = data.CompetitionId,
                SeasonId = data.SeasonId,
                Start = data.Start,
                End = data.End,
                PictureStoreUri = data.PictureStoreUri,
                JournalistUploadFotoDeadline = data.JournalistUploadFotoDeadline,
                JournalistRegistrationStart = data.JournalistRegistrationStart,
                JournalistRegistrationEnd = data.JournalistRegistrationEnd,
                OfficialSeasonName = data.OfficialSeasonName
            };
            Db.CompetitionSeason.Add(entity);
            await Db.SaveChangesAsync();
            return entity.CompetitionSeasonId;
        }

        public async Task DeleteById(int id)
        {
            var entity = Db.CompetitionSeason
               .Where(c => c.CompetitionSeasonId == id)
               .SingleOrDefault();
            Db.CompetitionSeason.Remove(entity);
            await Db.SaveChangesAsync();
        }

        public async Task<int> EditCompetitionSeason(int id, CompetitionSeasonCreateData data)
        {
            var entity = await Db.CompetitionSeason
                .Where(c => c.CompetitionSeasonId == id)
                .SingleOrDefaultAsync();
            entity.End = data.End;
            entity.Start = data.Start;
            entity.JournalistRegistrationEnd = data.JournalistRegistrationEnd;
            entity.JournalistRegistrationStart = data.JournalistRegistrationStart;
            entity.JournalistUploadFotoDeadline = data.JournalistUploadFotoDeadline;
            entity.PictureStoreUri = data.PictureStoreUri;
            entity.OfficialSeasonName = data.OfficialSeasonName;
            Db.CompetitionSeason.Update(entity);
            await Db.SaveChangesAsync();
            return entity.CompetitionSeasonId;
        }

        public async Task<List<CompetitionSeasonListing>> GetByCompetitionId(int competitionId)
        {
            return await Db.CompetitionSeason
                .Where(c => c.CompetitionId == competitionId)
                .Select(c => new CompetitionSeasonListing()
                {
                    CompetitionSeasonId = c.CompetitionSeasonId,
                    Season = new SeasonListing()
                    {
                        SeasonId = c.Season.SeasonId,
                        Year = c.Season.Year
                    },
                    OfficialSeasonName = c.OfficialSeasonName,
                    PictureUrl = c.PictureStoreUri.ToString()
                })
                .ToListAsync();
        }

        public async Task<CompetitionSeasonAccreditation> GetCompetitionSeasonAccreditationByCompetitionSeasonIdAndUserId(int competitionSeasonId, string userId)
        {
            var akred = await Db.Accreditation
                .Where(a => a.UserId == userId && a.CompetitionSeasonId == competitionSeasonId)
                .Select(a => new AccreditationListing()
                {
                    AccreditationId = a.AccreditationId,
                    Close = a.Close,
                    State = a.State
                })
                .SingleOrDefaultAsync();

            return await Db.CompetitionSeason
                .Where(cs => cs.CompetitionSeasonId == competitionSeasonId)
                   .Select(cs => new CompetitionSeasonAccreditation()
                   {
                       Championship = new ChampionshipListing()
                       {
                           Id = cs.Competition.ChampionshipId,
                           Name = cs.Competition.Championship.Name
                       },
                       Competition = new CompetitionListing()
                       {
                           CompetitionId = cs.CompetitionId,
                           Name = cs.Competition.Name
                       },
                       Sport = new SportListing()
                       {
                           Id = cs.Competition.Championship.Sport.SportId,
                           Name = cs.Competition.Championship.Sport.Name
                       },
                       Season = new SeasonListing()
                       {
                           SeasonId = cs.SeasonId,
                           Year = cs.Season.Year
                       },
                       CompetitionSeasonId = cs.CompetitionSeasonId,
                       End = cs.End,
                       JournalistRegistrationEnd = cs.JournalistRegistrationEnd,
                       JournalistRegistrationStart = cs.JournalistRegistrationStart,
                       JournalistUploadFotoDeadline = cs.JournalistUploadFotoDeadline,
                       Start = cs.Start,
                       Accreditation = akred,
                       OfficialSeasonName = cs.OfficialSeasonName
                   })
                   .SingleOrDefaultAsync();
        }

        public async Task<CompetitionSeasonDetails> GetCompetitionSeasonById(int id)
        {
            return await Db.CompetitionSeason
                .Where(c => c.CompetitionSeasonId == id)
                .Select(c => new CompetitionSeasonDetails()
                {
                    CompetitionSeasonId = c.CompetitionSeasonId,
                    Season = new SeasonListing()
                    {
                        SeasonId = c.Season.SeasonId,
                        Year = c.Season.Year
                    },
                    Competition = new CompetitionListing()
                    {
                        CompetitionId = c.Competition.CompetitionId,
                        Name = c.Competition.Name
                    },
                    End = c.End,
                    JournalistRegistrationEnd = c.JournalistRegistrationEnd,
                    JournalistRegistrationStart = c.JournalistRegistrationStart,
                    JournalistUploadFotoDeadline = c.JournalistUploadFotoDeadline,
                    PictureStoreUri = c.PictureStoreUri,
                    Start = c.Start
                })
                .SingleOrDefaultAsync();
        }

        public async Task<CompetitionSeasonDetails> GetCompetitionSeasonBySeasonAndCompetitionId(int seasonId, int competitionId)
        {
            return await Db.CompetitionSeason
                .Where(c => c.CompetitionId == competitionId && c.SeasonId == seasonId)
                .Select(c => new CompetitionSeasonDetails()
                {
                    CompetitionSeasonId = c.CompetitionSeasonId,
                    Season = new SeasonListing()
                    {
                        SeasonId = c.Season.SeasonId,
                        Year = c.Season.Year
                    },
                    Competition = new CompetitionListing()
                    {
                        CompetitionId = c.Competition.CompetitionId,
                        Name = c.Competition.Name
                    },
                    End = c.End,
                    JournalistRegistrationEnd = c.JournalistRegistrationEnd,
                    JournalistRegistrationStart = c.JournalistRegistrationStart,
                    JournalistUploadFotoDeadline = c.JournalistUploadFotoDeadline,
                    PictureStoreUri = c.PictureStoreUri,
                    Start = c.Start
                })
                .SingleOrDefaultAsync();
        }

        public async Task<List<CompetitionSeasonListing>> GetNextCompetitionSeason()
        {
            return await Db.CompetitionSeason.Where(c => c.End < DateTime.Now.AddMonths(2) && c.End > DateTime.Now)
                .Select(c => new CompetitionSeasonListing()
                {
                    CompetitionSeasonId = c.CompetitionSeasonId,
                    OfficialSeasonName = c.OfficialSeasonName,
                    PictureUrl = c.PictureStoreUri.ToString(),
                    Season = new SeasonListing()
                    {
                        SeasonId = c.Season.SeasonId,
                        Year = c.Season.Year
                    }
                })
                .ToListAsync();
        }

        public async Task<CompetitionSeasonSelection> GetSelectionByCompetitionId(int competitionId)
        {
            return await Db.Competition.Where(c => c.CompetitionId == competitionId)
                    .Select(c => new CompetitionSeasonSelection()
                    {
                        Championship = new ChampionshipListing()
                        {
                            Id = c.Championship.ChampionshipId,
                            Name = c.Championship.Name
                        },
                        Competition = new CompetitionListing()
                        {
                            CompetitionId = c.CompetitionId,
                            Name = c.Name
                        },
                        Sport = new SportListing()
                        {
                            Id = c.Championship.Sport.SportId,
                            Name = c.Championship.Sport.Name
                        },
                        Seasons = c.CompetitionSeasons.Any()
                            ? c.CompetitionSeasons.Select(cs =>
                                   new CompetitionSeasonListing()
                                   {
                                       CompetitionSeasonId = cs.CompetitionSeasonId,
                                       Season = new SeasonListing()
                                       {
                                           SeasonId = cs.Season.SeasonId,
                                           Year = cs.Season.Year
                                       },
                                       OfficialSeasonName = cs.OfficialSeasonName,
                                       PictureUrl=cs.PictureStoreUri.ToString()
                                   }
                                ).ToList()
                            : null
                    })
                    .SingleOrDefaultAsync();
        }

        public async Task UpdateGoogleDriveDirection(int competitionSeasonid, string id)
        {
            var entity = await Db.CompetitionSeason
                .Where(c => c.CompetitionSeasonId == competitionSeasonid)
                .SingleOrDefaultAsync();
            entity.DriveFolderId = id;
            Db.CompetitionSeason.Update(entity);
            await Db.SaveChangesAsync();
        }
    }
}
