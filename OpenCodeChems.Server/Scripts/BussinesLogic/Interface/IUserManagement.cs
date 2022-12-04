using System.Collections.Generic;
using OpenCodeChems.DataAccess;

namespace OpenCodeChems.BusinessLogic.Interface
{

    public interface IUserManagement
    {
        bool RegisterUser(User user);
        bool RegisterProfile(Profile profile);
        bool Login(string username, string password);
        bool EditProfileNickname(string username, string nickname);
        bool EditProfileImage(string username, byte[] imageProfile);
        bool EditUserPassword(string username, string newHashedPassword);
        bool PasswordExist(string username, string hashPassword);
        Profile GetProfile(string username);
        bool EmailRegistered(string email);
        bool UsernameRegistered(string username);
        bool NicknameRegistered(string nickname);
        bool AddFriend(Friends friends);
        bool AcceptFriendRequest(Friends friends);
        bool DenyFriendRequest(Friends friends);
        bool FriendshipExist(int idProfileActualPlayer, int idProfilePlayerFound);
        List<string> GetFriends(int idProfile);
    }


}
