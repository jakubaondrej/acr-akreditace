using InfoSystem.Web.Models.championship;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InfoSystem.Web.Models.Sport
{
    public class SportDetailModel
    {
        public int SportId { get; set; }
        public string Name { get; set; }
        public List<ChampionshipListingModel> championships { get; set; }
        public Uri ImageUri { get; set; }
    }
}
