using System.Collections.Generic;
using Journals.Model;
using Medico.Model;

namespace Medico.Repository.Interfaces
{
    public interface IIssueRepository
    {
        OperationStatus AddIssue(Issue issue);

        Issue GetIssueById(int id);

        OperationStatus DeleteIssue(Issue journal);
    }
}