using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InfoSystem.Web.Models.championship
{
    public class ChampionshipSelectModel
    {
        public Sport.SportListingModel Sport { get; set; }

        public List<ChampionshipSelectListingModel> Championships { get; set; }
    }
    public class ChampionshipSelectListingModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
    }
}
