using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using Medico.Model;
using Medico.Repository.DataContext;
using Medico.Repository.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Medico.Web.Tests.Repositories
{
    [TestClass]
    public class IssueRepositoryTests
    {
        private IQueryable<Issue> _issues;
        private Mock<JournalsContext> _context;
        private const string ContentType = "application/pdf";
        private const string ThirdJournalTitle = "3rd Journal";
        private const string ThirdJournalDescription = "3rd journal description";
        private const string Title = "6th Journal";
        private const string Description = "6th journal description";
        private readonly Journal _journalItem;
        private readonly Issue _issue2;
        private const string Filename2Txt = "filename2.txt";

        public IssueRepositoryTests()
        {
            var content = new byte[] { 1, 2, 3, 4, 5 };

            var modifiedDate = new DateTime(2016,01,01);
            var issue = new Issue { Id = 1, JournalId = 1, ModifiedDate = modifiedDate,
                Content = content, ContentType = ContentType, FileName = "filename.txt" };

            _journalItem = new Journal
            {
                Title = Title,
                Description = Description,
                Issues = new Collection<Issue>{ issue }
            };

            issue.Journal = _journalItem;

            _journalItem.Issues = new Collection<Issue>();

            _issue2 = new Issue
            {
                Id = 1,
                JournalId = 1,
                ModifiedDate = modifiedDate.AddDays(1),
                Content = content,
                ContentType = ContentType,
                FileName = Filename2Txt,
                Journal = _journalItem
            };
        }

        [TestInitialize]
        public void IssueRepositoryInitialize()
        {
            _issues = MockData.Issues.AsQueryable();

            _context = SetupJournalsSet();
        }

        [TestMethod]
        public void AddJournal_ReturnsStatus_True()
        {
            var repository = new IssueRepository(_context.Object);
            var result = repository.AddIssue(_issue2);
            var lastItem = _issues.Last();

            Assert.IsTrue(result.Status);
            Assert.AreEqual(8, _issues.Count());
            Assert.AreEqual(ContentType, lastItem.ContentType);
            Assert.AreEqual(Filename2Txt, lastItem.FileName);
        }

        [TestMethod]
        public void GetJournalById_Returns_SingleJournal()
        {
            var repository = new IssueRepository(_context.Object);
            var issue = repository.GetIssueById(1);

            Assert.AreEqual(1, issue.Id);
            Assert.AreEqual(ContentType, issue.ContentType);
            Assert.AreEqual("file1.txt", issue.FileName);
        }

        [TestMethod]
        public void DeleteJournalById_Returns_Success()
        {
            var repository = new IssueRepository(_context.Object);
            var issue = _issues.ElementAt(4);

            var result = repository.DeleteIssue(issue);

            Assert.IsTrue(result.Status);
            Assert.AreEqual(5, _issues.Count());
            Assert.IsNull(_issues.FirstOrDefault(x=>x.Id== issue.Id));
        }

        private Mock<JournalsContext> SetupJournalsSet()
        {
            var context = new Mock<JournalsContext>();

            var issues = new Mock<DbSet<Issue>>();
            issues.As<IQueryable<Issue>>().Setup(m => m.Provider).Returns(_issues.Provider);
            issues.As<IQueryable<Issue>>().Setup(m => m.Expression).Returns(_issues.Expression);
            issues.As<IQueryable<Issue>>().Setup(m => m.ElementType).Returns(_issues.ElementType);
            issues.As<IQueryable<Issue>>().Setup(m => m.GetEnumerator()).Returns(() => _issues.GetEnumerator());
            issues.Setup(m => m.Add(It.IsAny<Issue>())).Callback<Issue>((newJournal) =>
            {
                var list = new List<Issue>(_issues) { newJournal };
                _issues = list.AsQueryable();
            });

            issues.Setup(m => m.Remove(It.IsAny<Issue>())).Callback<Issue>(journal =>
            {
                var list = new List<Issue>(_issues);
                list.RemoveAll(x => x.Id == journal.Id);

                _issues = list.AsQueryable();
            });

            context.Setup(m => m.Set<Issue>()).Returns(issues.Object);

            context.Object.Issues = issues.Object;

            return context;
        }
    }
}
