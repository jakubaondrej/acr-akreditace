using InfoSystem.Web.Models.Season;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InfoSystem.Web.Models.Sport
{
    public class SportSelectionListModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Uri ImageUrl { get; set; }
    }
}
