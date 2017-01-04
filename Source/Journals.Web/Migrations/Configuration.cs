using System.Data.Entity.Migrations;
using Journals.Repository.DataContext;

namespace Medico.Web.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<UsersContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(UsersContext context)
        {
        }
    }
}