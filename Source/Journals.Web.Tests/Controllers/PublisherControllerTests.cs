using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Security;
using AutoMapper;
using Journals.Model;
using Medico.Model;
using Medico.Repository;
using Medico.Web.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Medico.Web.Tests.Controllers
{
    [TestClass]
    public class PublisherControllerTests : BaseTests
    {
        private readonly Mock<IStaticMembershipService> _membershipRepository;
        private readonly Mock<MembershipUser> _userMock;
        private readonly Mock<IJournalRepository> _journalRepository;
        private const string ContentType = "application/pdf";

        public PublisherControllerTests()
        {
            _membershipRepository = new Mock<IStaticMembershipService>();
            _userMock = new Mock<MembershipUser>();
            _journalRepository = new Mock<IJournalRepository>();

        }

        [TestInitialize]
        public void TestInitialize()
        {
            _userMock.Setup(x => x.ProviderUserKey).Returns(1);
            _membershipRepository.Setup(x => x.GetUser()).Returns(_userMock.Object);

            _journalRepository.Setup(x => x.GetAllJournals((int)_userMock.Object.ProviderUserKey)).Returns(new List<Journal>(){
                    new Journal{ Id=1, Description="TestDesc", FileName="TestFilename.pdf", Title="Tester", UserId=1, ModifiedDate= DateTime.Now},
                    new Journal{ Id=1, Description="TestDesc2", FileName="TestFilename2.pdf", Title="Tester2", UserId=1, ModifiedDate = DateTime.Now}
            });
        }

        [TestMethod]
        public void Index_Returns_All_Journals()
        {

            //Arrange


            //Act
            var controller = new PublisherController(_journalRepository.Object, _membershipRepository.Object);
            var actionResult = (ViewResult)controller.Index();
            var model = actionResult.Model as IEnumerable<JournalViewModel>;

            //Assert
            Assert.AreEqual(2, model.Count());
            _journalRepository.Verify(x => x.GetAllJournals((int)_userMock.Object.ProviderUserKey), Times.AtLeastOnce);
        }

        [TestMethod]
        public void Create_Returns_View()
        {

            //Act
            var controller = new PublisherController(_journalRepository.Object, _membershipRepository.Object);
            var actionResult = (ViewResult)controller.Create();

            //Assert
            Assert.IsNull(actionResult.View);
        }

        [TestMethod]
        public void GetFile_Returns_A_Journal()
        {
            
            //Arrange

            _userMock.Setup(x => x.ProviderUserKey).Returns(1);
            _membershipRepository.Setup(x => x.GetUser()).Returns(_userMock.Object);

            var contentType = "application/pdf";
            _journalRepository.Setup(x => x.GetJournalById(It.IsInRange(1,10, Range.Inclusive))).Returns(new Journal {
                Content = new byte[] { 1, 2, 3, 4, 5 },
                ContentType = contentType
            });

            //Act
            var controller = new PublisherController(_journalRepository.Object, _membershipRepository.Object);
            var fileContentResult = (FileContentResult)controller.GetFile(1);

            //Assert
            Assert.AreEqual(5, fileContentResult.FileContents.Length);
            Assert.AreEqual(4, fileContentResult.FileContents[3]);
            Assert.AreEqual(contentType, fileContentResult.ContentType);
        }

        [TestMethod]
        [ExpectedException(typeof(HttpResponseException))]
        public void GetFile_Returns_Exception()
        {

            //Arrange
            var journalRepository = new Mock<IJournalRepository>();

            journalRepository.Setup(x => x.GetJournalById(0)).Returns((Journal) null);

            //Act
            var controller = new PublisherController(_journalRepository.Object, _membershipRepository.Object);
            controller.GetFile(0);
        }

        [TestMethod]
        public void Create_Returns_IndexAction()
        {

            //Arrange

            _journalRepository.Setup(x => x.GetJournalById(It.IsInRange(1, 10, Range.Inclusive))).Returns(new Journal
            {
                Content = new byte[] { 1, 2, 3, 4, 5 },
                ContentType = ContentType
            });

            //Act
            var controller = new PublisherController(_journalRepository.Object, _membershipRepository.Object);
            var fileContentResult = (FileContentResult)controller.GetFile(1);

            //Assert
            Assert.AreEqual(5, fileContentResult.FileContents.Length);
            Assert.AreEqual(4, fileContentResult.FileContents[3]);
            Assert.AreEqual(ContentType, fileContentResult.ContentType);
        }

        [TestMethod]
        public void Create_InvalidModelState()
        {
            //Arrange
            var model = new JournalViewModel {Title="This Journal", Description = "This journal description", UserId=-99};

            //Act
            var controller = new PublisherController(_journalRepository.Object, _membershipRepository.Object);
            controller.ModelState.AddModelError("Model error","Incorrectly setup journal model");
            var result = (ViewResult)controller.Create(model);

            //Assert
            Assert.IsTrue(model.Equals(result.Model));
        }

        [TestMethod]
        [ExpectedException(typeof(HttpResponseException))]
        public void Create_Returns_Exception()
        {
            //Arrange
            var model = new JournalViewModel
            {
                Id = 1,
                Title = "This Journal",
                Description = "This journal description",
                UserId = -99,
                Content = new byte[] { 1, 2, 3, 4, 5 },
                ContentType = ContentType
            };


            _membershipRepository.Setup(x => x.GetUser()).Returns(_userMock.Object);
            _journalRepository.Setup(x => x.AddJournal(It.IsAny<Journal>())).Returns(new OperationStatus { Status = false });

            //Act
            var controller = new PublisherController(_journalRepository.Object, _membershipRepository.Object);
            controller.Create(model);

            //Assert
        }

        [TestMethod]
        public void Delete_Returns_Journal()
        {

            //Arrange

            var journal = new Journal
            {
                Content = new byte[] { 1, 2, 3, 4, 5 },
                ContentType = ContentType
            };
            _journalRepository.Setup(x => x.GetJournalById(It.IsAny<int>())).Returns(journal);
            var journalViewModel = Mapper.Map<Journal, JournalViewModel>(journal);

            //Act
            var controller = new PublisherController(_journalRepository.Object, _membershipRepository.Object);
            var model = (JournalViewModel)((ViewResult)controller.Delete(1)).Model;

            //Assert
            Assert.IsTrue(journalViewModel.Content.Equals(model.Content));
            Assert.IsTrue(journalViewModel.ContentType.Equals(model.ContentType));
        }

        [TestMethod]
        [ExpectedException(typeof(HttpResponseException))]
        public void DeletePost_Returns_Exception()
        {

            //Arrange

            var journal = new Journal
            {
                Content = new byte[] { 1, 2, 3, 4, 5 },
                ContentType = ContentType
            };
            _journalRepository.Setup(x => x.DeleteJournal(It.IsAny<Journal>())).Returns(new OperationStatus { Status = false });

            var journalViewModel = Mapper.Map<Journal, JournalViewModel>(journal);

            //Act
            var controller = new PublisherController(_journalRepository.Object, _membershipRepository.Object);
            controller.Delete(journalViewModel);

            //Assert

        }

        [TestMethod]
        public void DeletePost_Returns_Index()
        {

            //Arrange

            var journal = new Journal
            {
                Content = new byte[] { 1, 2, 3, 4, 5 },
                ContentType = ContentType
            };
            _journalRepository.Setup(x => x.DeleteJournal(It.IsAny<Journal>())).Returns(new OperationStatus { Status = true });

            var journalViewModel = Mapper.Map<Journal, JournalViewModel>(journal);

            //Act
            var controller = new PublisherController(_journalRepository.Object, _membershipRepository.Object);
            var result = (RedirectToRouteResult)controller.Delete(journalViewModel);

            //Assert
            Assert.AreEqual("Index", result.RouteValues["action"]);
        }

        [TestMethod]
        public void Edit_Returns_Journal()
        {

            //Arrange

            var journal = new Journal
            {
                Content = new byte[] { 1, 2, 3, 4, 5 },
                ContentType = ContentType
            };
            _journalRepository.Setup(x => x.GetJournalById(It.IsAny<int>())).Returns(journal);
            var journalUpdateViewModel = Mapper.Map<Journal, JournalUpdateViewModel>(journal);

            //Act
            var controller = new PublisherController(_journalRepository.Object, _membershipRepository.Object);
            var model = (JournalUpdateViewModel)((ViewResult)controller.Edit(1)).Model;

            //Assert
            Assert.IsTrue(journalUpdateViewModel.Content.Equals(model.Content));
            Assert.IsTrue(journalUpdateViewModel.ContentType.Equals(model.ContentType));
        }

        [TestMethod]
        [ExpectedException(typeof(HttpResponseException))]
        public void EditPost_Returns_Exception()
        {

            //Arrange

            var journal = new Journal
            {
                Content = new byte[] { 1, 2, 3, 4, 5 },
                ContentType = ContentType
            };
            _journalRepository.Setup(x => x.UpdateJournal(It.IsAny<Journal>())).Returns(new OperationStatus { Status = false });

            var journalViewModel = Mapper.Map<Journal, JournalUpdateViewModel>(journal);

            //Act
            var controller = new PublisherController(_journalRepository.Object, _membershipRepository.Object);
            controller.Edit(journalViewModel);

            //Assert

        }

        [TestMethod]
        public void EditPost_Returns_Index()
        {

            //Arrange

            var journal = new Journal
            {
                Content = new byte[] { 1, 2, 3, 4, 5 },
                ContentType = ContentType
            };
            _journalRepository.Setup(x => x.UpdateJournal(It.IsAny<Journal>())).Returns(new OperationStatus { Status = true });

            var journalViewModel = Mapper.Map<Journal, JournalUpdateViewModel>(journal);

            //Act
            var controller = new PublisherController(_journalRepository.Object, _membershipRepository.Object);
            var result = (RedirectToRouteResult)controller.Edit(journalViewModel);

            //Assert
            Assert.AreEqual("Index", result.RouteValues["action"]);
        }
    }
}