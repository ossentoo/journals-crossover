using System.Web.Mvc;
using Medico.Web.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Medico.Web.Tests.Controllers
{
    [TestClass]
    public class ErrorControllerTests
    {
        [TestMethod]
        public void RequestLengthExceeded_Returns_ViewBagMessage()
        {
            //Arrange
            var controller = new ErrorController();

            //Act
            var actionResult = (ViewResult)controller.RequestLengthExceeded();
            var model = (HandleErrorInfo)actionResult.Model;

            //Assert
            Assert.AreEqual("Uploading file this large is not supported. Please try again.", model.Exception.Message);
            Assert.AreEqual("Error", model.ControllerName);
            Assert.AreEqual("RequestLengthExceeded", model.ActionName);
        }
    }
}
