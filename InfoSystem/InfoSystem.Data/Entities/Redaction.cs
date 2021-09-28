using System;
using System.Collections.Generic;
using System.Text;

namespace InfoSystem.Data.Entities
{
    public class Redaction
    {
        public int RedactionId { get; set; }
        public string Name { get; set; }
        public string GeneralEditor { get; set; }
        public string GeneralEditorCallNumber { get; set; }
        public string GeneralEditorEmail { get; set; }
        public string Note { get; set; }
        public string Link { get; set; }
        public IList<User> Users { get; set; }
    }
}
