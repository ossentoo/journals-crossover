using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Mvc;
using AutoMapper;
using Medico.Model;
using Medico.Repository.Interfaces;
using Medico.Web.Filters;
using Medico.Web.Helpers;

namespace Medico.Web.Controllers
{
    [AuthorizeRedirect(Roles = "Publisher")]
    public class PublisherController : Controller
    {
        private readonly IJournalRepository _journalRepository;
        private readonly IIssueRepository _issueRepository;
        private readonly IStaticMembershipService _membershipService;

        public PublisherController(IJournalRepository journalRepo, IIssueRepository issueRepository, IStaticMembershipService membershipService)
        {
            _journalRepository = journalRepo;
            _issueRepository = issueRepository;
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
            var journalIssues = _journalRepository.GetJournalIssues(id).ToList();
            var issueViewModels = Mapper.Map<List<Issue>, List<JournalIssueViewModel>>(journalIssues);
            var issues = new JournalIssueViewList(issueViewModels);

            return View(issues);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(JournalViewModel journal)
        {
            if (ModelState.IsValid)
            {
                var newJournal = Mapper.Map<JournalViewModel, Journal>(journal);
                JournalHelper.PopulateFile(journal.File, newJournal.Issues.First());

                newJournal.UserId = (int)_membershipService.GetUser().ProviderUserKey;

                var opStatus = _journalRepository.AddJournal(newJournal);
                if (!opStatus.Status)
                    throw new System.Web.Http.HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError));

                return RedirectToAction("Index");
            }
            else
                return View(journal);
        }
        public ActionResult CreateIssue(int id)
        {
            var journal = _journalRepository.GetJournalById(id);

            if (journal != null)
            {
                var model = new JournalIssueViewModel
                {
                    JournalId = journal.Id,
                };

                return View(model);
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateIssue(JournalIssueViewModel newIssue)
        {
            if (ModelState.IsValid)
            {
                var issue = Mapper.Map<JournalIssueViewModel, Issue>(newIssue);
                JournalHelper.PopulateFile(newIssue.File, issue);

                var journal = _journalRepository.GetJournalById(issue.JournalId);

                if (journal != null)
                {
                    issue.Journal = journal;
                    issue.JournalId = journal.Id;

                    var opStatus = _issueRepository.AddIssue(issue);

                    if (!opStatus.Status)
                        throw new System.Web.Http.HttpResponseException(new HttpResponseMessage(HttpStatusCode.InternalServerError));

                    return RedirectToAction("Issues", new { id = journal.Id });
                }
            }
            return View(newIssue);
        }

        public ActionResult GetFile(int id)
        {
            var issue = _issueRepository.GetIssueById(id);
            if (issue == null)
                throw new System.Web.Http.HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound));

            return File(issue.Content, issue.ContentType);
        }

        public ActionResult Delete(int id)
        {
            var selectedJournal = _journalRepository.GetJournalById(id);
            var journal = Mapper.Map<Journal, JournalViewModel>(selectedJournal);
            return View(journal);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(JournalViewModel model)
        {
            var journal = _journalRepository.GetJournalById(model.Id);

            foreach (var issue in journal.Issues.ToArray())
            {
                _issueRepository.DeleteIssue(issue);
            }

            var opStatus = _journalRepository.DeleteJournal(journal);
            if (!opStatus.Status)
                throw new System.Web.Http.HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound));

            return RedirectToAction("Index");
        }
        public ActionResult DeleteIssue(int id)
        {
            var issue = _issueRepository.GetIssueById(id);
            var issueViewModel = Mapper.Map<Issue, JournalIssueViewModel>(issue);
            return View(issueViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteIssue(JournalIssueViewModel issueViewModel)
        {
            var issue = _issueRepository.GetIssueById(issueViewModel.Id);
            var journalId = issue.JournalId;
            var opStatus = _issueRepository.DeleteIssue(issue);
            if (!opStatus.Status)
                throw new System.Web.Http.HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound));

            // Check if the journal has 0 issues.
            // If so, delete the journal
            var journal = _journalRepository.GetJournalById(journalId);

            if (journal.Issues == null || !journal.Issues.Any())
            {
                _journalRepository.DeleteJournal(journal);
                return RedirectToAction("Index");
            }
            return RedirectToAction("Issues", new { id = issue.JournalId });
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

                // TODO Not clear how to handle the existing issues from the editted journal
                // JournalHelper.PopulateFile(journal.File, selectedJournal, selectedJournal.Issues.First());

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