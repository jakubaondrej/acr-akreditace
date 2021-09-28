using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InfoSystem.Core.Championships;
using InfoSystem.Core.Competitions;
using InfoSystem.Core.CompetitionSeasons;
using InfoSystem.Core.DataAbstraction;
using InfoSystem.Core.Seasons;
using InfoSystem.Core.Users;
using InfoSystem.Web.Models.championship;
using InfoSystem.Web.Models.competition;
using InfoSystem.Web.Models.CompetitionSeason;
using InfoSystem.Web.Models.Season;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace InfoSystem.Web.Controllers
{
    [Authorize]
    public class CompetitionController : Controller
    {
        private readonly ILogger<CompetitionController> _logger;
        private UserService _userService;
        private CompetitionService _competitionService;
        private readonly ChampionshipService _championshipService;
        private readonly CompetitionSeasonService _competitionSeasonService;

        public CompetitionController(ILogger<CompetitionController> logger, UserService userService,
            CompetitionService competitionService,
            ChampionshipService championshipService,
            CompetitionSeasonService competitionSeasonService)
        {
            _logger = logger;
            logger.LogInformation("CompetitionController initialize");
            _userService = userService;
            _competitionService = competitionService;
            _championshipService = championshipService;
            _competitionSeasonService = competitionSeasonService;
        }

        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> Index()
        {
            var list = await _competitionService.GetAll();
            var model = list.Select(c => new CompetitionListingModel()
            {
                CompetitionId = c.CompetitionId,
                Name = c.Name
            });
            return View(model);
        }

        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> Create()
        {
            var championships = await _championshipService.GetAllChampionships();
            var model = new CompetitionCreateModel();
            model.Championships = championships.Select(c => new ChampionshipListingModel()
            {
                Id = c.Id,
                Name = c.Name
            });
            return View(model);
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CompetitionCreateModel model)
        {
            if (ModelState.IsValid)
            {
                var data = new CompetitionCreateData()
                {
                    ChampionshipId = model.SelectedChampionshipId,
                    Description = model.Description,
                    Name = model.Name,
                    ImageUrl = model.ImageUrl
                };
                await _competitionService.Create(data);
                return RedirectToAction("Index");
            }

            var championships = await _championshipService.GetAllChampionships();
            model.Championships = championships.Select(c => new ChampionshipListingModel()
            {
                Id = c.Id,
                Name = c.Name
            });
            return View(model);
        }

        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> Edit(int id)
        {
            var comp = await _competitionService.GetById(id);
            if (comp == null)
            {
                Response.StatusCode = 404;
                return View("NotFound", id);
            }

            var championships = await _championshipService.GetAllChampionships();
            var model = new CompetitionCreateModel()
            {
                Description = comp.Description,
                Name = comp.Name,
                SelectedChampionshipId = comp.CompetitionId,
                ImageUrl = comp.ImageUrl
            };
            model.Championships = championships.Select(c => new ChampionshipListingModel()
            {
                Id = c.Id,
                Name = c.Name
            });
            return View(model);
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CompetitionCreateModel model, int id)
        {
            if (ModelState.IsValid)
            {
                var comp = await _competitionService.GetById(id);
                if (comp == null)
                {
                    Response.StatusCode = 404;
                    return View("NotFound", id);
                }
                var data = new CompetitionCreateData()
                {
                    ChampionshipId = model.SelectedChampionshipId,
                    Description = model.Description,
                    Name = model.Name,
                    ImageUrl = model.ImageUrl
                };
                await _competitionService.Edit(id, data);
                return RedirectToAction("Index");
            }
            var championships = await _championshipService.GetAllChampionships();
            model.Championships = championships.Select(c => new ChampionshipListingModel()
            {
                Id = c.Id,
                Name = c.Name
            });
            return View(model);
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var comp = await _competitionService.GetById(id);
            if (comp == null)
            {
                Response.StatusCode = 404;
                return View("NotFound", id);
            }
            await _competitionService.DeleteById(id);
            return RedirectToAction("Index");
        }

        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> Details(int id)
        {
            var comp = await _competitionService.GetById(id);
            if (comp == null)
            {
                Response.StatusCode = 404;
                return View("NotFound", id);
            }
            var seasons = await _competitionSeasonService.GetByCompetitionId(id);
            var model = new CompetitionDetailsModel()
            {
                Name = comp.Name,
                Championship = new ChampionshipListingModel()
                {
                    Id = comp.Championship.Id,
                    Name = comp.Championship.Name
                },
                CompetitionId = comp.CompetitionId,
                Description = comp.Description,
                ImageUrl = comp.ImageUrl,
                Seasons = seasons.Select(s => new CompetitionSeasonListingModel()
                {
                    CompetitionSeasonId = s.CompetitionSeasonId,
                    Season = new SeasonListingModel()
                    {
                        SeasonId = s.Season.SeasonId,
                        Year = s.Season.Year
                    },
                    OfficialSeasonName = s.OfficialSeasonName,
                    PictureUrl = s.PictureUrl
                })
                .ToList()
            };
            return View(model);
        }

        public async Task<IActionResult> SelectCompetition(int id)
        {
            var data = await _competitionService.GetCompetitionSelectionByChampionshipId(id);
            var model = new CompetitionSelectModel()
            {
                Competitions = data.Competitions.Select(c => new CompetitionSelecListingtModel()
                {
                    CompetitionId = c.CompetitionId,
                    Name = c.Name,
                    ImageUrl = c.ImageUrl
                }).ToList(),
                Championship = new ChampionshipListingModel()
                {
                    Id = data.Championship.Id,
                    Name = data.Championship.Name
                },
                Sport = new Models.Sport.SportListingModel()
                {
                    Name = data.Sport.Name,
                    Id = data.Sport.Id
                }
            };
            return View(model);
        }
    }
}