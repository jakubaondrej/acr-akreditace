using InfoSystem.Core.GoogleDrive;
using InfoSystem.Web.Models.CompetitionSeason;
using InfoSystem.Web.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InfoSystem.Web.Models.Accreditation
{
    public class AccreditationDetailsModel
    {
        public int AccreditationId { get; set; }
        public UserListingModel User { get; set; }
        public int CompetitionSeasonId { get; set; }
        public AccredationHeaderInfoModel HeaderInfo { get; set; }
        public string State { get; set; }
        public string Note { get; set; }
        public List<GoogleFileView> files { get; set; } //TODO Create as GoogleFileModel
        public bool Close { get; set; }
    }

    public class AccredationHeaderInfoModel
    {
        public string CompetitionName { get; set; }
        public int SeasonYear { get; set; }
    }
}
