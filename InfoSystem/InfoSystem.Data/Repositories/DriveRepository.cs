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
    public class DriveRepository : RepositoryBase, IDriveRepository
    {
        public DriveRepository(ApplicationDbContext applicationDbContext)
           : base(applicationDbContext)
        {

        }

        public async Task<Folder> GetCompetitionSeasonFolderById(int id)
        {
            return await Db.CompetitionSeason
                .Where(c => c.CompetitionSeasonId == id)
                .Select(c => new Folder()
                {
                    FolderId = c.DriveFolderId,
                    Name = $"{c.Competition.Name}_{c.Season.Year}"
                })
                .SingleOrDefaultAsync();
        }

        public async Task<Folder> GetUserFolderId(string userId)
        {
            return await Db.User
                .Where(u => u.Id == userId)
                .Select(u => new Folder()
                {
                    FolderId = u.GoogleDriveDirectoryId,
                    Name = u.UserName
                })
                .SingleOrDefaultAsync();
        }

        public async Task SaveRedactorReportData(RedactorReportUpload redactorReportUpload)
        {
            var entity = new RedactorReport()
            {
                CompetitionSeasonId = redactorReportUpload.CompetitionSeasonId,
                DriveFileId = redactorReportUpload.FileId,
                UserId = redactorReportUpload.UserId
            };
            Db.RedactorReport.Add(entity);
            await Db.SaveChangesAsync();
        }
    }
}
