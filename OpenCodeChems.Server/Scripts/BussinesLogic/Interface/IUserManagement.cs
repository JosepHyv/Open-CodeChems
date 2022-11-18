using OpenCodeChems.DataAccess;

namespace OpenCodeChems.BusinessLogic.Interface
{
    public interface IUserManagement
    {
        bool RegisterUser(User user, string nikname);
        bool Login(string username, string password);
    }

}
