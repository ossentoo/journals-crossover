using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using Journals.Model;
using Medico.Model;

namespace Medico.Repository.DataContext
{
    public class JournalsContext : DbContext, IDbContext
    {
        public JournalsContext(string connectionString)
            : base(connectionString)
        {
        }

        public JournalsContext()
        {
            
        }

        public virtual DbSet<Journal> Journals { get; set; }
        public virtual DbSet<Subscription> Subscriptions { get; set; }
        public bool IsDisposed { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            base.Configuration.LazyLoadingEnabled = false;
            modelBuilder.Entity<Journal>().ToTable("Journals");
            modelBuilder.Entity<Subscription>().ToTable("Subscriptions");
            base.OnModelCreating(modelBuilder);
        }

        protected override void Dispose(bool disposing)
        {
            IsDisposed = true;
            base.Dispose(disposing);
        }
    }
}