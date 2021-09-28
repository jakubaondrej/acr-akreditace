using System;
using System.Collections.Generic;
using System.Text;

namespace InfoSystem.Core.Redactions
{
    public class RedactionCreateData
    {
        public string Name { get; set; }
        public string GeneralEditor { get; set; }
        public string GeneralEditorCallNumber { get; set; }
        public string GeneralEditorEmail { get; set; }
        public string Note { get; set; }
        public string Link { get; set; }
    }
}
