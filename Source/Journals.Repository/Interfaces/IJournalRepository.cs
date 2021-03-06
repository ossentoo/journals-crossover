﻿using System.Collections.Generic;
using Journals.Model;
using Medico.Model;

namespace Medico.Repository.Interfaces
{
    public interface IJournalRepository
    {
        IEnumerable<Journal> GetAllJournals(int userId);

        OperationStatus AddJournal(Journal newJournal);

        Journal GetJournalById(int Id);

        OperationStatus DeleteJournal(Journal journal);

        OperationStatus UpdateJournal(Journal journal);

        IEnumerable<Issue> GetJournalIssues(int journalId);
    }
}