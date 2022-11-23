using OpenCodeChems.DataAccess;

namespace OpenCodeChems.BusinessLogic.Interface
{
    public interface IUserManagement
    {
        bool RegisterUser(User user);
        bool Login(string username, string password);
        bool EditProfileNickname(User user, string nickname);
        bool EditProfileImage(User user, byte[] imageProfile);
        bool EditUserEmail(User user, string email);
        bool EditUserPassword(User user, string actualHashedPassword, string newHashedPassword);
        string GetOldPassword(User user);
        User GetUser(string username);
        
    }

}
