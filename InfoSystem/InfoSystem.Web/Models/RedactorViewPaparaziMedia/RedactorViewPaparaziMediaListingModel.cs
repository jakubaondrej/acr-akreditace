using InfoSystem.Web.Models.CompetitionSeason;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InfoSystem.Web.Models.RedactorViewPaparaziMedia
{
    public class RedactorViewPaparaziMediaListingModel
    {
        public int Id { get; set; }
        public string Status { get; set; }
        public string Username { get; set; }
        public CompetitionSeasonListingModel CompetitionSeason { get; set; }
    }
}
