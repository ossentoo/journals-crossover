﻿using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Journals.Model;
using Medico.Model;
using Medico.Repository.Interfaces;

namespace Medico.Repository.Repositories
{

    public interface IDbContext
    {
        bool IsDisposed { get; set; }

        int SaveChanges();
    }

    public class JournalRepository : Repository<Journal>, IJournalRepository
    {
        public JournalRepository(DbContext context) : base(context)
        {
        }

        public IEnumerable<Journal> GetAllJournals(int userId)
        {
            return Get(u=>u.UserId == userId);
        }

        public IEnumerable<Issue> GetJournalIssues(int journalId)
        {
            var journal = Get(j => j.Id == journalId)
                .OrderBy(x=>x.ModifiedDate)
                .First();

            return journal?.Issues;
        }

        public OperationStatus AddJournal(Journal newJournal)
        {
            Add(newJournal);
            var operationStatus = new OperationStatus {Status = true};
            return operationStatus;
        }

        public Journal GetJournalById(int id)
        {
            return Get(j=>j.Id==id).FirstOrDefault();
        }

        public OperationStatus DeleteJournal(Journal journal)
        {
            Delete(journal);
            var operationStatus = new OperationStatus { Status = true };
            return operationStatus;
        }

        public OperationStatus UpdateJournal(Journal journal)
        {
            Update(journal);
            var operationStatus = new OperationStatus { Status = true };
            return operationStatus;
        }
    }
}