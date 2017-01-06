using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Journals.Model;
using Medico.Model;
using Medico.Repository;
using Medico.Repository.DataContext;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Medico.Web.Tests.Repositories
{
    [TestClass]
    public class SubscriptionRepositoryTests
    {
        private IQueryable<Subscription> _subscriptions;
        private Mock<JournalsContext> _context;
        private UserProfile _user2;
        private IQueryable<Journal> _journals;
        private const string ContentType = "application/pdf";

        [TestInitialize]
        public void SubscriptionRepositoryInitialize()
        {

            _subscriptions = MockData.Subscriptions.AsQueryable();
            _journals = MockData.Journals.AsQueryable();
            _user2 = MockData.Users.Last();
            _context = SetupSubscriptionSet();
        }


        [TestMethod]
        public void GetJournals_WithData_ReturnsListWithData()
        {
            var repository = new SubscriptionRepository(_context.Object);
            var journals = repository.GetAllJournals();

            Assert.AreEqual(5, journals.Count);
        }

        [TestMethod]
        public void GetJournals_WithEmptyList_ReturnsEmptyList()
        {
            var repository = new SubscriptionRepository(_context.Object);
            _subscriptions = new List<Subscription>().AsQueryable();
            var journals = repository.GetAllJournals();

            Assert.AreEqual(0, journals.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException), "Object reference not set to an instance of an object.")]
        public void GetJournals_WithNullSubscriptions_ReturnsNullReferenceException()
        {
            var repository = new SubscriptionRepository(_context.Object);
            _subscriptions = null;
            repository.GetAllJournals();
        }

        [TestMethod]
        public void AddSubscription_ReturnsStatus_True()
        {
            const string title = "6th Journal";
            const string description = "6th journal description";

            var userProfile = new UserProfile {UserId = 3, UserName = "3rd user"};

            var journal = new Journal
            {
                Id = 6,
                Title = title,
                Description = description,
                UserId = 6,
                User = userProfile,
                Content = new byte[] { 1, 2, 3, 4, 5 },
                ContentType = ContentType,
                ModifiedDate = new DateTime(2017, 01, 01)
            };
            var journals = new List<Journal>(_journals) { journal };
            _journals = journals.AsQueryable();

            var repository = new SubscriptionRepository(_context.Object);
            var result = repository.AddSubscription(6, 3);
            var lastItem = _subscriptions.Last();

            Assert.IsTrue(result.Status);
            Assert.AreEqual(6, _subscriptions.Count());
            Assert.AreEqual(6, lastItem.JournalId);
            Assert.AreEqual(3, lastItem.UserId);
        }

        [TestMethod]
        public void GetJournalsForSubscriber_UserId_ReturnsList()
        {
            var repository = new SubscriptionRepository(_context.Object);
            var userJournals = repository.GetJournalsForSubscriber(2);

            Assert.AreEqual(3, userJournals.Count);
            Assert.AreEqual(_user2.UserName, userJournals.ElementAt(0).User.UserName);
            Assert.AreEqual(_user2.UserName, userJournals.ElementAt(1).User.UserName);
            Assert.AreEqual(_user2.UserName, userJournals.ElementAt(2).User.UserName);
        }

        [TestMethod]
        public void GetJournalsForSubscriber_UserName_ReturnsList()
        {
            var repository = new SubscriptionRepository(_context.Object);
            var userJournals = repository.GetJournalsForSubscriber("firstUser");

            Assert.AreEqual(2, userJournals.Count);
            Assert.AreEqual(1, userJournals.ElementAt(0).User.UserId);
            Assert.AreEqual(1, userJournals.ElementAt(1).User.UserId);
        }

        [TestMethod]
        public void UnSubscribe_Returns_Success()
        {
            var repository = new SubscriptionRepository(_context.Object);

            var result = repository.UnSubscribe(4, 2);

            Assert.IsTrue(result.Status);
            Assert.AreEqual(4, _subscriptions.Count());
            Assert.IsNull(_subscriptions.FirstOrDefault(x => x.Id == 4));
        }


        private Mock<JournalsContext> SetupSubscriptionSet()
        {
            var context = new Mock<JournalsContext>();

            var subscriptions = new Mock<DbSet<Subscription>>();
            subscriptions.As<IQueryable<Subscription>>().Setup(m => m.Provider).Returns(_subscriptions.Provider);
            subscriptions.As<IQueryable<Subscription>>().Setup(m => m.Expression).Returns(_subscriptions.Expression);
            subscriptions.As<IQueryable<Subscription>>().Setup(m => m.ElementType).Returns(_subscriptions.ElementType);
            subscriptions.As<IQueryable<Subscription>>().Setup(m => m.GetEnumerator()).Returns(() => _subscriptions.GetEnumerator());
            subscriptions.Setup(m => m.Add(It.IsAny<Subscription>())).Callback<Subscription>(subscription =>
            {
                var list = new List<Subscription>(_subscriptions) { subscription };
                _subscriptions = list.AsQueryable();
            });

            subscriptions.Setup(m => m.Remove(It.IsAny<Subscription>())).Callback<Subscription>(subscription =>
            {
                var list = new List<Subscription>(_subscriptions);
                list.RemoveAll(x => x.Id == subscription.Id);

                _subscriptions = list.AsQueryable();
            });

            context.Setup(m => m.Set<Subscription>()).Returns(subscriptions.Object);

            context.Object.Subscriptions = subscriptions.Object;

            return context;
        }
    }
}
