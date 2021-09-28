using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace InfoSystem.Core.DataAbstraction
{
    public interface ISportRepository
    {
        Task<SportDetail> GetSportByName(string name);
        Task<int> CreateSport(SportCreateData data);
        Task<SportDetail> GetSportById(int id);
        Task Delete(int id);
        Task<int> EditSport(int id, SportCreateData data);
        Task<List<SportListing>> GetAllSports();
        Task<List<SportListing>> GetSportsBySeasonId(int seasonId);
    }
    public class SportListing 
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Uri ImageUri { get; set; }
    }
    public class SportDetail 
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<ChampionshipListing> Championships { get; set; }
        public Uri ImageUri { get; set; }
    }
    public class SportCreateData 
    {
        public string Name { get; set; }
        public Uri ImageUri { get; set; }
    }
}
