using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace InfoSystem.Core.DataAbstraction
{
    public interface IAccreditationRepository
    {
        Task<List<AccreditationListing>> GetAllAccreditations();
        Task<AccreditationDetails> GetAccreditationById(int id);
        Task<List<AccreditationListing>> GetAccreditationsByCompetitionSeasonId(int competitionSeasonId);
        Task CreateAccreditation(AccreditationCreateData data);
        Task<AccreditationDetails> GetAccreditationByCompetitionSeasonIdAndUserId(int competitionSeasonId, string userId);
        Task AcceptAccreditationById(int id);
        Task EditAccreditation(int id, AccreditationEditation data);
    }

    public class AccreditationListing
    {
        public int AccreditationId { get; set; }
        public UserListing User { get; set; }
        public string State { get; set; }
        public bool Close { get; set; }
    }

    public class AccreditationDetails
    {
        public int AccreditationId { get; set; }
        public UserListing User { get; set; }
        public int CompetitionSeasonId { get; set; }
        public AccredationHeaderInfo AccredationHeaderInfo { get; set; }
        public string State { get; set; }
        public string Note { get; set; }
        public bool Close { get; set; }
    }

    public class AccreditationCreateData
    {
        public string UserId { get; set; }
        public int CompetitionSeasonId { get; set; }
        public string Note { get; set; }
    }

    public class AccredationHeaderInfo
    {
        public string CompetitionName { get; set; }
        public int SeasonYear { get; set; }
    }

    public class AccreditationEditation
    {
        public string State { get; set; }
        public string Note { get; set; }
        public bool Close { get; set; }
    }
}
