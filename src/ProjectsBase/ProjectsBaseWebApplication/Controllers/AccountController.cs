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

        public AccountController(ApplicationUserManager userManager,
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
                    return await SignInUser(viewModel);
                }

                AddErrorsToModelState(createResult);
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

        private async Task<ActionResult> SignInUser(IAccountViewModel viewModel, bool rememberMe = false)
        {
            var result = await _signInManager.PasswordSignInAsync(
                viewModel.Email, viewModel.Password, rememberMe, false);

            return CheckSignInResult(null, result);
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

        private void AddErrorsToModelState(IdentityResult createResult)
        {
            foreach (var error in createResult.Errors)
            {
                ModelState.AddModelError("", error);
            }
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

            return await SignInUser(viewModel, viewModel.RememberMe);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult SignOut()
        {
            _authenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);

            return RedirectToAction("Index", "Home");
        }
    }
}