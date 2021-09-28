using System;
using System.Collections.Generic;
using System.Text;

namespace InfoSystem.Data.Entities
{
    public class Championship //Mistrovství 
    {
        public int ChampionshipId { get; set; }
        public string Name { get; set; }
        public Uri ImageUrl { get; set; }
        public int SportId { get; set; }
        public Sport Sport { get; set; }
        public List<Competition> Competitions { get; set; } = new List<Competition>();
    }
}
