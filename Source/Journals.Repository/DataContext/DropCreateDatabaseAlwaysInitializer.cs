using System.Data.Entity;

namespace Medico.Repository.DataContext
{
    public class DropCreateDatabaseAlwaysInitializer : DropCreateDatabaseAlways<JournalsContext>
    {
        protected override void Seed(JournalsContext context)
        {
            DataInitializer.Initialize(context);
            base.Seed(context);
        }
    }
}