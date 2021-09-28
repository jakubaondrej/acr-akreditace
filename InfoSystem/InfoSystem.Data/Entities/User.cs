using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace InfoSystem.Data.Entities
{
    public class User : IdentityUser
    {
        public string RedactorType { get; set; }
        public string Note { get; set; }
        public string GoogleDriveDirectoryId { get; set; }
        public int? RedactionId { get; set; }
        public virtual Redaction Redaction { get; set; }

        public List<Accreditation> Accreditations { get; set; }
        public List<RedactorReport> RedactorReports { get; set; }
        
    }
}
