using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using Autofac.Extras.Moq;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Moq;
using NUnit.Framework;
using ProjectsBaseShared.Models;
using ProjectsBaseShared.Security;
using ProjectsBaseWebApplication.Controllers;
using ProjectsBaseWebApplication.ViewModels;
using Assert = NUnit.Framework.Assert;

namespace ProjectsBaseWebApplicationTests.Controllers
{
    [TestFixture]
    public class AccountControllerTests
    {
        private AccountController _accountController;

        [Test]
        public void RegisterHttpGetTest()
        {
            using (var mock = AutoMock.GetLoose())
            {
                _accountController = mock.Create<AccountController>();

                var result = _accountController.Register();

                Assert.IsInstanceOf<ViewResult>(result);
            }
        }

        [Test]
        public async Task RegisterHttpPostValidViewModelTest()
        {
            var mockIUserStore = new Mock<IUserStore<User>>();
            var mockIAuthenticationManager = new Mock<IAuthenticationManager>();

            var mockApplicationUserManager = new Mock<ApplicationUserManager>(mockIUserStore.Object);
            mockApplicationUserManager.Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(default(User));
            mockApplicationUserManager.Setup(x => x.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            var mockApplicationSignInManager = new Mock<ApplicationSignInManager>(mockApplicationUserManager.Object, mockIAuthenticationManager.Object);
            mockApplicationSignInManager.Setup(x => x.PasswordSignInAsync(It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<bool>(),
                    It.IsAny<bool>()))
                .ReturnsAsync(SignInStatus.Success);

            _accountController = new AccountController(mockApplicationUserManager.Object, mockApplicationSignInManager.Object, mockIAuthenticationManager.Object);

            var viewModel = new AccountRegisterViewModel()
            {
                Email = "email",
                Password = "Password",
                ConfirmPassword = "Password"
            };

            var result = await _accountController.Register(viewModel) as RedirectToRouteResult;

            Assert.AreEqual("Index", (string)result?.RouteValues["action"]);
        }

        [Test]
        public async Task RegisterHttpPostEmailIsInUseTest()
        {
            var mockIUserStore = new Mock<IUserStore<User>>();
            var mockIAuthenticationManager = new Mock<IAuthenticationManager>();

            var mockApplicationUserManager = new Mock<ApplicationUserManager>(mockIUserStore.Object);
            mockApplicationUserManager.Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(new User());

            var mockApplicationSignInManager = new Mock<ApplicationSignInManager>(mockApplicationUserManager.Object, mockIAuthenticationManager.Object);

            _accountController = new AccountController(mockApplicationUserManager.Object, mockApplicationSignInManager.Object, mockIAuthenticationManager.Object);

            var viewModel = new AccountRegisterViewModel()
            {
                Email = "email",
                Password = "Password",
                ConfirmPassword = "Password"
            };

            var result = await _accountController.Register(viewModel);

            Assert.IsInstanceOf<ViewResult>(result);
        }

        [Test]
        public async Task RegisterHttpPostEmailIsNullTest()
        {
            var mockIUserStore = new Mock<IUserStore<User>>();
            var mockIAuthenticationManager = new Mock<IAuthenticationManager>();

            var mockApplicationUserManager = new Mock<ApplicationUserManager>(mockIUserStore.Object);

            var mockApplicationSignInManager = new Mock<ApplicationSignInManager>(mockApplicationUserManager.Object, mockIAuthenticationManager.Object);

            _accountController = new AccountController(mockApplicationUserManager.Object, mockApplicationSignInManager.Object, mockIAuthenticationManager.Object);
            _accountController.ModelState.AddModelError("key", "error message");

            var viewModel = new AccountRegisterViewModel()
            {
                Email = null,
                Password = "Password",
                ConfirmPassword = "Password"
            };

            var result = await _accountController.Register(viewModel);

            Assert.IsInstanceOf<ViewResult>(result);
        }

        [Test]
        public async Task RegisterHttpPostInvalidModelStateTest()
        {
            var mockIUserStore = new Mock<IUserStore<User>>();
            var mockIAuthenticationManager = new Mock<IAuthenticationManager>();

            var mockApplicationUserManager = new Mock<ApplicationUserManager>(mockIUserStore.Object);
            mockApplicationUserManager.Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(default(User));

            var mockApplicationSignInManager = new Mock<ApplicationSignInManager>(mockApplicationUserManager.Object, mockIAuthenticationManager.Object);

            _accountController = new AccountController(mockApplicationUserManager.Object, mockApplicationSignInManager.Object, mockIAuthenticationManager.Object);
            _accountController.ModelState.AddModelError("key", "error message");

            var viewModel = new AccountRegisterViewModel()
            {
                Email = "email",
                Password = "Password",
                ConfirmPassword = "Password"
            };

            var result = await _accountController.Register(viewModel);

            Assert.IsInstanceOf<ViewResult>(result);
        }

        [Test]
        public async Task RegisterHttpPostInvalidCreateAsyncTest()
        {
            var mockIUserStore = new Mock<IUserStore<User>>();
            var mockIAuthenticationManager = new Mock<IAuthenticationManager>();

            var mockApplicationUserManager = new Mock<ApplicationUserManager>(mockIUserStore.Object);
            mockApplicationUserManager.Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(default(User));
            mockApplicationUserManager.Setup(x => x.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Failed("error"));

            var mockApplicationSignInManager = new Mock<ApplicationSignInManager>(mockApplicationUserManager.Object, mockIAuthenticationManager.Object);

            _accountController = new AccountController(mockApplicationUserManager.Object, mockApplicationSignInManager.Object, mockIAuthenticationManager.Object);

            var viewModel = new AccountRegisterViewModel()
            {
                Email = "email",
                Password = "Password",
                ConfirmPassword = "Password"
            };

            var result = await _accountController.Register(viewModel);

            Assert.IsInstanceOf<ViewResult>(result);
        }

        [Test]
        public async Task RegisterHttpPostSignInUserReturnsFailureTest()
        {
            var mockIUserStore = new Mock<IUserStore<User>>();
            var mockIAuthenticationManager = new Mock<IAuthenticationManager>();

            var mockApplicationUserManager = new Mock<ApplicationUserManager>(mockIUserStore.Object);
            mockApplicationUserManager.Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(default(User));
            mockApplicationUserManager.Setup(x => x.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            var mockApplicationSignInManager = new Mock<ApplicationSignInManager>(mockApplicationUserManager.Object, mockIAuthenticationManager.Object);
            mockApplicationSignInManager.Setup(x => x.PasswordSignInAsync(It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<bool>(),
                    It.IsAny<bool>()))
                .ReturnsAsync(SignInStatus.Failure);

            _accountController = new AccountController(mockApplicationUserManager.Object, mockApplicationSignInManager.Object, mockIAuthenticationManager.Object);

            var viewModel = new AccountRegisterViewModel()
            {
                Email = "email",
                Password = "Password",
                ConfirmPassword = "Password"
            };

            var result = await _accountController.Register(viewModel);

            Assert.IsInstanceOf<ViewResult>(result);
        }

        [Test]
        public void RegisterHttpPostSignInUserReturnsLockedOutTest()
        {
            var mockIUserStore = new Mock<IUserStore<User>>();
            var mockIAuthenticationManager = new Mock<IAuthenticationManager>();

            var mockApplicationUserManager = new Mock<ApplicationUserManager>(mockIUserStore.Object);
            mockApplicationUserManager.Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(default(User));
            mockApplicationUserManager.Setup(x => x.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            var mockApplicationSignInManager = new Mock<ApplicationSignInManager>(mockApplicationUserManager.Object, mockIAuthenticationManager.Object);
            mockApplicationSignInManager.Setup(x => x.PasswordSignInAsync(It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<bool>(),
                    It.IsAny<bool>()))
                .ReturnsAsync(SignInStatus.LockedOut);

            _accountController = new AccountController(mockApplicationUserManager.Object, mockApplicationSignInManager.Object, mockIAuthenticationManager.Object);

            var viewModel = new AccountRegisterViewModel()
            {
                Email = "email",
                Password = "Password",
                ConfirmPassword = "Password"
            };

            Assert.ThrowsAsync<NotImplementedException>(async () => await _accountController.Register(viewModel));
        }

        [Test]
        public void RegisterHttpPostSignInUserReturnsRequiresVerificationTest()
        {
            var mockIUserStore = new Mock<IUserStore<User>>();
            var mockIAuthenticationManager = new Mock<IAuthenticationManager>();

            var mockApplicationUserManager = new Mock<ApplicationUserManager>(mockIUserStore.Object);
            mockApplicationUserManager.Setup(x => x.FindByEmailAsync(It.IsAny<string>()))
                .ReturnsAsync(default(User));
            mockApplicationUserManager.Setup(x => x.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            var mockApplicationSignInManager = new Mock<ApplicationSignInManager>(mockApplicationUserManager.Object, mockIAuthenticationManager.Object);
            mockApplicationSignInManager.Setup(x => x.PasswordSignInAsync(It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<bool>(),
                    It.IsAny<bool>()))
                .ReturnsAsync(SignInStatus.RequiresVerification);

            _accountController = new AccountController(mockApplicationUserManager.Object, mockApplicationSignInManager.Object, mockIAuthenticationManager.Object);

            var viewModel = new AccountRegisterViewModel()
            {
                Email = "email",
                Password = "Password",
                ConfirmPassword = "Password"
            };

            Assert.ThrowsAsync<NotImplementedException>(async () => await _accountController.Register(viewModel));
        }

        [Test]
        public void SignInHttpGetTest()
        {
            using (var mock = AutoMock.GetLoose())
            {
                _accountController = mock.Create<AccountController>();

                var result = _accountController.SignIn();

                Assert.IsInstanceOf<ViewResult>(result);
            }
        }

        [Test]
        public async Task SignInHttpPostInvalidViewModelTest()
        {
            using (var mock = AutoMock.GetLoose())
            {
                var viewModel = new AccountSignInViewModel()
                {
                    Email = "",
                    Password = "",
                    RememberMe = false
                };

                _accountController = mock.Create<AccountController>();
                _accountController.ModelState.AddModelError("key", "error message");

                var result = await _accountController.SignIn(viewModel);

                Assert.IsInstanceOf<ViewResult>(result);
            }
        }

        [Test]
        public async Task SignInHttpPostValidViewModelTest()
        {
            var mockIUserStore = new Mock<IUserStore<User>>();
            var mockIAuthenticationManager = new Mock<IAuthenticationManager>();
            var mockApplicationUserManager = new Mock<ApplicationUserManager>(mockIUserStore.Object);

            var mockApplicationSignInManager = new Mock<ApplicationSignInManager>(mockApplicationUserManager.Object, mockIAuthenticationManager.Object);
            mockApplicationSignInManager.Setup(x => x.PasswordSignInAsync(It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<bool>(),
                    It.IsAny<bool>()))
                .ReturnsAsync(SignInStatus.Success);

            _accountController = new AccountController(mockApplicationUserManager.Object, mockApplicationSignInManager.Object, mockIAuthenticationManager.Object);

            var viewModel = new AccountSignInViewModel()
            {
                Email = "",
                Password = "",
                RememberMe = false
            };

            var result = await _accountController.SignIn(viewModel) as RedirectToRouteResult;

            Assert.AreEqual("Index", (string)result?.RouteValues["action"]);
        }

        [Test]
        public void SignOutTest()
        {
            using (var mock = AutoMock.GetLoose())
            {
                _accountController = mock.Create<AccountController>();

                var result = _accountController.SignOut() as RedirectToRouteResult;

                Assert.AreEqual("Index", (string)result?.RouteValues["action"]);
            }
        }
    }
}
