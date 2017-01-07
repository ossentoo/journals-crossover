using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        private const string Title = "6th Journal";
        private const string Description = "6th journal description";
        private readonly Journal _journalItem;

        public JournalRepositoryTests()
        {
            _journalItem = new Journal
            {
                Title = Title,
                Description = Description,
                Issues = new Collection<JournalIssue> { new JournalIssue { Id = 1, JournalId = 1, ModifiedDate = new DateTime(2016,01,01),
                    Content = new byte[] { 1, 2, 3, 4, 5 }, ContentType = ContentType, FileName = "filename.txt" } },

            };
        }

        [TestInitialize]
        public void JournalRepositoryInitialize()
        {
            _journals = MockData.Journals.AsQueryable();

            _context = SetupJournalsSet();
        }

        [TestMethod]
        public void AddJournal_ReturnsStatus_True()
        {


            var repository = new JournalRepository(_context.Object);
            var result = repository.AddJournal(_journalItem);
            Assert.IsTrue(result.Status);
            Assert.AreEqual(6, _journals.Count());
            var lastItem = _journals.Last();
            Assert.AreEqual(Title, lastItem.Title);
            Assert.AreEqual(Description, lastItem.Description);
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
