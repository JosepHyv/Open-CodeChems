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
		Profile GetProfileByUsername(string username);
		bool EmailRegistered(string email);
		bool UsernameRegistered(string username);
		bool NicknameRegistered(string nickname);
		bool AddFriend(Friends friends);
		bool AcceptFriend(Friends friends);
		bool DenyFriend(Friends friends);
		bool DeleteFriend(Friends friends);
		bool FriendshipExist(int idProfileActualPlayer, int idProfilePlayerFound);
		List<string> GetFriends(int idProfile, bool status);
		List<string> GetFriendsRequests(int idProfile, bool status);
		Profile GetProfileByNickname(string nickname);
	}


}
