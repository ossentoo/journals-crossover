using System.Web.Security;

namespace Medico.Repository
{
    public interface IStaticMembershipService
    {
        MembershipUser GetUser();

        bool IsUserInRole(string userName, string roleName);
    }
}