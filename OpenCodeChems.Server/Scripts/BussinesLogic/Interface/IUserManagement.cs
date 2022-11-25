using OpenCodeChems.DataAccess;

namespace OpenCodeChems.BusinessLogic.Interface
{
    public interface IUserManagement
    {
		bool RegisterUser(User user);
		bool RegisterProfile(Profile profile);
		bool Login(string username, string password);
		bool EditProfileNickname(Profile profile, string nickname);
		bool EditProfileImage(Profile profile, byte[] imageProfile);
		bool EditUserEmail(User user, string email);
		bool EditUserPassword(User user, string actualHashedPassword, string newHashedPassword);
		string GetOldPassword(User user);
		Profile GetProfile(string username);
        bool EmailRegistered(string email);
        bool UsernameRegistered(string username);
        bool NicknameRegistered(string nickname);
        bool AddFriend(Friends friends);
		bool AcceptFriendRequest(Friends friends);
		bool DenyFriendRequest(Friends friends);
    }

}
