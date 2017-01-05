using System;
using System.Collections.Generic;
using System.Data.Entity;
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
            throw new NotImplementedException();
        }

        public OperationStatus AddSubscription(int journalId, int userId)
        {
            throw new NotImplementedException();
        }

        public List<Subscription> GetJournalsForSubscriber(int userId)
        {
            throw new NotImplementedException();
        }

        public OperationStatus UnSubscribe(int journalId, int userId)
        {
            throw new NotImplementedException();
        }

        public List<Subscription> GetJournalsForSubscriber(string userName)
        {
            throw new NotImplementedException();
        }
    }
}