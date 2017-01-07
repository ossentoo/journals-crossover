using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Mvc;
using AutoMapper;
using Medico.Model;
using Medico.Repository;
using Medico.Repository.Interfaces;
using Medico.Web.Filters;
using Medico.Web.Helpers;

namespace Medico.Web.Controllers
{
    [AuthorizeRedirect(Roles = "Publisher")]
    public class PublisherController : Controller
    {
        private readonly IJournalRepository _journalRepository;
        private readonly IStaticMembershipService _membershipService;

        public PublisherController(IJournalRepository journalRepo, IStaticMembershipService membershipService)
        {
            _journalRepository = journalRepo;
            _membershipService = membershipService;
        }

        public ActionResult Index()
        {
            var userId = (int)_membershipService.GetUser().ProviderUserKey;

            var allJournals = _journalRepository.GetAllJournals(userId);
            var journals = Mapper.Map<IEnumerable<Journal>, IEnumerable<JournalViewModel>>(allJournals);
            return View((List<JournalViewModel>) journals);
        }

        public ActionResult Issues(int id)
        {
            var journalIssues = _journalRepository.GetJournalIssues(id);
            var issueViewModels = Mapper.Map<IEnumerable<Issue>, IEnumerable<JournalIssueViewModel>>(journalIssues);
            return View((List<JournalIssueViewModel>)issueViewModels);
        }

        public ActionResult Create()
        {
            return View();
        }

        public ActionResult GetFile(int id, int issueId)
        {
            var j = _journalRepository.GetJournalById(id);
            if (j == null)
                throw new System.Web.Http.HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound));

            var issue = j.Issues.First(x => x.Id == issueId);
            return File(issue.Content, issue.ContentType);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(JournalViewModel journal)
        {
            if (ModelState.IsValid)
            {
                var newJournal = Mapper.Map<JournalViewModel, Journal>(journal);
                JournalHelper.PopulateFile(journal.File, newJournal, newJournal.Issues.First());

                newJournal.UserId = (int)_membershipService.GetUser().ProviderUserKey;

                var opStatus = _journalRepository.AddJournal(newJournal);
                if (!opStatus.Status)
                    throw new System.Web.Http.HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError));

                return RedirectToAction("Index");
            }
            else
                return View(journal);
        }

        public ActionResult Delete(int Id)
        {
            var selectedJournal = _journalRepository.GetJournalById(Id);
            var journal = Mapper.Map<Journal, JournalViewModel>(selectedJournal);
            return View(journal);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(JournalViewModel journal)
        {
            var selectedJournal = Mapper.Map<JournalViewModel, Journal>(journal);

            var opStatus = _journalRepository.DeleteJournal(selectedJournal);
            if (!opStatus.Status)
                throw new System.Web.Http.HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound));

            return RedirectToAction("Index");
        }

        public ActionResult Edit(int id)
        {
            var journal = _journalRepository.GetJournalById(id);

            var selectedJournal = Mapper.Map<Journal, JournalUpdateViewModel>(journal);

            return View(selectedJournal);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(JournalUpdateViewModel journal)
        {
            if (ModelState.IsValid)
            {
                var selectedJournal = Mapper.Map<JournalUpdateViewModel, Journal>(journal);
                JournalHelper.PopulateFile(journal.File, selectedJournal, selectedJournal.Issues.First());

                var opStatus = _journalRepository.UpdateJournal(selectedJournal);
                if (!opStatus.Status)
                    throw new System.Web.Http.HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound));

                return RedirectToAction("Index");
            }
            else
                return View(journal);
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            base.OnException(filterContext);
        }
    }
}