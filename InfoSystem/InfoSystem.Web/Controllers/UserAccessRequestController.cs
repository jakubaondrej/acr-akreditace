using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InfoSystem.Core.DataAbstraction;
using InfoSystem.Core.Emails;
using InfoSystem.Core.Redactions;
using InfoSystem.Core.UserAccessRequests;
using InfoSystem.Core.Users;
using InfoSystem.Data.Entities;
using InfoSystem.Web.Models.Redaction;
using InfoSystem.Web.Models.UserAccessRequest;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace InfoSystem.Web.Controllers
{
    [Authorize]
    public class UserAccessRequestController : Controller
    {
        private UserAccessRequestService _uarService;
        private readonly ILogger<UserAccessRequestController> _logger;
        private UserService _userService;
        private RedactionService _redactionService;
        private UserManager<User> _userManager;
        private readonly EmailService _emailService;

        public UserAccessRequestController(UserAccessRequestService uarService,
                                           ILogger<UserAccessRequestController> logger,
                                           UserService userService,
                                           RedactionService redactionService,
                                           UserManager<User> userManager,
                                           EmailService emailService)
        {
            _uarService = uarService;
            _logger = logger;
            _logger.LogInformation("UserAccessRequestController logger initial");
            _userService = userService;
            _redactionService = redactionService;
            _userManager = userManager;
            _emailService = emailService;
        }
        [AllowAnonymous]
        public IActionResult Create()
        {
            var model = new UserAccessRequestCreateModel();
            return View(model);
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAsync(UserAccessRequestCreateModel model)
        {
            if (ModelState.IsValid)
            {
                var data = new UserAccessRequestCreateData()
                {
                    CallNumber = model.CallNumber,
                    Email = model.Email,
                    Note = model.Note,
                    Redaction = model.Redaction,
                    Username = model.Username
                };
                await _uarService.Create(data);
                return RedirectToAction("AccessRequestCreated");
            }
            return View(model);
        }

        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> Index()
        {
            var list = await _uarService.GetAllUserAccessRequests();
            var model = list.Select(u => new UserAccessRequestListingModel()
            {
                Email = u.Email,
                Id = u.Id,
                Username = u.Username
            }).ToList();
            return View(model);
        }

        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> Details(int id)
        {
            //if (id == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            var u = await _uarService.GetUserAccessRequestById(id);
            //if (u == null)
            //{
            //    return HttpNotFound();
            //}
            var model = new UserAccessRequestDetailModel()
            {
                Email = u.Email,
                Id = u.Id,
                Username = u.Username,
                CallNumber = u.CallNumber,
                Note = u.Note,
                Redaction = u.Redaction
            };
            return View(model);
        }

        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> Delete(int id)
        {
            var u = await _uarService.GetUserAccessRequestById(id);
            var model = new UserAccessRequestDetailModel()
            {
                Email = u.Email,
                Id = u.Id,
                Username = u.Username,
                CallNumber = u.CallNumber,
                Note = u.Note,
                Redaction = u.Redaction
            };
            return View(model);
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            await _uarService.RemoveUserAccessRequestsById(id);
            return RedirectToAction("Index");
        }
        private async Task<IEnumerable<RedactionListingModel>> GetRedactionListingModels()
        {
            var list = await _redactionService.GetAll();
            return list.Select(r => new RedactionListingModel()
            {
                Id = r.Id,
                Name = r.Name
            });
        }

        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> AcceptUserAccessRequest(int id)
        {
            var u = await _uarService.GetUserAccessRequestById(id);
            var model = new AcceptUserAccessRequestModel()
            {
                Email = u.Email,
                Username = u.Username,
                PhoneNumber = u.CallNumber,
                Note = u.Note,
                RedactionRequired = u.Redaction,
            };
            model.Redactions = await GetRedactionListingModels();

            return View(model);
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpPost, ActionName("AcceptUserAccessRequest")]
        public async Task<IActionResult> AcceptUserAccessRequest(AcceptUserAccessRequestModel model, int id)
        {
            if (ModelState.IsValid)
            {
                if (model.CreateAsNewRedaction)
                {
                    var redaction = new RedactionCreateData()
                    {
                        Name = model.RedactionRequired,
                        Note = "This redaction was created automaticaly"
                    };
                    var redactionId = await _redactionService.CreateRedaction(redaction);
                    model.SelectedRedactionId = redactionId;
                }
                var user = new User
                {
                    UserName = model.Username,
                    Email = model.Email,
                    Note = model.Note,
                    PhoneNumber = model.PhoneNumber,
                    RedactionId = model.SelectedRedactionId,
                    RedactorType = model.RedactorType
                };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    _logger.LogInformation($"User '{user.UserName}' created.");
                    var resultRole = await _userManager.AddToRoleAsync(user, model.SelectedUserRole);
                    if (resultRole.Succeeded)
                    {
                        _logger.LogInformation($"User '{user.UserName}' added to {model.SelectedUserRole} role.");

                        await _uarService.RemoveUserAccessRequestsById(id);
                        _logger.LogInformation($"User request for '{user.UserName}' removed.");
                        try
                        {
                            await _emailService.SendUserCreatedEmail(user.UserName, model.Password, user.Email);
                            _logger.LogInformation("AcceptUserAccessRequest - Email sent.");
                        }
                        catch (Exception e)
                        {
                            _logger.LogError(e, $"AcceptUserAccessRequest - Email was not send.");
                        }
                        return RedirectToAction("Index");
                    }
                    foreach (var error in resultRole.Errors)
                    {
                        ModelState.AddModelError(String.Empty, error.Description);
                    }
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(String.Empty, error.Description);
                }

            }
            model.Redactions = await GetRedactionListingModels();
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult AccessRequestCreated()
        {
            return View();
        }
    }
}