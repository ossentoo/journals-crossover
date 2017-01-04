using Journals.Model;
using System.Collections.Generic;

namespace Journals.Repository
{
    public interface IJournalRepository
    {
        List<Journal> GetAllJournals(int userId);

        OperationStatus AddJournal(Journal newJournal);

        Journal GetJournalById(int Id);

        OperationStatus DeleteJournal(Journal journal);

        OperationStatus UpdateJournal(Journal journal);
    }
}