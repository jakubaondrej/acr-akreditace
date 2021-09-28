using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using HtmlAgilityPack;
using InfoSystem.Core.DataAbstraction;
using InfoSystem.Core.Emails;
using InfoSystem.Core.Redactions;
using InfoSystem.Core.Users;
using InfoSystem.Data.Entities;
using InfoSystem.Web.Models;
using InfoSystem.Web.Models.Redaction;
using InfoSystem.Web.Models.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace InfoSystem.Web.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly ILogger<UserController> _logger;
        private UserService _userService;
        private RedactionService _redactionService;
        private UserManager<User> _userManager;
        private SignInManager<User> _signInManager;
        private readonly EmailService _emailService;
        private readonly IHostingEnvironment _hostingEnvironment;

        public UserController(
            ILogger<UserController> logger,
            UserService userService,
            RedactionService redactionService,
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            EmailService emailService,
            IHostingEnvironment hostingEnvironment)
        {
            _logger = logger;
            _userService = userService;
            _redactionService = redactionService;
            _signInManager = signInManager;
            _emailService = emailService;
            _hostingEnvironment = hostingEnvironment;
            _userManager = userManager;
        }
        [AllowAnonymous]
        public IActionResult Login()
        {
            _logger.LogInformation("Login");
            if (User.Identity.IsAuthenticated)
            {
                _logger.LogInformation("User is already authenticated.");
                return RedirectToAction("Index", "Home");
            }
            var model = new LoginModel();
            return View(model);
        }
        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                _logger.LogInformation("Login - Model is valid");
                var result = await _signInManager.PasswordSignInAsync(model.Username, model.Password, false, false);

                if (result.Succeeded)
                {
                    if (!String.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    {
                        _logger.LogInformation($"Redirect - {returnUrl}");
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        _logger.LogInformation($"Redirect - Home");
                        return RedirectToAction("Index", "Home");
                    }
                }
                _logger.LogInformation("Login - Invalid login attempt.");

                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            }
            return View(model);
        }
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> CreateAsync()
        {
            var model = new CreateUserModel();
            model.Redactions = await GetRedactionListingModels();
            return View(model);
        }
        [Authorize(Roles = Roles.Admin)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAsync(CreateUserModel model)
        {
            if (ModelState.IsValid)
            {
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
                    var resultRole = await _userManager.AddToRoleAsync(user, model.SelectedUserRole);
                    if (resultRole.Succeeded)
                    {
                        await _emailService.SendUserCreatedEmail(model.Username, model.Password, model.Email);
                        return RedirectToAction("Index", "Home");
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

        private async Task<IEnumerable<RedactionListingModel>> GetRedactionListingModels()
        {
            var list = await _redactionService.GetAll();
            return list.Select(r => new RedactionListingModel()
            {
                Id = r.Id,
                Name = r.Name
            });
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [AcceptVerbs("Get", "Post")]
        [AllowAnonymous]
        public async Task<IActionResult> IsEmailInUse(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return Json(true);
            }
            else
            {
                return Json($"Email {email} is already in use");
            }
        }

        [AcceptVerbs("Get", "Post")]
        [AllowAnonymous]
        public async Task<IActionResult> IsUserNameInUse(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user == null)
            {
                return Json(true);
            }
            else
            {
                return Json($"User name {username} is already in use");
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }

        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> Index()
        {
            var list = _userManager.Users;
            var model = list.Select(u => new UserListingModel()
            {
                Id = u.Id,
                Name = u.UserName
            });
            return View(model);
        }

        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> Details(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                Response.StatusCode = 404;
                return View("UserNotFound", id);
            }
            var roles = await _userManager.GetRolesAsync(user);
            var model = new UserDetailsModel()
            {
                Id = user.Id,
                Username = user.UserName,
                Email = user.Email,
                Note = user.Note,
                PhoneNumber = user.PhoneNumber,
                RedactionId = user.RedactionId,
                UserRoles = roles.ToList()

            };
            if (user.RedactionId.HasValue)
            {
                var r = await _redactionService.GetRedactionById(user.RedactionId ?? 0);
                model.Redaction = new RedactionListingModel()
                {
                    Id = r.Id,
                    Name = r.Name
                };
            }
            return View(model);
        }

        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> Edit(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                Response.StatusCode = 404;
                return View("UserNotFound", id);
            }
            var roles = await _userManager.GetRolesAsync(user);
            var model = new UserEditModel()
            {
                Username = user.UserName,
                Email = user.Email,
                Note = user.Note,
                PhoneNumber = user.PhoneNumber,
                SelectedRedactionId = user.RedactionId,
                SelectedUserRole = roles[0],
            };
            model.Redactions = await GetRedactionListingModels();
            return View(model);
        }
        [Authorize(Roles = Roles.Admin)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UserEditModel model, string id)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(id);
                if (user == null)
                {
                    Response.StatusCode = 404;
                    return View("UserNotFound", id);
                }
                user.Email = model.Email;
                user.PhoneNumber = model.PhoneNumber;
                user.RedactionId = model.SelectedRedactionId;
                user.Note = model.Note;
                user.UserName = model.Username;
                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction("Details", new { id = id });
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(String.Empty, error.Description);
                }
            }
            model.Redactions = await GetRedactionListingModels();
            return View(model);
        }

        [Authorize]
        public IActionResult ChangePassword()
        {
            var model = new UserChangePasswordModel();
            return View(model);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(UserChangePasswordModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                var result = await _userManager.ChangePasswordAsync(user, model.OldPassword,model.NewPassword);
                if (result.Succeeded)
                {
                    _logger.LogInformation($"Password for '{user.UserName}' was changed.");
                    TempData["Success"] = "Password was changed";
                    return View();
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(String.Empty, error.Description);
                }
            }
            return View(new UserChangePasswordModel());
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                Response.StatusCode = 404;
                return View("UserNotFound", id);
            }
            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                return RedirectToAction("Index");
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
            return View("Index");
        }
    }
}