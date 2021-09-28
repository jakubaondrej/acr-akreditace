using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using InfoSystem.Core.Accreditations;
using InfoSystem.Core.DataAbstraction;
using InfoSystem.Core.GoogleDrive;
using InfoSystem.Core.Users;
using InfoSystem.Data.Entities;
using InfoSystem.Web.Models.Accreditation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace InfoSystem.Web.Controllers
{
    [Authorize]
    public class AccreditationController : Controller
    {
        private UserManager<User> _userManager;
        private readonly AccreditationService _accreditationService;
        private readonly GoogleDriveService _googleDriveService;
        private ILogger<AccreditationController> _logger;
        public AccreditationController(ILogger<AccreditationController> logger,
            UserManager<User> userManager,
            AccreditationService accreditationService,
            GoogleDriveService googleDriveService)
        {
            _logger = logger;
            _userManager = userManager;
            _accreditationService = accreditationService;
            _googleDriveService = googleDriveService;
        }

        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> AccreditationsByCompetitionSeasonId(int id)
        {
            var list = await _accreditationService.GetAccreditationsByCompetitionSeasonId(id);
            var model = list.Select(async a => new AccreditationListingModel()
            {
                User = new Models.Users.UserListingModel()
                {
                    Id = a.User.Id,
                    Name = a.User.Username
                },
                AccreditationId = a.AccreditationId,
                Close = a.Close,
                State = a.State,
               files =await _googleDriveService.RedactorReport(id,a.User.Id)
            }).ToList();

            return View(model);
        }

        /// <summary>
        /// Create accreditation By User
        /// </summary>
        /// <param name="id">CompetitionSeasonId</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateByUser(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var data = new AccreditationCreateData()
            {
                CompetitionSeasonId = id,
                UserId = userId
            };
            await _accreditationService.CreateAccreditation(data);
            return RedirectToAction("CompetitionSeasonAccreditation", "CompetitionSeason", new { id = id });
        }

        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> Details(int id)
        {
            var data = await _accreditationService.GetById(id);
            var model = new AccreditationDetailsModel()
            {
                AccreditationId = data.AccreditationId,
                Close = data.Close,
                CompetitionSeasonId = data.CompetitionSeasonId,
                HeaderInfo = new AccredationHeaderInfoModel()
                {
                    CompetitionName = data.AccredationHeaderInfo.CompetitionName,
                    SeasonYear = data.AccredationHeaderInfo.SeasonYear
                },
                Note = data.Note,
                State = data.State,
                User = new Models.Users.UserListingModel()
                {
                    Id = data.User.Id,
                    Name = data.User.Username
                },
                files =await _googleDriveService.RedactorReport(id,data.User.Id)
            };
            return View(model);
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AcceptAccreditation(int id)
        {
            await _accreditationService.AcceptAccreditationById(id);
            return RedirectToAction("Details", new { id = id });
        }

        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> Edit(int id)
        {
            var data = await _accreditationService.GetById(id);
            if (data == null)
            {
                Response.StatusCode = 404;
                return View("NotFound", id);
            }
            var model = new AccredationEditModel()
            {
                Close = data.Close,
                CompetitionSeasonId = data.CompetitionSeasonId,
                HeaderInfo = new AccredationHeaderInfoModel()
                {
                    CompetitionName = data.AccredationHeaderInfo.CompetitionName,
                    SeasonYear = data.AccredationHeaderInfo.SeasonYear
                },
                Note = data.Note,
                State = data.State,
                User = new Models.Users.UserListingModel()
                {
                    Id = data.User.Id,
                    Name = data.User.Username
                }
            };
            return View(model);
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(AccredationEditModel model, int id)
        {
            if (ModelState.IsValid)
            {
                var data = new AccreditationEditation()
                {
                    Close = model.Close,
                    Note = model.Note,
                    State = model.State
                };
                await _accreditationService.EditAccreditation(id, data);
                return RedirectToAction("AccreditationsByCompetitionSeasonId",new { id = model.CompetitionSeasonId });
            }
            return View(model);
        }
    }
}
