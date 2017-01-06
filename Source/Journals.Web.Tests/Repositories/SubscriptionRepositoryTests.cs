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
        private const string ThirdJournalTitle = "3rd Journal";
        private const string ThirdJournalDescription = "3rd journal description";

        [TestInitialize]
        public void SubscriptionRepositoryInitialize()
        {
            var modifiedDate = new DateTime(2016, 11, 01);

            #region setupData
            _journals = new List<Journal>  {
                new Journal {
                    Id = 1,
                    Title = "1st Journal",
                    Description = "1st journal description",
                    UserId = 1,
                    Content = new byte[] { 1, 2, 3, 4, 5 },
                    ContentType = ContentType,
                    ModifiedDate = modifiedDate
                },
                new Journal {
                    Id = 2,
                    Title = "2nd Journal",
                    Description = "2nd journal description",
                    UserId = 1,
                    Content = new byte[] { 1, 2, 3, 4, 5 },
                    ContentType = ContentType,
                    ModifiedDate = modifiedDate
                },
                new Journal {
                    Id = 3,
                    Title = ThirdJournalTitle,
                    Description = ThirdJournalDescription,
                    UserId = 1,
                    Content = new byte[] { 1, 2, 3, 4, 5 },
                    ContentType = ContentType,
                    ModifiedDate = modifiedDate
                },
                new Journal {
                    Id = 4,
                    Title = "4th Journal",
                    Description = "4th journal description",
                    UserId = 2,
                    Content = new byte[] { 1, 2, 3, 4, 5 },
                    ContentType = ContentType,
                    ModifiedDate = modifiedDate
                },
                new Journal {
                    Id = 5,
                    Title = "5th Journal",
                    Description = "5th journal description",
                    UserId = 2,
                    Content = new byte[] { 1, 2, 3, 4, 5 },
                    ContentType = ContentType,
                    ModifiedDate = modifiedDate
                }
            }.AsQueryable();

            var user1 = new UserProfile {UserId=1, UserName = "firstUser"};
            _user2 = new UserProfile {UserId=2, UserName = "secondUser" };

            var journal1 = _journals.ElementAt(0);
            var journal2 = _journals.ElementAt(1);
            var journal3 = _journals.ElementAt(2);
            var journal4 = _journals.ElementAt(3);
            var journal5 = _journals.ElementAt(4);

            _subscriptions = new List<Subscription>
            {
                new Subscription
                {
                    Id= 1,
                    Journal = journal1,
                    JournalId = journal1.Id,
                    User=user1,
                    UserId = user1.UserId
                },
                new Subscription
                {
                    Id= 2,
                    Journal = journal2,
                    JournalId = journal2.Id,
                    User=user1,
                    UserId = user1.UserId
                },
                new Subscription
                {
                    Id= 3,
                    Journal = journal3,
                    JournalId = journal3.Id,
                    User=_user2,
                    UserId = _user2.UserId
                },
                new Subscription
                {
                    Id= 4,
                    Journal = journal4,
                    JournalId = journal4.Id,
                    User=_user2,
                    UserId = _user2.UserId
                },
                new Subscription
                {
                    Id= 5,
                    Journal = journal5,
                    JournalId = journal5.Id,
                    User=_user2,
                    UserId = _user2.UserId
                }
            }.AsQueryable();
            #endregion
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
