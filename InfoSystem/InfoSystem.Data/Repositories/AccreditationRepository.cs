using InfoSystem.Core.Accreditations;
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
    public class AccreditationRepository : RepositoryBase, IAccreditationRepository
    {
        public AccreditationRepository(ApplicationDbContext applicationDbContext)
            : base(applicationDbContext)
        {

        }

        public async Task AcceptAccreditationById(int id)
        {
            var entity = await Db.Accreditation.Where(a => a.AccreditationId == id)
                .SingleOrDefaultAsync();
            entity.State = AccreditationStates.Accepted;
            Db.Accreditation.Update(entity);
            await Db.SaveChangesAsync();
        }

        public async Task CreateAccreditation(AccreditationCreateData data)
        {
            var entity = new Accreditation()
            {
                UserId = data.UserId,
                CompetitionSeasonId=data.CompetitionSeasonId,
                State = AccreditationStates.InProcess,
                Close=false,
                Note=data.Note
            };
            Db.Accreditation.Add(entity);
            await Db.SaveChangesAsync();
            //return entity.AccreditationId;
        }

        public async Task EditAccreditation(int id, AccreditationEditation data)
        {
            var entity = await Db.Accreditation
                .Where(a => a.AccreditationId == id)
                .SingleOrDefaultAsync();
            if (entity == null)
            {
                throw new Exception($"Accreditation does not exist. id = {id}");
            }
            entity.Close = data.Close;
            entity.Note = data.Note;
            entity.State = data.State;

            Db.Accreditation.Update(entity);
            await Db.SaveChangesAsync();
        }

        public async Task<AccreditationDetails> GetAccreditationByCompetitionSeasonIdAndUserId(int competitionSeasonId, string userId)
        {
            return await Db.Accreditation
                .Where(a => a.CompetitionSeasonId == competitionSeasonId && a.UserId == userId)
                .Select(a => new AccreditationDetails()
                {
                    User = new UserListing()
                    {
                        Id = a.UserId,
                        Username = a.User.UserName
                    },
                    AccreditationId = a.AccreditationId,
                    Close = a.Close,
                    CompetitionSeasonId= a.CompetitionSeasonId,
                    AccredationHeaderInfo = new AccredationHeaderInfo()
                    {
                        SeasonYear=a.CompetitionSeason.Season.Year,
                        CompetitionName = a.CompetitionSeason.Competition.Name
                    },
                    Note = a.Note,
                    State = a.State
                })
                .SingleOrDefaultAsync();
        }

        public async Task<AccreditationDetails> GetAccreditationById(int id)
        {
            return await Db.Accreditation
                .Where(a => a.AccreditationId==id)
                .Select(a => new AccreditationDetails()
                {
                    User = new UserListing()
                    {
                        Id = a.UserId,
                        Username = a.User.UserName
                    },
                    AccreditationId = a.AccreditationId,
                    Close = a.Close,
                    CompetitionSeasonId = a.CompetitionSeasonId,
                    AccredationHeaderInfo = new AccredationHeaderInfo()
                    {
                        SeasonYear = a.CompetitionSeason.Season.Year,
                        CompetitionName = a.CompetitionSeason.Competition.Name
                    },
                    Note = a.Note,
                    State = a.State
                })
                .SingleOrDefaultAsync();
        }

        public async Task<List<AccreditationListing>> GetAccreditationsByCompetitionSeasonId(int competitionSeasonId)
        {
            return await Db.Accreditation.Where(a => a.CompetitionSeasonId == competitionSeasonId)
                .Select(a => new AccreditationListing()
                {
                    AccreditationId = a.AccreditationId,
                    Close = a.Close,
                    State = a.State,
                    User = new UserListing()
                    {
                        Id = a.UserId,
                        Username = a.User.UserName
                    }
                })
                .ToListAsync();
        }

        public Task<List<AccreditationListing>> GetAllAccreditations()
        {
            throw new NotImplementedException();
        }
    }
}
