using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using AutoMapper;
using Journals.Model;
using Journals.Repository;
using Medico.Web.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Telerik.JustMock;

namespace Medico.Web.Tests.Controllers
{
    [TestClass]
    public class AccountControllerTests
    {
        [TestMethod]
        public void UnauthorizedLogin_Returns_ErrorMessageInModelState()
        {
            //Arrange
            var returnUrl = string.Empty;
            var error = "Unauthorized";

            //Act
            var controller = new AccountController();
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
            var controller = new AccountController();
            controller.ModelState.AddModelError("Login", "Username is mandatory.");
            var actionResult = (ViewResult)controller.Login(model, error);
            var data = actionResult.ViewData;

            //Assert
            var modelState = data.ModelState.Values.First();
            Assert.IsTrue(modelState.Errors.Count == 1);
            Assert.AreEqual("Username is mandatory.", modelState.Errors.ElementAt(0).ErrorMessage);
        }
    }
}