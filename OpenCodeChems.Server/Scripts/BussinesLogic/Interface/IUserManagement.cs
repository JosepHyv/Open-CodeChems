using OpenCodeChems.DataAccess;

namespace OpenCodeChems.BusinessLogic.Interface
{
    public interface IUserManagement
    {
        bool RegisterUser(User user, Profile profile);
        bool Login(string username, string password);
        bool EditProfileNickname(Profile profile, string nickname);
        bool EditProfileImage(Profile profile, byte[] imageProfile);
        bool EditUserEmail(User user, string email);
        bool EditUserPassword(User user, string actualHashedPassword, string newHashedPassword);
        string GetOldPassword(User user);
        
    }

}
