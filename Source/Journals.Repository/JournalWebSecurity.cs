using Medico.Repository.Interfaces;
using WebMatrix.WebData;

namespace Medico.Repository
{
    public class JournalWebSecurity : IWebSecurity
    {
        public void Logout()
        {
            WebSecurity.Logout();
        }

        public bool Login(string userName, string password)
        {
            return WebSecurity.Login(userName, password);
        }

        public bool Login(string userName, string password, bool persistCookie)
        {
            return WebSecurity.Login(userName, password, persistCookie);
        }

        public string CreateUserAndAccount(string userName, string password)
        {
            return WebSecurity.CreateUserAndAccount(userName, password);
        }

        public string CreateAccount(string userName, string password)
        {
            return WebSecurity.CreateAccount(userName, password);
        }

        public int GetUserId(string userName)
        {
            return WebSecurity.GetUserId(userName);
        }

        public bool ChangePassword(string userName, string oldPassword, string newPassword)
        {
            return WebSecurity.ChangePassword(userName, oldPassword, newPassword);
        }
    }
}
