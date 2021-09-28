using System;
using System.Collections.Generic;
using System.Text;

namespace InfoSystem.Data.Entities
{
    public class CompetitionSeason
    {
        public int CompetitionSeasonId { get; set; }
        public int CompetitionId { get; set; }
        public int SeasonId { get; set; }
        public string OfficialSeasonName { get; set; }
        public string DriveFolderId { get; set; }
        public Uri PictureStoreUri { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public DateTime JournalistRegistrationStart { get; set; }
        public DateTime JournalistRegistrationEnd { get; set; }
        public DateTime JournalistUploadFotoDeadline { get; set; }
        public Competition Competition { get; set; }
        public Season Season { get; set; }
        public List<Accreditation> Accreditations { get; set; }
        public List<RedactorReport> RedactorReports { get; set; }
    }
}
