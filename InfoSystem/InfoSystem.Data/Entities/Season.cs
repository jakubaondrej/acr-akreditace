using System;
using System.Collections.Generic;
using System.Text;

namespace InfoSystem.Data.Entities
{
    public class Season
    {
        public int SeasonId { get; set; }
        public int Year { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public List<Competition> Competitions { get; set; }
    }
}
