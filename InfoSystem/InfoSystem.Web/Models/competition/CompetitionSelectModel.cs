using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InfoSystem.Web.Models.competition
{
    public class CompetitionSelectModel
    {
        public championship.ChampionshipListingModel Championship { get; set; }
        public Sport.SportListingModel Sport { get; set; }
        public List<CompetitionSelecListingtModel> Competitions { get; set; }
    }

    public class CompetitionSelecListingtModel
    {
        public int CompetitionId { get; set; }
        public string Name { get; set; }
        public Uri ImageUrl { get; set; }
    }
}
