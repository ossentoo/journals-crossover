using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using Medico.Model;

namespace Medico.Repository.DataContext
{
    public class JournalsContext : DbContext, IDbContext
    {
        public JournalsContext()
            :base("name=JournalsDB")
        {
            
        }

        public virtual DbSet<Journal> Journals { get; set; }
        public virtual DbSet<JournalIssue> JournalIssues { get; set; }
        public virtual DbSet<Subscription> Subscriptions { get; set; }
        public bool IsDisposed { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            base.Configuration.LazyLoadingEnabled = false;
            modelBuilder.Entity<Journal>().ToTable("Journals");
            modelBuilder.Entity<Subscription>().ToTable("Subscriptions");
            modelBuilder.Entity<JournalIssue>().ToTable("JournalIssues");
            base.OnModelCreating(modelBuilder);
        }

        protected override void Dispose(bool disposing)
        {
            IsDisposed = true;
            base.Dispose(disposing);
        }
    }
}