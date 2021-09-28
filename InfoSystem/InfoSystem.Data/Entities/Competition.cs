using System;
using System.Collections.Generic;
using System.Text;

namespace InfoSystem.Data.Entities
{
    public class Competition //Soutěž
    {
        public int CompetitionId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Uri ImageUri { get; set; }
        public int ChampionshipId { get; set; }
        public Championship Championship { get; set; }
        public virtual List<CompetitionSeason> CompetitionSeasons { get; set; }
    }
}
