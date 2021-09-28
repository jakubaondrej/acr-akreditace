using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace InfoSystem.Core.DataAbstraction
{
    public interface IRedactorViewPaparaziMediaRepository
    {
        Task CreateNew(string userId, int competitionSeasonId);
        Task<List<RedactorViewPaparaziMediaListing>> GetAll();
        Task ChangeStatusById(int id, string status);
        Task<bool> HasAcceptedRequest(int competitionSeasonId, string userId);
    }
    public class RedactorViewPaparaziMediaListing
    {
        public int Id { get; set; }
        public string Status { get; set; }
        public string Username { get; set; }
        public CompetitionSeasonListing CompetitionSeason { get; set; }
    }
}
