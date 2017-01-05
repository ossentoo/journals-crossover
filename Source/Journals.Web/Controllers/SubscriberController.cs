using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Mvc;
using System.Web.Security;
using AutoMapper;
using Journals.Model;
using Medico.Model;
using Medico.Repository;

namespace Medico.Web.Controllers
{
    [Authorize]
    public class SubscriberController : Controller
    {
        private IJournalRepository _journalRepository;
        private readonly ISubscriptionRepository _subscriptionRepository;

        public SubscriberController(IJournalRepository journalRepo, ISubscriptionRepository subscriptionRepo)
        {
            _journalRepository = journalRepo;
            _subscriptionRepository = subscriptionRepo;
        }

        public ActionResult Index()
        {
            var journals = _subscriptionRepository.GetAllJournals();

            if (journals == null)
                return View();

            var userId = (int)Membership.GetUser().ProviderUserKey;
            var subscriptions = _subscriptionRepository.GetJournalsForSubscriber(userId);

            var subscriberModel = Mapper.Map<List<Journal>, List<SubscriptionViewModel>>(journals);
            foreach (var journal in subscriberModel)
            {
                if (subscriptions.Any(k => k.JournalId == journal.Id))
                    journal.IsSubscribed = true;
            }

            return View(subscriberModel);
        }

        public ActionResult Subscribe(int Id)
        {
            var opStatus = _subscriptionRepository.AddSubscription(Id, (int)Membership.GetUser().ProviderUserKey);
            if (!opStatus.Status)
                throw new System.Web.Http.HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound));

            return RedirectToAction("Index");
        }

        public ActionResult UnSubscribe(int Id)
        {
            var opStatus = _subscriptionRepository.UnSubscribe(Id, (int)Membership.GetUser().ProviderUserKey);
            if (!opStatus.Status)
                throw new System.Web.Http.HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound));

            return RedirectToAction("Index");
        }
    }
}