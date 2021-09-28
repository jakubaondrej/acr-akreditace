using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InfoSystem.Core.RedactorViewPaparaziMedia;
using InfoSystem.Core.Users;
using InfoSystem.Web.Models.RedactorViewPaparaziMedia;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace InfoSystem.Web.Controllers
{
    [Authorize]
    public class RedactorViewPaparaziMediaController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly RedactorViewPaparaziMediaService _redactorViewPaparaziMediaService;

        public RedactorViewPaparaziMediaController(ILogger<HomeController> logger
            , RedactorViewPaparaziMediaService redactorViewPaparaziMediaService)
        {
            _logger = logger;
            this._redactorViewPaparaziMediaService = redactorViewPaparaziMediaService;
        }
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> Index()
        {
            var list = await _redactorViewPaparaziMediaService.GetAll();
            var model = list.Select(r => new RedactorViewPaparaziMediaListingModel()
            {
                CompetitionSeason = new Models.CompetitionSeason.CompetitionSeasonListingModel()
                {
                    CompetitionSeasonId = r.CompetitionSeason.CompetitionSeasonId,
                    Season = new Models.Season.SeasonListingModel()
                    {
                        SeasonId = r.CompetitionSeason.Season.SeasonId,
                        Year = r.CompetitionSeason.Season.Year
                    },
                    OfficialSeasonName = r.CompetitionSeason.OfficialSeasonName,
                    PictureUrl = r.CompetitionSeason.PictureUrl
                },
                Status = r.Status,
                Id = r.Id,
                Username = r.Username
            }); ;
            return View(model);
        }
        [Authorize(Roles = Roles.Admin)]
        [HttpPost]
        public async Task<IActionResult> AcceptRequest(int id)
        {
            await _redactorViewPaparaziMediaService.ChangeStatusById(id, RedactorViewPaparaziMediaStatus.Accpeted);
            return RedirectToAction("Index");
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpPost]
        public async Task<IActionResult> DenyRequest(int id)
        {
            await _redactorViewPaparaziMediaService.ChangeStatusById(id, RedactorViewPaparaziMediaStatus.Accpeted);
            return RedirectToAction("Index");
        }
    }
}