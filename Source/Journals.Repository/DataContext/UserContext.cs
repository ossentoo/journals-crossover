using Journals.Model;
using System.Data.Entity;

namespace Journals.Repository.DataContext
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