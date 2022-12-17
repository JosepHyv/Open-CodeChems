using System.Collections.Generic;
using OpenCodeChems.DataAccess;

namespace OpenCodeChems.BusinessLogic.Interface
{
 	/// <summary>
   	/// Interface for UserManagement methods
   	/// </summary>
	public interface IUserManagement
	{
		/// <summary>
        /// register a new user in the database
        /// </summary>
        /// <remarks>
        /// evaluates whether a new register has been successfully entered into the database
        /// </remarks>
        /// <param name = "user"> receives an object of type User </param>
        /// <returns> boolean with true value if performed correctly </returns>
		bool RegisterUser(User user);
		
		/// <summary>
        /// register a new profile in the database
        /// </summary>
        /// <remarks>
        /// evaluates whether a new register has been successfully entered into the database
        /// </remarks>
        /// <param name = "profile"> receives an object of type Profile </param>
        /// <returns> boolean with true value if performed correctly </returns>
		bool RegisterProfile(Profile profile);

		/// <summary>
        /// check whether a user exists with the provided username and password
        /// </summary>
        /// <remarks>
        /// evaluates whether the credentials entered are correct
        /// </remarks>
        /// <param name = "username"> receives an string with the username of the user </param>
        /// <param name = "password"> receives an string with the hash password of the user </param>
        /// <returns> boolean with true value if credentials match a user </returns>
		bool Login(string username, string password);

		/// <summary>
        /// assigns a new nickname to the user it profile with the provided username
        /// </summary>
        /// <remarks>
        /// evaluates if there is a profile with the specified username, if so it changes its nickname
        /// </remarks>
        /// <param name = "username"> receives an string with the username of the user </param>
        /// <param name = "nickname"> receives an string with the new nickname of the user </param>
        /// <returns> boolean with true value if it could update the new nickname </returns>
		bool EditProfileNickname(string username, string nickname);

		/// <summary>
        /// assigns a new image profile to the profile it finds with the provided username
        /// </summary>
        /// <remarks>
        /// evaluates if there is a profile with the specified username, if so it changes its image profile
        /// </remarks>
        /// <param name = "username"> receives an string with the username of the user </param>
        /// <param name = "imageProfile"> receives an array of bytes with the new image profile of the user </param>
        /// <returns> boolean with true value if it could update the new image profile </returns>
		bool EditProfileImage(string username, int imageProfile);

		/// <summary>
        /// assigns a new password to the user it finds with the provided username
        /// </summary>
        /// <remarks>
        /// evaluates if there is a user with the specified username, if so it changes its password
        /// </remarks>
        /// <param name = "username"> receives an string with the username of the user </param>
        /// <param name = "newHashedPassword"> receives an string with the new password of the user </param>
        /// <returns> boolean with true value if it could update the new password </returns>
		bool EditUserPassword(string username, string newHashedPassword);

		/// <summary>
        /// check if exist a register with the hash password and the username
        /// </summary>
        /// <param name = "username"> receives an string with the username of the user </param>
        /// <param name = "hashPassword"> receives an string with the password of the user </param>
        /// <returns> boolean with true value if exist a register with the password </returns>
		bool PasswordExist(string username, string hashPassword);

		/// <summary>
        /// gets an object of type Profile  
        /// </summary>
        /// <remarks>
        /// evaluates if there is a Profile with the specified username, if so it gets the Profile
        /// </remarks>
        /// <param name = "username"> receives an string with the username of the user </param>
        /// <returns> object of type Profile </returns>
		Profile GetProfileByUsername(string username);

		/// <summary>
        /// check if exist a register with the email provided 
        /// </summary>
        /// <param name = "email"> receives an string with the email of the user </param>
        /// <returns>boolean with true value if exist a register with the email</returns>
		bool EmailRegistered(string email);

		/// <summary>
        /// check if exist a register with the username provided 
        /// </summary>
        /// <param name = "username"> receives an string with the username of the user </param>
        /// <returns>boolean with true value if exist a register with the username</returns>
		bool UsernameRegistered(string username);

		/// <summary>
        /// check if exist a register with the nickname provided 
        /// </summary>
        /// <param name = "nickname"> receives an string with the nickname of the user </param>
        /// <returns>boolean with true value if exist a register with the nickname</returns>
		bool NicknameRegistered(string nickname);

		/// <summary>
        /// register a new friendship in database 
        /// </summary>
        /// <remarks>
        /// register a object of type Friends with the status false
        /// </remarks>
        /// <param name = "friends"> receives an object of type Friends </param>
        /// <returns>boolean with true value if could register the friends</returns>
		bool AddFriend(Friends friends);

		/// <summary>
        /// update a friendship in database 
        /// </summary>
        /// <remarks>
        /// update a object of type Friends with the status false to status true, first check if exist a register with the idProfileFrom in the idProfileFrom column and the idProfileTo in the ifProfileTo column
        /// </remarks>
        /// <param name = "friends"> receives an object of type Friends </param>
        /// <returns>boolean with true value if could update the new friends</returns>
		bool AcceptFriend(Friends friends);

		 /// <summary>
        /// dalete a friendship in database 
        /// </summary>
        /// <remarks>
        /// delete a register of Friends table if the status is equal to false
        /// </remarks>
        /// <param name = "friends"> receives an object of type Friends </param>
        /// <returns>boolean with true value if could delete the friends registerd</returns>
		bool DenyFriend(Friends friends);

		/// <summary>
        /// dalete a friendship in database 
        /// </summary>
        /// <remarks>
        /// delete a register of Friends table if exist a register with the idProfileFrom in the idProfileFrom column and idProfileTo in the idProfileTo column, else exist a register with the idProfileTo in the idProfileFrom column and idProfileTo in the idProfileFrom column and the status is equal to true;
        /// </remarks>
        /// <param name = "friendsForDelete"> receives an object of type Friends </param>
        /// <returns>boolean with true value if could delete the friends registerd</returns>
		bool DeleteFriend(Friends friendsForDelete);

		 /// <summary>
        /// check if exist a friendship in the database
        /// </summary>
        /// <remarks>
        /// check if the idProfileFrom column exist the idProfileActualPlayer and in the idProfileTo column exist the idProfilePlayerFound, else the the idProfileFrom column exist the idProfilePlayerFound and in the idProfileTo column exist the idProfileActualPlayer
        /// </remarks>
        /// <param name = "idProfileActualPlayer"> receives an int with the id profile of the actual player </param>
        /// <param name = "idProfilePlayerFound"> receives an int with the id profile of the player want to send a friend request </param>
        /// <returns>boolean with true value if friendship exist</returns>
		bool FriendshipExist(int idProfileActualPlayer, int idProfilePlayerFound);

		/// <summary>
        /// gets the friends of the actual player
        /// </summary>
        /// <remarks>
        /// gets the id of the profiles with wich the actual player has a friendship and the status is equal to true, replaces the id for the nicknames of the profiles
        /// </remarks>
        /// <param name = "idProfile"> receives an int with de id profle of the actual player </param>
        /// <returns>List with the friends of the actual player</returns>
		List<string> GetFriends(int idProfile);

		/// <summary>
        /// gets the friends requests of the actual player
        /// </summary>
        /// <remarks>
        /// gets the id of the profiles with wich the actual player has a friendship and the status is equal to false, replaces the id for the nicknames of the profiles
        /// </remarks>
        /// <param name = "idProfile"> receives an int with de id profle of the actual player </param>
        /// <returns>List with the friends requests of the actual player</returns>
		List<string> GetFriendsRequests(int idProfile);

		/// <summary>
        /// gets an object of type Profile  
        /// </summary>
        /// <remarks>
        /// evaluates if there is a Profile with the specified nickname, if so it gets the Profile
        /// </remarks>
        /// <param name = "nickname"> receives an string with the nickname of the user </param>
        /// <returns> object of type Profile </returns>
		Profile GetProfileByNickname(string nickname);

		/// <summary>
        /// dalete a user that played as invitated
        /// </summary>
        /// <param name = "username"> receives a string with de username of the player </param>
        /// <returns>boolean with true value if could delete the user registerd</returns>
		bool DeleteInvitatedPlayer(string username);

		/// <summary>
        /// add a victory to the player with de nickname provided 
        /// </summary>
        /// <param name = "nickname"> receives a string with the nickname of the user </param>
        /// <returns>boolean with true value if could update the victories of the user</returns>
		bool AddVictory(string nickname);

		/// <summary>
        /// add a defeat to the player with de nickname provided 
        /// </summary>
        /// <param name = "nickname"> receives a string with the nickname of the user </param>
        /// <returns>boolean with true value if could update the defeats of the user</returns>
		bool AddDefeat(string nickname);

		/// <summary>
        /// assigns a new password to the user it finds with the provided email
        /// </summary>
        /// <remarks>
        /// evaluates if there is a user with the specified email, if so it changes its password
        /// </remarks>
        /// <param name = "email"> receives an string with the email of the user </param>
        /// <param name = "newHashedPassword"> receives an string with the new password of the user </param>
        /// <returns> boolean with true value if it could update the new password </returns>
		bool RestorePassword(string email, string newHashedPassword);

		/// <summary>
        /// check if exist a register in Friends the object provided
        /// </summary>
        /// <param name = "friends"> receives an object of type Friends for search the form of friendship </param>
        /// <returns>boolean with true value if exist a register with the friends</returns>
		bool SearchFriends(Friends friends);
	}


}
