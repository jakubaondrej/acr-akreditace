using InfoSystem.Core.DataAbstraction;
using InfoSystem.Core.RedactorViewPaparaziMedia;
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
    public class RedactorViewPaparaziMediaRepository : RepositoryBase, IRedactorViewPaparaziMediaRepository
    {
        public RedactorViewPaparaziMediaRepository(ApplicationDbContext applicationDbContext)
                    : base(applicationDbContext)
        {

        }

        public async Task ChangeStatusById(int id, string status)
        {
            var entity = await Db.RedactorViewPaparaziMedia
                .Where(r => r.RedactorViewPaparaziMediaId == id)
                .SingleOrDefaultAsync();
            entity.Status = status;
            Db.RedactorViewPaparaziMedia.Update(entity);
            await Db.SaveChangesAsync();
        }

        public async Task CreateNew(string userId, int competitionSeasonId)
        {
            var entity = new RedactorViewPaparaziMedia()
            {
                CompetitionSeasonId = competitionSeasonId,
                UserId = userId,
                Status = RedactorViewPaparaziMediaStatus.New
            };
            Db.RedactorViewPaparaziMedia.Add(entity);
            await Db.SaveChangesAsync();
        }

        public async Task<List<RedactorViewPaparaziMediaListing>> GetAll()
        {
            return await Db.RedactorViewPaparaziMedia
                .Select(r => new RedactorViewPaparaziMediaListing()
                {
                    CompetitionSeason = new CompetitionSeasonListing()
                    {
                        CompetitionSeasonId = r.CompetitionSeasonId,
                        Season = new SeasonListing()
                        {
                            SeasonId = r.CompetitionSeason.SeasonId,
                            Year = r.CompetitionSeason.Season.Year
                        },
                        OfficialSeasonName=r.CompetitionSeason.OfficialSeasonName,
                        PictureUrl = r.CompetitionSeason.PictureStoreUri.ToString()
                    },
                    Id = r.RedactorViewPaparaziMediaId,
                    Status = r.Status,
                    Username = r.User.UserName
                })
                .ToListAsync();
        }

        public async Task<bool> HasAcceptedRequest(int competitionSeasonId, string userId)
        {
            return await Db.RedactorViewPaparaziMedia
                .AnyAsync(r =>
                    r.CompetitionSeasonId == competitionSeasonId
                    && r.UserId == userId
                    && r.Status == RedactorViewPaparaziMediaStatus.Accpeted);
        }
    }
}
