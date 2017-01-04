using WebMatrix.WebData;

namespace Journals.Repository.DataContext
{
    public class SimpleMembershipInitializer
    {
        public SimpleMembershipInitializer()
        {
            using (var context1 = new JournalsContext())
                context1.Journals.Find(1);

            using (var context = new UsersContext())
                context.UserProfiles.Find(1);

            if (!WebSecurity.Initialized)
                WebSecurity.InitializeDatabaseConnection("JournalsDB", "UserProfile", "UserId", "UserName", autoCreateTables: true);
        }
    }
}