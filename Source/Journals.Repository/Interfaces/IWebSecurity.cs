
namespace Medico.Repository.Interfaces
{
    public interface IWebSecurity
    {
        void Logout();
        bool Login(string userName, string password);
        bool Login(string userName, string password, bool persistCookie);
        string CreateUserAndAccount(string userName, string password);
        string CreateAccount(string userName, string password);
        int GetUserId(string userName);
        bool ChangePassword(string userName, string ldPassword, string newPassword);
    }
}
