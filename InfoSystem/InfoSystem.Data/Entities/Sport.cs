using System;
using System.Collections.Generic;
using System.Text;

namespace InfoSystem.Data.Entities
{
    public class Sport //Disciplína 
    {
        public int SportId { get; set; }
        public string Name { get; set; }
        public Uri ImageUri { get; set; }
        public List<Championship> Championships { get; set; }
    }
}
