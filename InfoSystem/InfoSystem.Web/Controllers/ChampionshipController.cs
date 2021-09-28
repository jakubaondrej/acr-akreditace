using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InfoSystem.Core.Championships;
using InfoSystem.Core.DataAbstraction;
using InfoSystem.Core.Sports;
using InfoSystem.Core.Users;
using InfoSystem.Web.Models.championship;
using InfoSystem.Web.Models.Sport;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace InfoSystem.Web.Controllers
{
    [Authorize]
    public class ChampionshipController : Controller
    {
        private readonly ILogger<ChampionshipController> _logger;
        private UserService _userService;
        private ChampionshipService _championshipService;
        private readonly SportService _sportService;

        public ChampionshipController(ILogger<ChampionshipController> logger, UserService userService,
            ChampionshipService championshipService,
            SportService sportService)
        {
            _logger = logger;
            _userService = userService;
            _championshipService = championshipService;
            _sportService = sportService;
        }

        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> Index()
        {
            var list = await _championshipService.GetAllChampionships();
            var model = list.Select(c => new ChampionshipListingModel()
            {
                Id = c.Id,
                Name = c.Name
            });
            return View(model);
        }

        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> Create()
        {
            var sports = await _sportService.GetAllSports();
            var model = new ChampionshipCreateModel();
            model.Sports = sports.Select(s => new SportListingModel()
            {
                Id = s.Id,
                Name = s.Name
            })
                                .ToList();
            return View(model);
        }

        [Authorize(Roles = Roles.Admin)]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Create(ChampionshipCreateModel model)
        {
            if (ModelState.IsValid)
            {
                var data = new ChampionshipCreateData()
                {
                    Name = model.Name,
                    SportId = model.SelectedSportId,
                    ImageUrl = new Uri(model.ImageUrl)
                };
                await _championshipService.Create(data);
                return RedirectToAction("Index");
            }
            var sports = await _sportService.GetAllSports();
            model.Sports = sports.Select(s => new SportListingModel()
            {
                Id = s.Id,
                Name = s.Name
            })
                .ToList();
            return View(model);
        }

        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> Edit(int id)
        {
            var champ = await _championshipService.GetChampionshipById(id);
            var sports = await _sportService.GetAllSports();
            var model = new ChampionshipCreateModel()
            {
                Name = champ.Name,
                SelectedSportId = champ.Sport.Id,
                ImageUrl = champ.ImageUrl?.ToString()
            };
            model.Sports = sports.Select(s => new SportListingModel()
            {
                Id = s.Id,
                Name = s.Name
            }).ToList();
            return View(model);
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ChampionshipCreateModel model, int id)
        {
            if (ModelState.IsValid)
            {
                var data = new ChampionshipCreateData()
                {
                    Name = model.Name,
                    SportId = model.SelectedSportId,
                    ImageUrl = new Uri(model.ImageUrl)
                };
                await _championshipService.Edit(id, data);
                return RedirectToAction("Index");
            }
            var sports = await _sportService.GetAllSports();
            model.Sports = sports.Select(s => new SportListingModel()
            {
                Id = s.Id,
                Name = s.Name,
            })
                .ToList();
            return View(model);
        }

        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> Details(int id)
        {
            var champ = await _championshipService.GetChampionshipById(id);
            if (champ == null)
            {
                Response.StatusCode = 404;
                return View("NotFound", id);
            }
            var model = new ChampionshipDetailModel()
            {
                Name = champ.Name,
                Id = champ.Id,
                Sport = new SportListingModel()
                {
                    Id = champ.Sport.Id,
                    Name = champ.Sport.Name
                },
                ImageUrl = champ.ImageUrl
            };
            return View(model);
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var champ = await _championshipService.GetChampionshipById(id);
            if (champ == null)
            {
                Response.StatusCode = 404;
                return View("NotFound", id);
            }
            await _championshipService.Delete(id);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> SelectChampionship(int id)
        {
            var champ = await _championshipService.GetChampionshipSelectionBySportId(id);
            if (champ == null)
            {
                Response.StatusCode = 404;
                return View("NotFound", id);
            }
            var model = new ChampionshipSelectModel()
            {
                Sport = new SportListingModel()
                {
                    Name = champ.Sport.Name,
                    Id = champ.Sport.Id
                },
                Championships = champ.Championships.Select(c =>
             new ChampionshipSelectListingModel()
             {
                 Name = c.Name,
                 Id = c.Id,
                 ImageUrl = c.ImageUrl?.ToString()
             }).ToList()
            };
            return View(model);
        }
    }
}