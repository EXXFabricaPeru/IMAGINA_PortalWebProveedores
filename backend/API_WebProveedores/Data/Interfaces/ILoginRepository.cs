
using System.Collections.Generic;
using WebProov_API.Models;

namespace WebProov_API.Data.Interfaces
{
    public interface ILoginRepository
    {
        User GetLogin(User user);
        string ChangePassword(User user);
        string envioReestablecerPass(User user);
    }
}