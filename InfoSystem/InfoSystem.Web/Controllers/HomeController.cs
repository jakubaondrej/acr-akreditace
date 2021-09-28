using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using InfoSystem.Web.Models;
using InfoSystem.Core.Users;
using Microsoft.AspNetCore.Authorization;
using InfoSystem.Core.GoogleDrive;
using InfoSystem.Core.DataAbstraction;
using InfoSystem.Core.CompetitionSeasons;
using InfoSystem.Web.Models.CompetitionSeason;

namespace InfoSystem.Web.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private UserService _userService;
        private readonly CompetitionSeasonService _competitionSeasonService;

        public HomeController(ILogger<HomeController> logger
            , UserService userService
            , CompetitionSeasonService competitionSeasonService)
        {
            _competitionSeasonService = competitionSeasonService;
            _logger = logger;
            _userService = userService;
            _logger.LogInformation("HomeController - Initialize Done");
        }
        [AllowAnonymous]
        public async Task<IActionResult> IndexAsync()
        {
            var cs = await _competitionSeasonService.GetNextCompetitionSeasons();
            var model = cs.Select(c => new CompetitionSeasonListingModel()
            {
                CompetitionSeasonId = c.CompetitionSeasonId,
                OfficialSeasonName = c.OfficialSeasonName,
                PictureUrl = c.PictureUrl,
                Season = new Models.Season.SeasonListingModel()
                {
                    SeasonId = c.Season.SeasonId,
                    Year = c.Season.Year
                }
            }).ToList();

            _logger.LogInformation("Action - Home");
            return View(model);
        }
        [AllowAnonymous]
        public IActionResult Privacy()
        {
            return View();
        }
        [AllowAnonymous]
        public IActionResult Documents()
        {
            return View();
        }
        [AllowAnonymous]
        public IActionResult Contacts()
        {
            return View();
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}
