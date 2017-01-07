using System.Web.Security;

namespace Medico.Repository.Interfaces
{
    public interface IStaticMembershipService
    {
        MembershipUser GetUser();

        bool IsUserInRole(string userName, string roleName);
    }
}