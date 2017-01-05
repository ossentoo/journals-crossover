using System.Data.Entity;
using Journals.Model;

namespace Medico.Repository.DataContext
{
    public class UsersContext : DbContext
    {
        public UsersContext()
            : base("JournalsDB")
        {
        }

        public DbSet<UserProfile> UserProfiles { get; set; }
    }
}