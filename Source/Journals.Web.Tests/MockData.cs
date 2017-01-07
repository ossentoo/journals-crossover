using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Journals.Model;
using Medico.Model;

namespace Medico.Web.Tests
{
    public static class MockData
    {
        public static string ContentType => "application/pdf";
        private const string ThirdJournalTitle = "3rd Journal";
        private const string ThirdJournalDescription = "3rd journal description";

        public static List<Journal> Journals { get; private set; }
        public static List<Subscription> Subscriptions { get; private set; }
        public static List<UserProfile> Users { get; private set; }

        

        static MockData()
        {
            var modifiedDate = new DateTime(2016, 11, 01);

            var data = new byte[] {1, 2, 3, 4, 5};
            var fileName = "file1.txt";
            Journals = new List<Journal>
            {
                new Journal
                {
                    Id = 1,
                    Title = "1st Journal",
                    Description = "1st journal description",
                    UserId = 1,
                    ModifiedDate = modifiedDate,
                    Issues = new Collection<JournalIssue> {new JournalIssue {Id=1, JournalId = 1, ModifiedDate = modifiedDate, Content = data, ContentType = ContentType, FileName = fileName } }
                },
                new Journal
                {
                    Id = 2,
                    Title = "2nd Journal",
                    Description = "2nd journal description",
                    UserId = 1,
                    ModifiedDate = modifiedDate,
                    Issues = new Collection<JournalIssue> {new JournalIssue {Id=2, JournalId = 2, ModifiedDate = modifiedDate, Content = data, ContentType = ContentType, FileName = fileName } }
                },
                new Journal
                {
                    Id = 3,
                    Title = ThirdJournalTitle,
                    Description = ThirdJournalDescription,
                    UserId = 1,
                    ModifiedDate = modifiedDate,
                    Issues = new Collection<JournalIssue> {new JournalIssue {Id=3, JournalId = 3, ModifiedDate = modifiedDate, Content = data, ContentType = ContentType, FileName = fileName } }
                },
                new Journal
                {
                    Id = 4,
                    Title = "4th Journal",
                    Description = "4th journal description",
                    UserId = 2,
                    ModifiedDate = modifiedDate,
                    Issues = new Collection<JournalIssue> {new JournalIssue {Id=4, JournalId = 4, ModifiedDate = modifiedDate, Content = data, ContentType = ContentType, FileName = fileName } }
                },
                new Journal
                {
                    Id = 5,
                    Title = "5th Journal",
                    Description = "5th journal description",
                    UserId = 2,
                    ModifiedDate = modifiedDate,
                    Issues = new Collection<JournalIssue> {new JournalIssue {Id=5, JournalId = 5, ModifiedDate = modifiedDate, Content = data, ContentType = ContentType, FileName = fileName } }
                }
            };

            var user1 = new UserProfile { UserId = 1, UserName = "firstUser" };
            var user2 = new UserProfile { UserId = 2, UserName = "secondUser" };

            var journal1 = Journals[0];
            var journal2 = Journals[1];
            var journal3 = Journals[2];
            var journal4 = Journals[3];
            var journal5 = Journals[4];

            Subscriptions = new List<Subscription>
            {
                new Subscription
                {
                    Id = 1,
                    Journal = journal1,
                    JournalId = journal1.Id,
                    User = user1,
                    UserId = user1.UserId
                },
                new Subscription
                {
                    Id = 2,
                    Journal = journal2,
                    JournalId = journal2.Id,
                    User = user1,
                    UserId = user1.UserId
                },
                new Subscription
                {
                    Id = 3,
                    Journal = journal3,
                    JournalId = journal3.Id,
                    User = user2,
                    UserId = user2.UserId
                },
                new Subscription
                {
                    Id = 4,
                    Journal = journal4,
                    JournalId = journal4.Id,
                    User = user2,
                    UserId = user2.UserId
                },
                new Subscription
                {
                    Id = 5,
                    Journal = journal5,
                    JournalId = journal5.Id,
                    User = user2,
                    UserId = user2.UserId
                }
            };

            Users = new List<UserProfile> { user1 , user2};

        }

    }
}
