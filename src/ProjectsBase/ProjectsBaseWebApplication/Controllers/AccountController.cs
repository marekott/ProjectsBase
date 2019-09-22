using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using ProjectsBaseShared.Models;
using ProjectsBaseShared.Security;
using ProjectsBaseWebApplication.ViewModels;

namespace ProjectsBaseWebApplication.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationUserManager _userManager;
        private readonly ApplicationSignInManager _signInManager;
        private readonly IAuthenticationManager _authenticationManager;

        public AccountController(
            ApplicationUserManager userManager,
            ApplicationSignInManager signInManager,
            IAuthenticationManager authenticationManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _authenticationManager = authenticationManager;
        }

        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken, AllowAnonymous]
        public async Task<ActionResult> Register(AccountRegisterViewModel viewModel)
        {
            await CheckIfEmailIsInUse(viewModel.Email);

            if (ModelState.IsValid)
            {
                var user = CreateUser(viewModel);

                var createResult = await _userManager.CreateAsync(user, viewModel.Password);

                if (createResult.Succeeded)
                {
                    var signInResult = await SignInUser(viewModel.Email, viewModel.Password);
                    return CheckSignInResult(viewModel, signInResult);
                }

                foreach (var error in createResult.Errors)
                {
                    ModelState.AddModelError("", error);
                }
            }

            return View();
        }

        private async Task CheckIfEmailIsInUse(string email)
        {
            if (email == null)
            {
                return;
            }

            var existingUser = await _userManager.FindByEmailAsync(email);
            if (existingUser != null)
            {
                ModelState.AddModelError("Email",
                    $"The provided email address '{email}' has already been used to register an account. Please sign-in using your existing account.");
            }
        }

        private static User CreateUser(AccountRegisterViewModel viewModel)
        {
            return new User
            {
                UserName = viewModel.Email,
                Email = viewModel.Email
            };
        }

        private async Task<SignInStatus> SignInUser(string login, string password, bool rememberMe = false)
        {
            return await _signInManager.PasswordSignInAsync(
                login, password, rememberMe, false); //TODO do sprawdzenia ostatni parametr
        }

        [AllowAnonymous]
        public ActionResult SignIn()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken, AllowAnonymous]
        public async Task<ActionResult> SignIn(AccountSignInViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var result = await SignInUser(viewModel.Email, viewModel.Password, viewModel.RememberMe);

            return CheckSignInResult(viewModel, result);
        }

        private ActionResult CheckSignInResult(IAccountViewModel viewModel, SignInStatus result)
        {
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToAction("Index", "Home");
                case SignInStatus.Failure:
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View(viewModel);
                case SignInStatus.LockedOut:
                    throw new NotImplementedException("Identity feature not implemented.");
                case SignInStatus.RequiresVerification:
                    throw new NotImplementedException("Identity feature not implemented.");
                default:
                    throw new Exception("Unexpected Microsoft.AspNet.Identity.Owin.SignInStatus enum value: " + result);
            }
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult SignOut()
        {
            _authenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);

            return RedirectToAction("Index", "Home");
        }
    }
}