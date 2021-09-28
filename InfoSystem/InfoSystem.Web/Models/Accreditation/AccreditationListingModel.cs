using InfoSystem.Core.GoogleDrive;
using InfoSystem.Web.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InfoSystem.Web.Models.Accreditation
{
    public class AccreditationListingModel
    {
        public int AccreditationId { get; set; }
        public UserListingModel User { get; set; }
        public string State { get; set; }
        public bool Close { get; set; }
        public List<GoogleFileView> files { get; set; } //TODO Create as GoogleFileModel
    }
}
