using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Medico.Model;
using Medico.Repository;
using Medico.Repository.DataContext;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Medico.Web.Tests.Repositories
{
    [TestClass]
    public class JournalRepositoryTests
    {
        private IQueryable<Journal> _journals;
        private Mock<JournalsContext> _context;
        private const string ContentType = "application/pdf";
        private const string ThirdJournalTitle = "3rd Journal";
        private const string ThirdJournalDescription = "3rd journal description";

        [TestInitialize]
        public void JournalRepositoryInitialize()
        {
            _journals = new List<Journal>  {
                new Journal {
                    Id = 1,
                    Title = "1st Journal",
                    Description = "1st journal description",
                    UserId = 1,
                    Content = new byte[] { 1, 2, 3, 4, 5 },
                    ContentType = ContentType,
                    ModifiedDate = DateTime.UtcNow
                },
                new Journal {
                    Id = 2,
                    Title = "2nd Journal",
                    Description = "2nd journal description",
                    UserId = 1,
                    Content = new byte[] { 1, 2, 3, 4, 5 },
                    ContentType = ContentType,
                    ModifiedDate = DateTime.UtcNow
                },
                new Journal {
                    Id = 3,
                    Title = ThirdJournalTitle,
                    Description = ThirdJournalDescription,
                    UserId = 1,
                    Content = new byte[] { 1, 2, 3, 4, 5 },
                    ContentType = ContentType,
                    ModifiedDate = DateTime.UtcNow
                },
                new Journal {
                    Id = 4,
                    Title = "4th Journal",
                    Description = "4th journal description",
                    UserId = 2,
                    Content = new byte[] { 1, 2, 3, 4, 5 },
                    ContentType = ContentType,
                    ModifiedDate = DateTime.UtcNow
                },
                new Journal {
                    Id = 5,
                    Title = "5th Journal",
                    Description = "5th journal description",
                    UserId = 2,
                    Content = new byte[] { 1, 2, 3, 4, 5 },
                    ContentType = ContentType,
                    ModifiedDate = DateTime.UtcNow
                }
            }.AsQueryable();

            _context = SetupJournalsSet();
        }

        [TestMethod]
        public void AddJournal_ReturnsStatus_True()
        {
            const string title = "6th Journal";
            const string description = "6th journal description";
            var journal = new Journal
            {
                Id=6,
                Title = title,
                Description = description,
                UserId = 6,
                Content = new byte[] { 1, 2, 3, 4, 5 },
                ContentType = ContentType
            };

            var repository = new JournalRepository(_context.Object);
            var result = repository.AddJournal(journal);
            Assert.IsTrue(result.Status);
            Assert.AreEqual(6, _journals.Count());
            var lastItem = _journals.Last();
            Assert.AreEqual(title, lastItem.Title);
            Assert.AreEqual(description, lastItem.Description);
        }

        [TestMethod]
        public void GetJournalsForUser_Returns_List()
        {
            var repository = new JournalRepository(_context.Object);
            var userJournals = repository.GetAllJournals(2);

            Assert.AreEqual(2, userJournals.Count());
            Assert.AreEqual("4th Journal", userJournals.ElementAt(0).Title);
            Assert.AreEqual("5th Journal", userJournals.ElementAt(1).Title);
        }

        [TestMethod]
        public void GetJournalById_Returns_SingleJournal()
        {

            var repository = new JournalRepository(_context.Object);
            var journal = repository.GetJournalById(3);

            Assert.AreEqual(3, journal.Id);
            Assert.AreEqual(ThirdJournalTitle, journal.Title);
            Assert.AreEqual(ThirdJournalDescription, journal.Description);
        }

        [TestMethod]
        public void DeleteJournalById_Returns_Success()
        {
            var repository = new JournalRepository(_context.Object);
            var journal = _journals.ElementAt(4);

            var result = repository.DeleteJournal(journal);

            Assert.IsTrue(result.Status);
            Assert.AreEqual(4, _journals.Count());
            Assert.IsNull(_journals.FirstOrDefault(x=>x.Id== journal.Id));
        }

        private Mock<JournalsContext> SetupJournalsSet()
        {
            var context = new Mock<JournalsContext>();

            var journals = new Mock<DbSet<Journal>>();
            journals.As<IQueryable<Journal>>().Setup(m => m.Provider).Returns(_journals.Provider);
            journals.As<IQueryable<Journal>>().Setup(m => m.Expression).Returns(_journals.Expression);
            journals.As<IQueryable<Journal>>().Setup(m => m.ElementType).Returns(_journals.ElementType);
            journals.As<IQueryable<Journal>>().Setup(m => m.GetEnumerator()).Returns(() => _journals.GetEnumerator());
            journals.Setup(m => m.Add(It.IsAny<Journal>())).Callback<Journal>((newJournal) =>
            {
                var list = new List<Journal>(_journals) { newJournal };
                _journals = list.AsQueryable();
            });

            journals.Setup(m => m.Remove(It.IsAny<Journal>())).Callback<Journal>(journal =>
            {
                var list = new List<Journal>(_journals);
                list.RemoveAll(x => x.Id == journal.Id);

                _journals = list.AsQueryable();
            });

            context.Setup(m => m.Set<Journal>()).Returns(journals.Object);

            context.Object.Journals = journals.Object;

            return context;
        }
    }
}
