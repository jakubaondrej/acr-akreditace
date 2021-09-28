using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace InfoSystem.Core.DataAbstraction
{
    public interface IChampionshipRepository
    {
        Task Delete(int id);
        Task<ChampionshipDetails> GetChampionshipByName(string name);
        Task<int> CreateChampionship(ChampionshipCreateData data);
        Task<ChampionshipDetails> GetChampionshipById(int id);
        Task<int> EditChampionship(int id, ChampionshipCreateData data);
        Task<List<ChampionshipListing>> GetAllChampionships();
        Task<ChampionshipSelection> GetChampionshipSelectionBySportId(int id);
    }
    public class ChampionshipCreateData
    {
        public string Name { get; set; }
        public int SportId { get; set; }
        public Uri ImageUrl { get; set; }
    }

    public class ChampionshipListing
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Uri ImageUrl { get; set; }
    }

    public class ChampionshipDetails
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public SportListing Sport { get; set; }
        public Uri ImageUrl { get; set; }
    }

    public class ChampionshipSelection
    {
        public List<ChampionshipListing> Championships { get; set; }
        public SportListing Sport { get; set; }
    }
}
