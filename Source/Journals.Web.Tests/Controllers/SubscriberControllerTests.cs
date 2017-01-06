using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Security;
using Journals.Model;
using Medico.Model;
using Medico.Repository;
using Medico.Web.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Medico.Web.Tests.Controllers
{
    [TestClass]
    public class SubscriberControllerTests : BaseTests
    {
        private readonly Mock<IStaticMembershipService> _membershipService;
        private readonly Mock<MembershipUser> _userMock;
        private readonly Mock<IJournalRepository> _journalRepository;
        private readonly Mock<ISubscriptionRepository> _subscriptionRepository;

        public SubscriberControllerTests()
        {
            _membershipService = new Mock<IStaticMembershipService>();
            _userMock = new Mock<MembershipUser>();
            _journalRepository = new Mock<IJournalRepository>();
            _subscriptionRepository = new Mock<ISubscriptionRepository>();
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _userMock.Setup(x => x.ProviderUserKey).Returns(1);
            _membershipService.Setup(x => x.GetUser()).Returns(_userMock.Object);

            _journalRepository.Setup(x => x.GetAllJournals((int)_userMock.Object.ProviderUserKey)).Returns(()=>MockData.Journals);
            _subscriptionRepository.Setup(x => x.GetAllJournals()).Returns(() => MockData.Journals);

            _subscriptionRepository.Setup(x => x.GetJournalsForSubscriber(1)).Returns(MockData.Subscriptions.Where(u=>u.UserId==1).ToList());

            _subscriptionRepository.Setup(x => x.AddSubscription(It.Is<int>(y=>y==4), It.Is<int>(y => y == 1))).Returns(new OperationStatus {Status = true});
            _subscriptionRepository.Setup(x => x.AddSubscription(It.Is<int>(y=>y==4), It.Is<int>(y => y == 2))).Returns(new OperationStatus {Status = false});

            _subscriptionRepository.Setup(x => x.UnSubscribe(It.Is<int>(y => y == 1), It.Is<int>(y => y == 1))).Returns(new OperationStatus { Status = true });
            _subscriptionRepository.Setup(x => x.UnSubscribe(It.Is<int>(y => y == 4), It.Is<int>(y => y == 1))).Returns(new OperationStatus { Status = false });

            _subscriptionRepository.Setup(x => x.GetJournalsForSubscriber(1)).Returns(MockData.Subscriptions.Where(u => u.UserId == 1).ToList());
        }

        [TestMethod]
        public void Index_EmptyJournals_Returns_EmptyView()
        {
            _subscriptionRepository.Setup(x => x.GetAllJournals()).Returns((List<Journal>) null);

            //Act
            var controller = new SubscriberController(_journalRepository.Object, _subscriptionRepository.Object, _membershipService.Object);
            var actionResult = (ViewResult)controller.Index();

            //Assert
            Assert.IsNull(actionResult.View);
        }

        [TestMethod]
        public void Index_Returns_All_Journals()
        {
            //Act
            var controller = new SubscriberController(_journalRepository.Object, _subscriptionRepository.Object, _membershipService.Object);
            var actionResult = (ViewResult)controller.Index();
            var model = actionResult.Model as List<SubscriptionViewModel>;

            //Assert
            Assert.AreEqual(5, model.Count);
            Assert.AreEqual(2, model.Count(x=>x.IsSubscribed));
            _subscriptionRepository.Verify(x => x.GetJournalsForSubscriber((int)_userMock.Object.ProviderUserKey), Times.AtLeastOnce);
        }

        [TestMethod]
        public void Subscribe_Returns_RedirectToIndex()
        {
            //Act
            var controller = new SubscriberController(_journalRepository.Object, _subscriptionRepository.Object, _membershipService.Object);
            var result = (RedirectToRouteResult)controller.Subscribe(4);

            //Assert
            Assert.AreEqual("Index", result.RouteValues["action"]);
        }

        [TestMethod]
        [ExpectedException(typeof(HttpResponseException))]
        public void SubscribeWithExistingSubscription_Returns_Exception()
        {
            //Arrange
            _userMock.Setup(x => x.ProviderUserKey).Returns(2);

            //Act
            var controller = new SubscriberController(_journalRepository.Object, _subscriptionRepository.Object, _membershipService.Object);
            controller.Subscribe(4);
        }


        [TestMethod]
        public void Unsubscribe_Returns_RedirectToIndex()
        {
            //Act
            var controller = new SubscriberController(_journalRepository.Object, _subscriptionRepository.Object, _membershipService.Object);
            var result = (RedirectToRouteResult)controller.UnSubscribe(1);

            //Assert
            Assert.AreEqual("Index", result.RouteValues["action"]);
        }

        [TestMethod]
        [ExpectedException(typeof(HttpResponseException))]
        public void UnsubscribeWithoutSubscription__Returns_Exception()
        {
            //Act
            var controller = new SubscriberController(_journalRepository.Object, _subscriptionRepository.Object, _membershipService.Object);
            controller.UnSubscribe(4);
        }

    }
}
