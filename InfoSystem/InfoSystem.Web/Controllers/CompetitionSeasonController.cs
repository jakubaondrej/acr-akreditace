using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using InfoSystem.Core.Competitions;
using InfoSystem.Core.CompetitionSeasons;
using InfoSystem.Core.DataAbstraction;
using InfoSystem.Core.GoogleDrive;
using InfoSystem.Core.RedactorViewPaparaziMedia;
using InfoSystem.Core.Seasons;
using InfoSystem.Core.Users;
using InfoSystem.Data.Entities;
using InfoSystem.Web.Models.CompetitionSeason;
using InfoSystem.Web.Models.Season;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace InfoSystem.Web.Controllers
{
    [Authorize]
    public class CompetitionSeasonController : Controller
    {
        private readonly ILogger<CompetitionSeasonController> _logger;
        private CompetitionService _competitionService;
        private readonly CompetitionSeasonService _competitionSeasonService;
        private readonly SeasonService _seasonService;
        private readonly GoogleDriveService _googleDriveService;
        private readonly SignInManager<User> _signInManager;
        private readonly RedactorViewPaparaziMediaService _redactorViewPaparaziMediaService;

        public CompetitionSeasonController(ILogger<CompetitionSeasonController> logger,
            CompetitionService competitionService,
            CompetitionSeasonService competitionSeasonService,
            SeasonService seasonService,
            GoogleDriveService googleDriveService,
             SignInManager<User> signInManager,
             RedactorViewPaparaziMediaService redactorViewPaparaziMediaService)
        {
            _logger = logger;
            _logger.LogInformation("CompetitionSeasonController - Initialize Start");
            _competitionService = competitionService;
            _competitionSeasonService = competitionSeasonService;
            _seasonService = seasonService;
            _googleDriveService = googleDriveService;
            _signInManager = signInManager;
            this._redactorViewPaparaziMediaService = redactorViewPaparaziMediaService;
            _logger.LogInformation("CompetitionSeasonController - Initialize Done");
        }

        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> Create(int id)
        {
            _logger.LogInformation("Get competition by ID");
            var comp = await _competitionService.GetById(id);
            if (comp == null)
            {
                Response.StatusCode = 404;
                return View("NotFound", id);
            }
            _logger.LogInformation("Get all seasons");
            var seasons = await _seasonService.GetAllSeasons();
            _logger.LogInformation("Create model");
            var model = new CompetitionSeasonCreateModel()
            {
                Seasons = seasons.Any() ? seasons.Select(s => new SeasonListingModel()
                {
                    SeasonId = s.SeasonId,
                    Year = s.Year
                }).ToList()
                : null
            };
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = Roles.Admin)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CompetitionSeasonCreateModel model, int id)
        {
            if (ModelState.IsValid)
            {
                var comp = await _competitionService.GetById(id);
                if (comp == null)
                {
                    Response.StatusCode = 404;
                    return View("NotFound", id);
                }
                var data = new CompetitionSeasonCreateData()
                {
                    SeasonId = model.SelectedSeasonId,
                    CompetitionId = id,
                    End = model.End,
                    JournalistRegistrationEnd = model.JournalistRegistrationEnd,
                    JournalistRegistrationStart = model.JournalistRegistrationStart,
                    JournalistUploadFotoDeadline = model.JournalistUploadFotoDeadline,
                    PictureStoreUri = model.PictureStoreUri,
                    OfficialSeasonName = model.OfficialName,
                    Start = model.Start
                };
                await _competitionSeasonService.Create(data);
                return RedirectToAction("Details", "Competition", new { id = id });
            }
            var seasons = await _seasonService.GetAllSeasons();
            model.Seasons = seasons.Select(s => new SeasonListingModel()
            {
                SeasonId = s.SeasonId,
                Year = s.Year
            }).ToList();
            return View(model);
        }


        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> Details(int id)
        {
            var comp = await _competitionSeasonService.GetById(id);
            if (comp == null)
            {
                Response.StatusCode = 404;
                return View("NotFound", id);
            }
            var model = new CompetitionSeasonDetailsModel()
            {
                Competition = new Models.competition.CompetitionListingModel()
                {
                    CompetitionId = comp.Competition.CompetitionId,
                    Name = comp.Competition.Name
                },
                End = comp.End,
                Id = comp.CompetitionSeasonId,
                JournalistRegistrationEnd = comp.JournalistRegistrationEnd,
                JournalistRegistrationStart = comp.JournalistRegistrationStart,
                JournalistUploadFotoDeadline = comp.JournalistUploadFotoDeadline,
                PictureStoreUri = comp.PictureStoreUri,
                Seasons = new SeasonListingModel()
                {
                    SeasonId = comp.Season.SeasonId,
                    Year = comp.Season.Year
                },
                Start = comp.Start
            };
            return View(model);
        }


        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> Edit(int id)
        {
            var comp = await _competitionSeasonService.GetById(id);
            if (comp == null)
            {
                Response.StatusCode = 404;
                return View("NotFound", id);
            }
            var seasons = await _seasonService.GetAllSeasons();
            var model = new CompetitionSeasonEditModel()
            {
                CompetitionId = comp.Competition.CompetitionId,
                CompetitionName = comp.Competition.Name,
                End = comp.End,
                JournalistRegistrationEnd = comp.JournalistRegistrationEnd,
                JournalistRegistrationStart = comp.JournalistRegistrationStart,
                JournalistUploadFotoDeadline = comp.JournalistUploadFotoDeadline,
                PictureStoreUri = comp.PictureStoreUri,
                Start = comp.Start,
                SelectedSeasonId = comp.Season.SeasonId,
                Seasons = seasons.Select(s => new SeasonListingModel()
                {
                    SeasonId = s.SeasonId,
                    Year = s.Year
                }).ToList()
            };
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = Roles.Admin)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CompetitionSeasonEditModel model, int id)
        {
            if (ModelState.IsValid)
            {
                var comp = await _competitionSeasonService.GetById(id);
                if (comp == null)
                {
                    Response.StatusCode = 404;
                    return View("NotFound", id);
                }
                var data = new CompetitionSeasonCreateData()
                {
                    SeasonId = model.SelectedSeasonId,
                    CompetitionId = model.CompetitionId,
                    End = model.End,
                    JournalistRegistrationEnd = model.JournalistRegistrationEnd,
                    JournalistRegistrationStart = model.JournalistRegistrationStart,
                    JournalistUploadFotoDeadline = model.JournalistUploadFotoDeadline,
                    PictureStoreUri = model.PictureStoreUri,
                    Start = model.Start
                };
                await _competitionSeasonService.Edit(id, data);
                return RedirectToAction("Details", "Competition", new { id = id });
            }
            var seasons = await _seasonService.GetAllSeasons();
            model.Seasons = seasons.Select(s => new SeasonListingModel()
            {
                SeasonId = s.SeasonId,
                Year = s.Year
            }).ToList();
            return View(model);
        }

        public async Task<IActionResult> SelectCompetitionSeason(int id)
        {
            _logger.LogInformation("SelectCompetitionSeason - GetSelectionByCompetitionId");
            var cs = await _competitionSeasonService.GetSelectionByCompetitionId(id);
            _logger.LogInformation("SelectCompetitionSeason - create model");
            var model = new CompetitionSeasonSelectionModel()
            {
                Championship = new Models.championship.ChampionshipListingModel()
                {
                    Id = cs.Championship.Id,
                    Name = cs.Championship.Name
                },
                Competition = new Models.competition.CompetitionListingModel()
                {
                    CompetitionId = cs.Competition.CompetitionId,
                    Name = cs.Competition.Name
                },
                Sport = new Models.Sport.SportListingModel()
                {
                    Id = cs.Sport.Id,
                    Name = cs.Sport.Name
                },
                competitionSeasonListings = cs.Seasons?.Select(s => new CompetitionSeasonListingModel()
                {
                    CompetitionSeasonId = s.CompetitionSeasonId,
                    Season = new SeasonListingModel()
                    {
                        SeasonId = s.Season.SeasonId,
                        Year = s.Season.Year
                    },
                    OfficialSeasonName = s.OfficialSeasonName,
                    PictureUrl = s.PictureUrl
                }).ToList()
            };
            return View(model);
        }

        public async Task<IActionResult> CompetitionSeasonAccreditation(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var cs = await _competitionSeasonService.GetCompetitionSeasonAccreditationById(id, userId);
            var model = new CompetitionSeasonAccreditationUserModel()
            {
                Championship = new Models.championship.ChampionshipListingModel()
                {
                    Id = cs.Championship.Id,
                    Name = cs.Championship.Name
                },
                Competition = new Models.competition.CompetitionListingModel()
                {
                    CompetitionId = cs.Competition.CompetitionId,
                    Name = cs.Competition.Name
                },
                Sport = new Models.Sport.SportListingModel()
                {
                    Id = cs.Sport.Id,
                    Name = cs.Sport.Name
                },
                Season = new SeasonListingModel()
                {
                    SeasonId = cs.Season.SeasonId,
                    Year = cs.Season.Year
                },
                JournalistUploadFotoDeadline = cs.JournalistUploadFotoDeadline,
                End = cs.End,
                Id = cs.CompetitionSeasonId,
                JournalistRegistrationEnd = cs.JournalistRegistrationEnd,
                JournalistRegistrationStart = cs.JournalistRegistrationStart,
                Start = cs.Start,
                OfficialSeasonName = cs.OfficialSeasonName
            };
            if (cs.Accreditation != null)
            {
                model.Accreditation = new Models.Accreditation.AccreditationListingModel()
                {
                    AccreditationId = cs.Accreditation.AccreditationId,
                    Close = cs.Accreditation.Close,
                    State = cs.Accreditation.State,
                    files = _googleDriveService.RedactorReport(id, userId).Result
                };
            }
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = Roles.Redactor)]
        public async Task<IActionResult> UploadFile(List<IFormFile> files, int id)
        {
            if (id < 1)
            {
                _logger.LogError($"Competition season was not found. ID: {id}");
                return View("NotFound","Competition season was not found");
            }
            var loggedUser = await _signInManager.UserManager.FindByNameAsync(User.Identity.Name);

            long size = files.Sum(f => f.Length);
            var filePaths = new List<string>();

            foreach (var file in files)
            {
                if (file.Length > 0)
                {
                    var filePath = Path.GetTempFileName();
                    filePaths.Add(filePath);
                    await _googleDriveService.RedactorReportUpload(file, id, loggedUser.Id);
                }
            }
            return RedirectToAction("CompetitionSeasonAccreditation", new { id = id });
        }

        [HttpPost]
        [Authorize(Roles = Roles.Paparazi)]
        public async Task<IActionResult> UploadPaparaziFile(List<IFormFile> files, int id)
        {
            long size = files.Sum(f => f.Length);
            var filePaths = new List<string>();

            foreach (var file in files)
            {
                if (file.Length > 0)
                {
                    var filePath = Path.GetTempFileName();
                    filePaths.Add(filePath);
                    await _googleDriveService.CompetitionSeasonPaparaziUpload(id, file);
                }
            }
            return RedirectToAction("CompetitionSeason", new { id = id });
        }

        [HttpPost]
        [Authorize(Roles = Roles.Redactor)]
        public async Task<IActionResult> CreateRequestRedactorViewPaparaziMedia(int id)
        {
            var loggedUser = await _signInManager.UserManager.FindByIdAsync(User.Identity.Name);
            await _redactorViewPaparaziMediaService.CreateNew(loggedUser.Id, id);
            return RedirectToAction("CompetitionSeason", new { id = id });
        }

        public async Task<IActionResult> PaparaziMedia(int id)
        {
            var loggedUser = await _signInManager.UserManager.FindByIdAsync(User.Identity.Name);
            var hasAcceptedRequest = await _redactorViewPaparaziMediaService.HasAcceptedRequest(id, loggedUser.Id);
            if (!hasAcceptedRequest)
            {
                Response.StatusCode = 403;
                return View("AccessDenied");
            }
            var files = await _googleDriveService.GetCompetitionSeasonMediaList(id);
            var model = files.Select(f => new CompetitionSeasonMediaListingModel()
            {
                IconLink = f.IconLink,
                Id = f.Id,
                Name = f.Name,
                WebViewLink = f.WebViewLink
            });
            return View(model);
        }
    }
}