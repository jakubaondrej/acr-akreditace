using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace InfoSystem.Core.DataAbstraction
{
    public interface IDriveRepository
    {
        Task SaveRedactorReportData(RedactorReportUpload redactorReportUpload);
        Task<Folder> GetUserFolderId(string userId);
        Task<Folder> GetCompetitionSeasonFolderById(int id);
    }

    public class Folder
    {
        public string  FolderId { get; set; }
        public string Name { get; set; }
    }

    public class RedactorReportUpload
    {
        public string UserId { get; set; }
        public string FileId { get; set; }
        public int CompetitionSeasonId { get; set; }

    }
}
