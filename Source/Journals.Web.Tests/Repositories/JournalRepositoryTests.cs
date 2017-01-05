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
        private const string ContentType = "application/pdf";

        [TestInitialize]
        public void JournalRepositoryInitialize()
        {
            _journals = new List<Journal>  {
                new Journal { Title = "1st Journal",
                    Description = "1st journal description",
                    UserId = 1,
                    Content = new byte[] { 1, 2, 3, 4, 5 },
                    ContentType = ContentType,
                    ModifiedDate = DateTime.UtcNow
                },
                new Journal { Title = "2nd Journal",
                    Description = "2nd journal description",
                    UserId = 1,
                    Content = new byte[] { 1, 2, 3, 4, 5 },
                    ContentType = ContentType,
                    ModifiedDate = DateTime.UtcNow
                },
                new Journal { Title = "3rd Journal",
                    Description = "3rd journal description",
                    UserId = 1,
                    Content = new byte[] { 1, 2, 3, 4, 5 },
                    ContentType = ContentType,
                    ModifiedDate = DateTime.UtcNow
                },
                new Journal { Title = "4th Journal",
                    Description = "4th journal description",
                    UserId = 1,
                    Content = new byte[] { 1, 2, 3, 4, 5 },
                    ContentType = ContentType,
                    ModifiedDate = DateTime.UtcNow
                },
                new Journal { Title = "5th Journal",
                    Description = "5th journal description",
                    UserId = 1,
                    Content = new byte[] { 1, 2, 3, 4, 5 },
                    ContentType = ContentType,
                    ModifiedDate = DateTime.UtcNow
                }
            }.AsQueryable();

        }

        [TestMethod]
        public void AddJournal_ReturnsStatus_True()
        {
            var context = new Mock<JournalsContext>();

            var journals = new Mock<DbSet<Journal>>();
            journals.As<IQueryable<Journal>>().Setup(m => m.GetEnumerator()).Returns(_journals.GetEnumerator());
            journals.As<IQueryable<Journal>>().Setup(m => m.Expression).Returns(_journals.Expression);
            journals.As<IQueryable<Journal>>().Setup(m => m.ElementType).Returns(_journals.ElementType);
            journals.As<IQueryable<Journal>>().Setup(m => m.GetEnumerator()).Returns(_journals.GetEnumerator());
            journals.Setup(m => m.Add(It.IsAny<Journal>())).Callback<Journal>((newJournal) =>
            {
                var list = new List<Journal>(_journals) {newJournal};
                _journals = list.AsQueryable();
            });
            context.Setup(m => m.Set<Journal>()).Returns(journals.Object);

            context.Object.Journals = journals.Object;

            var repository = new JournalRepository(context.Object);

            const string title = "6th Journal";
            const string description = "6th journal description";
            var journal = new Journal
            {
                Title = title,
                Description = description,
                UserId = 1,
                Content = new byte[] {1, 2, 3, 4, 5},
                ContentType = ContentType
            };

            var result = repository.AddJournal(journal);
            Assert.IsTrue(result.Status);
            Assert.AreEqual(6, _journals.Count());
            var lastItem = _journals.Last();
            Assert.AreEqual(title, lastItem.Title);
            Assert.AreEqual(description, lastItem.Description);
        }
    }
}
