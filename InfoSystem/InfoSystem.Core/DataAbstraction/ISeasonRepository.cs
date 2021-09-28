using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace InfoSystem.Core.DataAbstraction
{
    public interface ISeasonRepository
    {
        Task<SeasonDetails> GetSeasonByYear(int year);
        Task<int> CreateSeason(SeasonCreateData data);
        Task<SeasonDetails> GetSeasonById(int id);
        Task<int> EditSeason(int id, SeasonCreateData data);
        Task<List<SeasonListing>> GetAllSeasons();
        Task Delete(int id);
    }
    public class SeasonCreateData
    {
        public int Year { get; set; }
    }
    public class SeasonListing
    {
        public int SeasonId { get; set; }
        public int Year { get; set; }
    }
    public class SeasonDetails
    {
        public int SeasonId { get; set; }
        public int Year { get; set; }
        public List<CompetitionListing> Competitions { get; set; }
    }
}
