using InfoSystem.Web.Models.championship;
using InfoSystem.Web.Models.CompetitionSeason;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InfoSystem.Web.Models.competition
{
    public class CompetitionDetailsModel
    {
        public int CompetitionId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ChampionshipListingModel Championship { get; set; }
        public List<CompetitionSeasonListingModel> Seasons { get; set; }
        public Uri ImageUrl { get; set; }
    }
}
