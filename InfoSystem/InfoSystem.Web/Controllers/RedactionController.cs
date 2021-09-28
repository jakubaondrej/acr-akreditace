using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InfoSystem.Core.DataAbstraction;
using InfoSystem.Core.Redactions;
using InfoSystem.Core.Users;
using InfoSystem.Web.Models.Redaction;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InfoSystem.Web.Controllers
{
    [Authorize]
    public class RedactionController : Controller
    {
        private RedactionService _redactionService;

        public RedactionController(RedactionService redactionService)
        {
            _redactionService = redactionService;
        }
        [Authorize(Roles =Roles.Admin)]
        public async Task<IActionResult> Index()
        {
            var list = await _redactionService.GetAll();
            var model = list.Select(m=>new RedactionListingModel()
            {
                Id = m.Id,
                Name = m.Name
            });
            return View(model);
        }

        [Authorize(Roles =Roles.Admin)]
        public IActionResult Create()
        {
            var model = new CreateRedactionModel();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles =Roles.Admin)]
        public async Task<IActionResult> CreateAsync(CreateRedactionModel model)
        {
            if (ModelState.IsValid)
            {
                var r = new RedactionCreateData()
                {
                    GeneralEditor = model.GeneralEditor,
                    GeneralEditorCallNumber = model.GeneralEditorCallNumber,
                    GeneralEditorEmail = model.GeneralEditorEmail,
                    Link = model.Link,
                    Name = model.Name,
                    Note = model.Note
                };
                await _redactionService.CreateRedaction(r); //todo: ošetřit přidání s názvem pro existující záznam
            }
            return View(model);
        }

        [Authorize(Roles =Roles.Admin)]
        public async Task<IActionResult> Edit(int id)
        {
            var r = await _redactionService.GetRedactionById(id);
            if (r == null)
            {
                Response.StatusCode = 404;
                return View("NotFound", id);
            }
            var model = new RedactionEditModel() 
            {
                GeneralEditor = r.GeneralEditor,
                GeneralEditorCallNumber = r.GeneralEditorCallNumber,
                GeneralEditorEmail = r.GeneralEditorEmail,
                Link = r.Link,
                Name = r.Name,
                Note = r.Note,
                Id = r.Id
            };
            return View(model);
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles =Roles.Admin)]
        public async Task<ActionResult> Edit(int id,RedactionEditModel model)
        {
            if (ModelState.IsValid)
            {
                var data = new RedactionDetail()
                {
                    GeneralEditor = model.GeneralEditor,
                    GeneralEditorCallNumber = model.GeneralEditorCallNumber,
                    GeneralEditorEmail = model.GeneralEditorEmail,
                    Link = model.Link,
                    Name = model.Name,
                    Note = model.Note
                };
                await _redactionService.EditRedaction(id, data);
                return RedirectToAction("Index");
            }
            return View(model);
        }

        public async Task<IActionResult> Details(int id)
        {
            var r = await _redactionService.GetRedactionById(id);
            if (r == null)
            {
                //TODO
            }
            var model = new RedactionDetailModel()
            {
                GeneralEditor = r.GeneralEditor,
                GeneralEditorCallNumber = r.GeneralEditorCallNumber,
                GeneralEditorEmail = r.GeneralEditorEmail,
                Link = r.Link,
                Name = r.Name,
                Note = r.Note,
                Id = r.Id
            };
            return View(model);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles =Roles.Admin)]
        public async Task<ActionResult> Delete(int id)
        {
            await _redactionService.DeleteRedaction(id);
            return RedirectToAction("Index");
        }
    }
}