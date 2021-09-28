using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace InfoSystem.Core.DataAbstraction
{
    public interface ICompetitionRepository
    {
        Task<List<CompetitionListing>> GetAllCompetitions();
        Task<CompetitionDetails> GetCompetitionById( int id);
        Task DeleteById(int id);
        Task<CompetitionDetails> GetCompetitionByName(string name);
        Task<int> CreateCompetition(CompetitionCreateData data);
        Task<int> EditCompetition(int id, CompetitionCreateData data);
        Task<CompetitionSelection> GetCompetitionSelectionByChampionshipId(int id);
    }

    public class CompetitionCreateData
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int ChampionshipId { get; set; }
        public Uri ImageUrl { get; set; }
    }

    public class CompetitionDetails
    {
        public int CompetitionId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ChampionshipListing Championship { get; set; }
        public List<CompetitionSeasonListing> CompetitionSeasons { get; set; }
        public Uri ImageUrl { get; set; }
    }

    public class CompetitionListing
    {
        public int CompetitionId { get; set; }
        public string Name { get; set; }
        public Uri ImageUrl { get; set; }
    }

    public class CompetitionSelection
    {
        public ChampionshipListing Championship { get; set; }
        public SportListing Sport { get; set; }
        public List<CompetitionListing> Competitions { get; set; }
    }
}
