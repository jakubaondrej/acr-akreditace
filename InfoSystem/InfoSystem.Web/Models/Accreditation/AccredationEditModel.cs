using InfoSystem.Core.Accreditations;
using InfoSystem.Web.Models.Users;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InfoSystem.Web.Models.Accreditation
{
    public class AccredationEditModel
    {
        public int CompetitionSeasonId { get; set; }
        public UserListingModel User { get; set; }
        public AccredationHeaderInfoModel HeaderInfo { get; set; }
        public string State { get; set; }
        public string Note { get; set; }
        public bool Close { get; set; }

        public IEnumerable<SelectListItem> StateItems
        {
            get
            {
                var items = AccreditationStates.GetAll().Select(a => new SelectListItem(a, a)).ToList();
                return items;
            }
        }
    }
}
