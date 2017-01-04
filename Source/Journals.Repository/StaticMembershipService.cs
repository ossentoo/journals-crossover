using System.Web.Security;
using WebMatrix.WebData;

namespace Journals.Repository
{
    public class StaticMembershipService : IStaticMembershipService
    {
        public System.Web.Security.MembershipUser GetUser()
        {
            return Membership.GetUser();
        }

        public bool IsUserInRole(string userName, string roleName)
        {
            var roles = (SimpleRoleProvider)Roles.Provider;
            return roles.IsUserInRole(userName, roleName);
        }
    }
}