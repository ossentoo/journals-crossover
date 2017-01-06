using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Journals.Model;
using Medico.Model;

namespace Medico.Repository
{
    public class SubscriptionRepository : Repository<Subscription>, ISubscriptionRepository
    {
        public SubscriptionRepository(DbContext context) : base(context)
        {
        }

        public List<Journal> GetAllJournals()
        {
            try
            {
                var subscriptions = Get(s => !string.IsNullOrEmpty(s.Journal.Title));
                var result = from a in subscriptions
                             .Select(j=>j.Journal)
                                select new
                                {
                                    a.Id,
                                    a.Title,
                                    a.Description,
                                    a.User,
                                    a.UserId,
                                    a.ModifiedDate,
                                    a.FileName
                                };

                if (result == null || !result.Any())
                    return new List<Journal>();

                var list = result.AsEnumerable()
                                    .Select(f => new Journal
                                    {
                                        Id = f.Id,
                                        Title = f.Title,
                                        Description = f.Description,
                                        UserId = f.UserId,
                                        User = f.User,
                                        ModifiedDate = f.ModifiedDate,
                                        FileName = f.FileName
                                    }).ToList();

                return list;
            }
            catch (Exception e)
            {
                OperationStatus.CreateFromException("Error fetching subscriptions: ", e); ;
                throw e;
            }            
        }

        public OperationStatus AddSubscription(int journalId, int userId)
        {
            var opStatus = new OperationStatus { Status = true };
            try
            {
                var s = new Subscription
                {
                    JournalId = journalId,
                    UserId = userId
                };

                Add(s);
            }
            catch (Exception e)
            {
                opStatus = OperationStatus.CreateFromException("Error adding subscription: ", e);
            }

            return opStatus;
        }

        public List<Subscription> GetJournalsForSubscriber(int userId)
        {
            try
            {
                var subscriptions = Get(u => u.UserId == userId);

                if (subscriptions != null)
                    return subscriptions.ToList();
            }
            catch (Exception e)
            {
                OperationStatus.CreateFromException("Error fetching subscriptions: ", e); ;
            }

            return new List<Subscription>();
        }

        public List<Subscription> GetJournalsForSubscriber(string userName)
        {
            try
            {
                var subscriptions = Get(u => u.User.UserName == userName)
                                        .Include(j => j.Journal);

                if (subscriptions != null)
                    return subscriptions.ToList();
            }
            catch (Exception e)
            {
                OperationStatus.CreateFromException("Error fetching subscriptions: ", e); ;
            }

            return new List<Subscription>();
        }

        public OperationStatus UnSubscribe(int journalId, int userId)
        {
            var opStatus = new OperationStatus { Status = true };

            try
            {
                var subscriptions = Get(u => u.JournalId == journalId && u.UserId == userId);
                Delete(subscriptions);                
            }
            catch (Exception e)
            {
                opStatus = OperationStatus.CreateFromException("Error deleting subscription: ", e);
            }

            return opStatus;
        }
    }
}