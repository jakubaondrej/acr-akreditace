using InfoSystem.Web.Models.Season;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InfoSystem.Web.Models.CompetitionSeason
{
    public class CompetitionSeasonSelectionModel
    {
        public championship.ChampionshipListingModel Championship { get; set; }
        public Sport.SportListingModel Sport { get; set; }
        public competition.CompetitionListingModel Competition { get; set; }
        public List<CompetitionSeasonListingModel> competitionSeasonListings { get; set; }
    }

    public class CompetitionSeasonSelectionListingModel
    {
        public int CompetitionSeasonId { get; set; }
        public SeasonListingModel Season { get; set; }
    }
}
