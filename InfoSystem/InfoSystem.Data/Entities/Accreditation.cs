using System;
using System.Collections.Generic;
using System.Text;

namespace InfoSystem.Data.Entities
{
    public class Accreditation
    {
        public int AccreditationId { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        public int CompetitionSeasonId { get; set; }
        public CompetitionSeason CompetitionSeason { get; set; }
        public string State { get; set; }
        public string Note { get; set; }
        public bool Close { get; set; }
    }
}
