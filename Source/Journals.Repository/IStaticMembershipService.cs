using System.Web.Security;

namespace Journals.Repository
{
    public interface IStaticMembershipService
    {
        MembershipUser GetUser();

        bool IsUserInRole(string userName, string roleName);
    }
}