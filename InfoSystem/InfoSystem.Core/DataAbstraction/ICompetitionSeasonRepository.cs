using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace InfoSystem.Core.DataAbstraction
{
    public interface ICompetitionSeasonRepository
    {
        //Task<List<CompetitionSeasonListing>> GetAllCompetitionSeasons();
        Task<CompetitionSeasonDetails> GetCompetitionSeasonById(int id);
        Task<int> CreateCompetitionSeason(CompetitionSeasonCreateData data);
        Task<CompetitionSeasonDetails> GetCompetitionSeasonBySeasonAndCompetitionId(int seasonId, int competitionId);
        Task<int> EditCompetitionSeason(int id, CompetitionSeasonCreateData data);
        Task DeleteById(int id);
        Task<List<CompetitionSeasonListing>> GetByCompetitionId(int competitionId);
        Task<CompetitionSeasonSelection> GetSelectionByCompetitionId(int competitionId);
        Task<CompetitionSeasonAccreditation> GetCompetitionSeasonAccreditationByCompetitionSeasonIdAndUserId(int competitionSeasonId, string userId);
        Task UpdateGoogleDriveDirection(int competitionSeasonid, string id);
        Task<List<CompetitionSeasonListing>> GetNextCompetitionSeason();
    }

    public class CompetitionSeasonListing
    {
        public int CompetitionSeasonId { get; set; }
        public string OfficialSeasonName { get; set; }
        public SeasonListing Season { get; set; }
        public string PictureUrl { get; set; }
    }
    public class CompetitionSeasonDetails
    {
        public int CompetitionSeasonId { get; set; }
        public Uri PictureStoreUri { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public DateTime JournalistRegistrationStart { get; set; }
        public DateTime JournalistRegistrationEnd { get; set; }
        public DateTime JournalistUploadFotoDeadline { get; set; }
        public CompetitionListing Competition { get; set; }
        public SeasonListing Season { get; set; }
    }
    public class CompetitionSeasonCreateData
    {
        public int CompetitionId { get; set; }
        public int SeasonId { get; set; }
        public Uri PictureStoreUri { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public DateTime JournalistRegistrationStart { get; set; }
        public DateTime JournalistRegistrationEnd { get; set; }
        public DateTime JournalistUploadFotoDeadline { get; set; }
        public CompetitionListing Competition { get; set; }
        public SeasonListing Season { get; set; }
        public string OfficialSeasonName { get; set; }
    }


    public class CompetitionSeasonSelection
    {
        public List<CompetitionSeasonListing> Seasons { get; set; }
        public ChampionshipListing Championship { get; set; }
        public CompetitionListing Competition { get; set; }
        public SportListing Sport { get; set; }

    }

    public class CompetitionSeasonAccreditation
    {
        public int CompetitionSeasonId { get; set; }
        public string OfficialSeasonName { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public DateTime JournalistRegistrationStart { get; set; }
        public DateTime JournalistRegistrationEnd { get; set; }
        public DateTime JournalistUploadFotoDeadline { get; set; }
        public SeasonListing Season { get; set; }
        public ChampionshipListing Championship { get; set; }
        public CompetitionListing Competition { get; set; }
        public SportListing Sport { get; set; }
        public AccreditationListing Accreditation { get; set; }
    }
}
