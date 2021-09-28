using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InfoSystem.Core.DataAbstraction;
using InfoSystem.Core.Seasons;
using InfoSystem.Core.Users;
using InfoSystem.Web.Models.Season;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace InfoSystem.Web.Controllers
{
    [Authorize]
    public class SeasonController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private UserService _userService;
        private SeasonService _seasonService;

        public SeasonController(ILogger<HomeController> logger,
            UserService userService,
            SeasonService seasonService)
        {
            _logger = logger;
            _userService = userService;
            _seasonService = seasonService;
        }

        public async Task<IActionResult> Index()
        {
            var list = await _seasonService.GetAllSeasons();
            var model = list.Select(s => new SeasonListingModel()
            {
                SeasonId = s.SeasonId,
                Year = s.Year
            });
            return View(model);
        }

        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> Create()
        {
            var model = new SeasonCreateModel();
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = Roles.Admin)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SeasonCreateModel model)
        {
            if (ModelState.IsValid)
            {
                var data = new SeasonCreateData()
                {
                    Year = model.Year
                };
                await _seasonService.Create(data);
                return RedirectToAction("Index");
            }
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = Roles.Admin)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var s = await _seasonService.GetSeasonById(id);
            if (s == null)
            {
                Response.StatusCode = 404;
                return View("NotFound", id);
            }
            await _seasonService.Delete(id);

            return RedirectToAction("Index");
        }

    }
}