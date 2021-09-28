using System;
using System.Collections.Generic;
using System.Text;

namespace InfoSystem.Data.Entities
{
    public class RedactorReport
    {
        public int RedactorReportId { get; set; }
        public string DriveFileId { get; set; }
        public User User { get; set; }
        public string UserId { get; set; }
        public CompetitionSeason CompetitionSeason {get;set;}
        public int CompetitionSeasonId { get; set; }
    }
}
