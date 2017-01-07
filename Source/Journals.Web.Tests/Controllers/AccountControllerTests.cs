using System.Linq;
using System.Web.Mvc;
using Journals.Model;
using Medico.Repository.Interfaces;
using Medico.Web.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Medico.Web.Tests.Controllers
{
    [TestClass]
    public class AccountControllerTests
    {
        private readonly Mock<IWebSecurity> _webSecurity;

        public AccountControllerTests()
        {
            _webSecurity = new Mock<IWebSecurity>();
        }
        [TestMethod]
        public void UnauthorizedLogin_Returns_ErrorMessageInModelState()
        {
            //Arrange
            var returnUrl = string.Empty;
            var error = "Unauthorized";

            //Act
            var controller = new AccountController(_webSecurity.Object);
            var actionResult = (ViewResult)controller.Login(returnUrl, error);
            var data = actionResult.ViewData;

            //Assert
            var modelState = data.ModelState.Values.First();
            Assert.IsTrue(modelState.Errors.Count==1);
            Assert.AreEqual("You are not authorized to access this section", modelState.Errors.First().ErrorMessage);
        }

        [TestMethod]
        public void InvalidModelState_Returns_ErrorMessageInModelState()
        {
            //Arrange
            var error = "Unauthorized";
            var model = new LoginModel
            {
                Password = string.Empty,
                UserName = string.Empty
            };

            //Act
            var controller = new AccountController(_webSecurity.Object);
            controller.ModelState.AddModelError("Login", "Username is mandatory.");
            var actionResult = (ViewResult)controller.Login(model, error);
            var data = actionResult.ViewData;

            //Assert
            var modelState = data.ModelState.Values.First();
            Assert.IsTrue(modelState.Errors.Count == 1);
            Assert.AreEqual("Username is mandatory.", modelState.Errors.ElementAt(0).ErrorMessage);
        }

        [TestMethod]
        public void Logout_Returns_RedirectAction()
        {
            //Arrange
            //Act
            var controller = new AccountController(_webSecurity.Object);
            var actionResult = (RedirectToRouteResult)controller.LogOff();

            //Assert
            Assert.AreEqual("Home", actionResult.RouteValues["controller"]);
            Assert.AreEqual("Index", actionResult.RouteValues["action"]);
        }
    }
}