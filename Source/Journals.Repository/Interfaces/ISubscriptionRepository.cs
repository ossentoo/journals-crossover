using System.Collections.Generic;
using Journals.Model;
using Medico.Model;

namespace Medico.Repository.Interfaces
{
    public interface ISubscriptionRepository
    {
        List<Journal> GetAllJournals();

        OperationStatus AddSubscription(int journalId, int userId);

        List<Subscription> GetJournalsForSubscriber(int userId);

        OperationStatus UnSubscribe(int journalId, int userId);

        List<Subscription> GetJournalsForSubscriber(string userName);
    }
}