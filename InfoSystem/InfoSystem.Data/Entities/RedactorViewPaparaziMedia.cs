using System;
using System.Collections.Generic;
using System.Text;

namespace InfoSystem.Data.Entities
{
    public class RedactorViewPaparaziMedia
    {
        public int RedactorViewPaparaziMediaId { get; set; }
        public string Status { get; set; }
        public string UserId { get; set; }
        public int CompetitionSeasonId { get; set; }
        public virtual User User { get; set; }
        public virtual CompetitionSeason CompetitionSeason { get; set; }
    }
}
