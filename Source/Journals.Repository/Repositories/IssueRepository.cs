using System.Data.Entity;
using System.Linq;
using Journals.Model;
using Medico.Model;
using Medico.Repository.Interfaces;

namespace Medico.Repository.Repositories
{
    public class IssueRepository : Repository<Issue>, IIssueRepository
    {
        public IssueRepository(DbContext context) : base(context)
        {
        }

        public OperationStatus AddIssue(Issue issue)
        {
            Add(issue);
            var operationStatus = new OperationStatus {Status = true};
            return operationStatus;
        }

        public Issue GetIssueById(int id)
        {
            return Get(j=>j.Id==id).FirstOrDefault();
        }

        public OperationStatus DeleteIssue(Issue journal)
        {
            Delete(journal);
            var operationStatus = new OperationStatus { Status = true };
            return operationStatus;
        }
    }
}