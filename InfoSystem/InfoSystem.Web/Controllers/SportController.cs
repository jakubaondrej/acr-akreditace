using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InfoSystem.Core.DataAbstraction;
using InfoSystem.Core.Seasons;
using InfoSystem.Core.Sports;
using InfoSystem.Core.Users;
using InfoSystem.Web.Models.championship;
using InfoSystem.Web.Models.Sport;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InfoSystem.Web.Controllers
{
    [Authorize]
    public class SportController : Controller
    {
        private SportService _sportService;
        private SeasonService _seasonService;

        public SportController(SportService sportService, SeasonService seasonService)
        {
            _sportService = sportService;
            _seasonService = seasonService;
        }
        [Authorize(Roles =Roles.Admin)]
        public async Task<IActionResult> Index()
        {
            var s = await _sportService.GetAllSports();
            var model = s.Select(s => new SportListingModel()
            {
                Id = s.Id,
                Name = s.Name
            }).ToList();
            return View(model);
        }
        [Authorize(Roles =Roles.Admin)]
        public IActionResult Create()
        {
            var model = new SportCreateModel();
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles =Roles.Admin)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SportCreateModel model)
        {
            if (ModelState.IsValid)
            {
                var sport = new SportCreateData()
                {
                    Name = model.Name
                };
                await _sportService.Create(sport);
                return RedirectToAction("Index");
            }
            return View(model);
        }

        [Authorize(Roles =Roles.Admin)]
        public async Task<IActionResult> Details(int id)
        {
            var sport = await _sportService.GetSportById(id);
            var model = new SportDetailModel()
            {
                Name = sport.Name,
                ImageUri = sport.ImageUri,
                SportId = sport.Id,
                championships = sport.Championships.Select(c => new ChampionshipListingModel()
                {
                    Id = c.Id,
                    Name = c.Name
                }).ToList()
            };
            return View(model);
        }

        [Authorize(Roles =Roles.Admin)]
        public async Task<IActionResult> Edit(int id)
        {
            var sport = await _sportService.GetSportById(id);
            var model = new SportCreateModel()
            {
                Name = sport.Name,
                ImageUri = sport.ImageUri
            };
            return View(model);
        }
        [HttpPost, ActionName("Edit")]
        [Authorize(Roles =Roles.Admin)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, SportCreateModel model)
        {
            var data = new SportCreateData()
            {
                Name = model.Name,
                ImageUri = model.ImageUri
            };
            await _sportService.Edit(id, data);
            return RedirectToAction("Index");
        }

        [Authorize(Roles =Roles.Admin)]
        public async Task<IActionResult> Delete(int id)
        {
            var sport = await _sportService.GetSportById(id);
            var model = new SportDetailModel()
            {
                Name = sport.Name,
                SportId = sport.Id,
                championships = sport.Championships.Select(c => new ChampionshipListingModel()
                {
                    Id = c.Id,
                    Name = c.Name
                }).ToList()
            };
            return View(model);
        }
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles =Roles.Admin)]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _sportService.Delete(id);
            return RedirectToAction("Index");
        }


        public async Task<IActionResult> SelectSport()
        {
            var list = await _sportService.GetAllSports();
            var model = list.Select(l => new SportSelectionListModel()
            {
                Id = l.Id,
                Name = l.Name,
                ImageUrl= l.ImageUri
            }).ToList();
            
            return View(model);
        }
    }
}