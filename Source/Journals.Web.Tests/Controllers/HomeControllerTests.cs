using System.Web.Mvc;
using Medico.Web.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Medico.Web.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTests
    {
        [TestMethod]
        public void Index_Returns_ViewBagMessage()
        {
            //Act
            var controller = new HomeController();
            var actionResult = (ViewResult)controller.Index();
            var data = actionResult.ViewBag;

            //Assert
            Assert.AreEqual("Open Journal Publishers", data.Message);
        }

        [TestMethod]
        public void About_Returns_ViewBagMessage()
        {
            //Act
            var controller = new HomeController();
            var actionResult = (ViewResult)controller.About();
            var data = actionResult.ViewBag;

            //Assert
            Assert.AreEqual("Your app description page.", data.Message);
        }

        [TestMethod]
        public void Contact_Returns_ViewBagMessage()
        {
            //Act
            var controller = new HomeController();
            var actionResult = (ViewResult)controller.Contact();
            var data = actionResult.ViewBag;

            //Assert
            Assert.AreEqual("Your contact page.", data.Message);
        }

    }
}
