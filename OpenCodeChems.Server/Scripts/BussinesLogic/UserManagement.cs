using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenCodeChems.DataAccess;
using OpenCodeChems.BusinessLogic.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Data.SqlClient;
using Godot;

namespace OpenCodeChems.BussinesLogic
{
    /// <summary>
    /// Class that manage the users
    /// </summary>
    /// <remarks>
    /// This class is in charge of handling everything related to users and making registrations, queries and deletions in the database about them.
    /// </remarks>

    public class UserManagement : IUserManagement
    {
        
        /// <summary>
        /// register a new user in the database
        /// </summary>
        /// <remarks>
        /// evaluates whether a new register has been successfully entered into the database
        /// </remarks>
        /// <param name = "user"> receives an object of type User </param>
        /// <returns> boolean with true value if performed correctly </returns>
        /// <exception cref="DbUpdateException">throw if lost connection with the database</exception>
        /// <exception cref="InvalidOperationException">throw if the object type User is null</exception>

        public bool RegisterUser(User user)
        {  
            bool status = false;
            using (OpenCodeChemsContext context = new OpenCodeChemsContext())
            {
                
                try
                {
                    context.User.Add(user);
                    context.SaveChanges();
                    status = true;
                }
                catch (DbUpdateException)
                {
                    status = false;
                }
                catch(InvalidOperationException)
                {
                    status = false;
                }
            }
            return status;
        }
        /// <summary>
        /// register a new profile in the database
        /// </summary>
        /// <remarks>
        /// evaluates whether a new register has been successfully entered into the database
        /// </remarks>
        /// <param name = "profile"> receives an object of type Profile </param>
        /// <returns> boolean with true value if performed correctly </returns>
        /// <exception cref="DbUpdateException">throw if lost connection with the database</exception>
        /// <exception cref="InvalidOperationException">throw if the object type Profile is null</exception>
        public bool RegisterProfile(Profile profile)
		{  
			bool status = false;
			using (OpenCodeChemsContext context = new OpenCodeChemsContext())
			{
				
				try
				{
					context.Profile.Add(profile);
					context.SaveChanges();
					status = true;
				}
				catch (DbUpdateException)
				{
					status = false;
				}
                catch(InvalidOperationException)
                {
                    status = false;
                }
			}
			return status;
		}


        /// <summary>
        /// check whether a user exists with the provided username and password
        /// </summary>
        /// <remarks>
        /// evaluates whether the credentials entered are correct
        /// </remarks>
        /// <param name = "username"> receives an string with the username of the user </param>
        /// <param name = "password"> receives an string with the hash password of the user </param>
        /// <returns> boolean with true value if credentials match a user </returns>
        /// <exception cref="InvalidOperationException">throw if the username or password is null</exception>
        public bool Login(string username, string password) 
        {
            
            bool status = false;
            try
            {
                using (OpenCodeChemsContext context = new OpenCodeChemsContext())
                {
                    int foundUser = (from User in context.User where User.username == username && User.password == password   select User).Count();
                    if (foundUser > 0)
                    {
                        status = true;
                    }
                }
            }
            catch (InvalidOperationException)
            {
                status = false;
            }
            
            return status;
        }
        
        /// <summary>
        /// assigns a new nickname to the user it profile with the provided username
        /// </summary>
        /// <remarks>
        /// evaluates if there is a profile with the specified username, if so it changes its nickname
        /// </remarks>
        /// <param name = "username"> receives an string with the username of the user </param>
        /// <param name = "nickname"> receives an string with the new nickname of the user </param>
        /// <returns> boolean with true value if it could update the new nickname </returns>
        /// <exception cref="DbUpdateException">throw if lost connection with the database</exception>
        /// <exception cref="InvalidOperationException">throw if username or newNickname is null</exception>
        public bool EditProfileNickname(string username, string nickname)
        {
            bool status = false;
            try
            {
                using (OpenCodeChemsContext context = new OpenCodeChemsContext())
                {   
                    var profiles = (from Profile in context.Profile where Profile.username == username select Profile).First();
                    profiles.nickname = nickname;
                    context.SaveChanges();
                    status = true;
                }
            }
            catch (DbUpdateException)
            {
                status = false;
            }
            catch(InvalidOperationException)
            {
                status = false;
            }
            return status;
        }

        /// <summary>
        /// assigns a new image profile to the profile it finds with the provided username
        /// </summary>
        /// <remarks>
        /// evaluates if there is a profile with the specified username, if so it changes its image profile
        /// </remarks>
        /// <param name = "username"> receives an string with the username of the user </param>
        /// <param name = "imageProfile"> receives an array of bytes with the new image profile of the user </param>
        /// <returns> boolean with true value if it could update the new image profile </returns>
        /// <exception cref="DbUpdateException">throw if lost connection with the database</exception>
        /// <exception cref="InvalidOperationException">throw if the username is null</exception>
        public bool EditProfileImage(string username, int imageProfile)
        {
            bool status = false;
            try
            {
                using (OpenCodeChemsContext context = new OpenCodeChemsContext())
                {   
                    var profiles = (from Profile in context.Profile where Profile.username == username  select Profile).First();
                    profiles.imageProfile = imageProfile;
                    context.SaveChanges();
                    status = true;
                }
            }
            catch (DbUpdateException)
            {
                status = false;
            }
            catch(InvalidOperationException)
            {
                status = false;
            }
            return status;
        }

        /// <summary>
        /// check if exist a register with the hash password and the username
        /// </summary>
        /// <param name = "username"> receives an string with the username of the user </param>
        /// <param name = "hashPassword"> receives an string with the password of the user </param>
        /// <returns> boolean with true value if exist a register with the password </returns>
        public bool PasswordExist(string username, string hashPassword)
        {
            bool existPassword = false;
            using(OpenCodeChemsContext context = new OpenCodeChemsContext())
            {
                int oldPasswordFound = (from User in context.User where User.username.Equals(username) && User.password.Equals(hashPassword) select User).Count();
                if (oldPasswordFound > 0)
                {
                    existPassword = true;
                }
            }
            return existPassword;
        }
        
        /// <summary>
        /// assigns a new password to the user it finds with the provided username
        /// </summary>
        /// <remarks>
        /// evaluates if there is a user with the specified username, if so it changes its password
        /// </remarks>
        /// <param name = "username"> receives an string with the username of the user </param>
        /// <param name = "newHashedPassword"> receives an string with the new password of the user </param>
        /// <returns> boolean with true value if it could update the new password </returns>
        /// <exception cref="DbUpdateException">throw if lost connection with the database</exception>
        /// <exception cref="InvalidOperationException">throw if the username or newHashedPassword is null</exception>
        public bool EditUserPassword(string username, string newHashedPassword)
        {
            bool status = false;
            try
            {
                using(OpenCodeChemsContext context = new OpenCodeChemsContext())
                {
                    var users = (from User in context.User where User.username == username  select User).First();
                    users.password = newHashedPassword;
                    context.SaveChanges();
                    status = true;                           
                }
            }
            catch (DbUpdateException)
            {
                status = false;
            }
            catch(InvalidOperationException)
            {
                status = false;
            }
            return status;
        }

        /// <summary>
        /// gets an object of type Profile  
        /// </summary>
        /// <remarks>
        /// evaluates if there is a Profile with the specified username, if so it gets the Profile
        /// </remarks>
        /// <param name = "username"> receives an string with the username of the user </param>
        /// <returns> object of type Profile </returns>
        /// <exception cref="DbUpdateException">throw if lost connection with the database</exception>
        /// <exception cref="InvalidOperationException">throw if the username is null</exception>
        public Profile GetProfileByUsername(string username)
		{
            Profile profileObteined = null;
			try
			{
				using(OpenCodeChemsContext context = new OpenCodeChemsContext())
				{
					profileObteined = (from Profile in context.Profile where Profile.username == username select Profile).First();
					context.SaveChanges();
				}
			}
			catch (DbUpdateException)
			{
                profileObteined = null;
			}
            catch (InvalidOperationException)
            {
                profileObteined = null;
            }
            return profileObteined;
		}
        /// <summary>
        /// check if exist a register with the email provided 
        /// </summary>
        /// <param name = "email"> receives an string with the email of the user </param>
        /// <returns>boolean with true value if exist a register with the email</returns>
        /// <exception cref="DbUpdateException">throw if lost connection with the database</exception>
        /// <exception cref="InvalidOperationException">throw if the email is null</exception>
        public bool EmailRegistered(string email)
        {
            bool isRegistered = false;
            using(OpenCodeChemsContext context = new OpenCodeChemsContext())
            {
                int emailFound = (from User in context.User where User.email.Equals(email) select User).Count();
                if (emailFound > 0)
                {
                    isRegistered = true;
                }
            }
            return isRegistered;
        }

        /// <summary>
        /// check if exist a register with the username provided 
        /// </summary>
        /// <param name = "username"> receives an string with the username of the user </param>
        /// <returns>boolean with true value if exist a register with the username</returns>
        /// <exception cref="DbUpdateException">throw if lost connection with the database</exception>
        /// <exception cref="InvalidOperationException">throw if the username is null</exception>
        public bool UsernameRegistered(string username)
        {
            bool isRegistered = false;
            using(OpenCodeChemsContext context = new OpenCodeChemsContext())
            {
                int usernameFound = (from User in context.User where User.username.Equals(username) select User).Count();
                if (usernameFound > 0)
                {
                    isRegistered = true;
                }
            }
            return isRegistered;
        } 

        /// <summary>
        /// check if exist a register with the nickname provided 
        /// </summary>
        /// <param name = "nickname"> receives an string with the nickname of the user </param>
        /// <returns>boolean with true value if exist a register with the nickname</returns>
        /// <exception cref="DbUpdateException">throw if lost connection with the database</exception>
        /// <exception cref="InvalidOperationException">throw if the nickname is null</exception>
        public bool NicknameRegistered(string nickname)
        {
            bool isRegistered = false;
            using(OpenCodeChemsContext context = new OpenCodeChemsContext())
            {
                int nicknameFound = (from Profile in context.Profile where Profile.nickname.Equals(nickname) select Profile).Count();
                if (nicknameFound > 0)
                {
                    isRegistered = true;
                }
            }
            return isRegistered;
        }

        /// <summary>
        /// register a new friendship in database 
        /// </summary>
        /// <remarks>
        /// register a object of type Friends with the status false
        /// </remarks>
        /// <param name = "friends"> receives an object of type Friends </param>
        /// <returns>boolean with true value if could register the friends</returns>
        /// <exception cref="DbUpdateException">throw if lost connection with the database</exception>
        /// <exception cref="InvalidOperationException">throw if object type friends is null</exception>
        public bool AddFriend(Friends friends)
		{  
			bool status = false;
			using (OpenCodeChemsContext context = new OpenCodeChemsContext())
			{
				
				try
				{
					context.Friends.Add(friends);
					context.SaveChanges();
					status = true;
				}
				catch (DbUpdateException)
				{
					status = false;
				}
                catch (InvalidOperationException)
                {
                    status = false;
                }
			}
			return status;
		} 

        /// <summary>
        /// update a friendship in database 
        /// </summary>
        /// <remarks>
        /// update a object of type Friends with the status false to status true, first check if exist a register with the idProfileFrom in the idProfileFrom column and the idProfileTo in the ifProfileTo column
        /// </remarks>
        /// <param name = "friends"> receives an object of type Friends </param>
        /// <returns>boolean with true value if could update the new friends</returns>
        /// <exception cref="DbUpdateException">throw if lost connection with the database</exception>
        /// <exception cref="InvalidOperationException">throw if object type friends is null</exception>
        public bool AcceptFriend(Friends friends)
        {
            bool status = false;
            try
            {
                int idProfileFrom = friends.idProfileFrom;
                int idProfileTo = friends.idProfileTo;
                using (OpenCodeChemsContext context = new OpenCodeChemsContext())
                {   
                    var friendsUpdate = (from Friends in context.Friends where Friends.idProfileFrom == idProfileFrom && Friends.idProfileTo ==  idProfileTo select Friends).First();
                    friendsUpdate.status = friends.status;
                    context.SaveChanges();
                    status = true;
                }
            }
            catch (DbUpdateException)
            {
                status = false;
            }
            catch (InvalidOperationException)
            {
                status = false;
            }
            return status;
        } 

        /// <summary>
        /// dalete a friendship in database 
        /// </summary>
        /// <remarks>
        /// delete a register of Friends table if the status is equal to false
        /// </remarks>
        /// <param name = "friends"> receives an object of type Friends </param>
        /// <returns>boolean with true value if could delete the friends registerd</returns>
        /// <exception cref="DbUpdateException">throw if lost connection with the database</exception>
        /// <exception cref="InvalidOperationException">throw if object type friends is null</exception>
        public bool DenyFriend(Friends friends)
        {
            bool status = false;
            try{
                using (OpenCodeChemsContext context = new OpenCodeChemsContext())
                {
                    int idProfileFrom = friends.idProfileFrom;
                    int idProfileTo = friends.idProfileTo;
                    var friendsDelete = (from Friends in context.Friends where Friends.idProfileFrom == idProfileFrom && Friends.idProfileTo ==  idProfileTo select Friends).First();
                    context.Friends.Remove(friendsDelete);
                    context.SaveChanges();
                    status = true;
                }
            }
            catch (DbUpdateException)
            {
                status = false;
            }
            catch (InvalidOperationException)
            {
                status = false;
            }
            return status;
        }

        /// <summary>
        /// check if exist a friendship in the database
        /// </summary>
        /// <remarks>
        /// check if the idProfileFrom column exist the idProfileActualPlayer and in the idProfileTo column exist the idProfilePlayerFound, else the the idProfileFrom column exist the idProfilePlayerFound and in the idProfileTo column exist the idProfileActualPlayer
        /// </remarks>
        /// <param name = "idProfileActualPlayer"> receives an int with the id profile of the actual player </param>
        /// <param name = "idProfilePlayerFound"> receives an int with the id profile of the player want to send a friend request </param>
        /// <returns>boolean with true value if friendship exist</returns>
        public bool FriendshipExist(int idProfileActualPlayer, int idProfilePlayerFound)
        {
            bool existFriendship = false;
            using(OpenCodeChemsContext context = new OpenCodeChemsContext())
            {
                int existInIdProfileFrom = (from Friends in context.Friends where Friends.idProfileFrom.Equals(idProfileActualPlayer) && Friends.idProfileTo.Equals(idProfilePlayerFound) select Friends).Count();
                if (existInIdProfileFrom > 0)
                {
                    existFriendship = true;
                }
                int existInIdProfileTo = (from Friends in context.Friends where Friends.idProfileTo.Equals(idProfileActualPlayer) && Friends.idProfileFrom.Equals(idProfilePlayerFound) select Friends).Count();
                if (existInIdProfileTo > 0)
                {
                    existFriendship = true;
                }
            }
            return existFriendship;
        }

        /// <summary>
        /// gets the friends of the actual player
        /// </summary>
        /// <remarks>
        /// gets the id of the profiles with wich the actual player has a friendship and the status is equal to true, replaces the id for the nicknames of the profiles
        /// </remarks>
        /// <param name = "idProfile"> receives an int with de id profle of the actual player </param>
        /// <returns>List with the friends of the actual player</returns>
        /// <exception cref="InvalidOperationException">throw if idProfile is null</exception>
        public List<string> GetFriends(int idProfile)
        {
            List<string> friendsObtained = new List<string>();
            List<string> friendsIdFrom = new List<string>();
            List<string> friendsIdTo= new List<string>();
            try
            {
                using (OpenCodeChemsContext context = new OpenCodeChemsContext())
                {
                    friendsIdFrom = (from Friends in context.Friends join Profiles in context.Profile on Friends.idProfileTo equals Profiles.idProfile where Friends.idProfileFrom == idProfile where Friends.status.Equals(true) select Profiles.nickname).ToList();
                    friendsIdTo = (from Friends in context.Friends join Profiles in context.Profile on Friends.idProfileFrom equals Profiles.idProfile where Friends.idProfileTo == idProfile where Friends.status.Equals(true) select Profiles.nickname).ToList();
                }
                foreach(var friendIdFrom in friendsIdFrom)
                {
                    friendsObtained.Add(friendIdFrom);
                }
                foreach(var friendIdTo in friendsIdTo)
                {
                    friendsObtained.Add(friendIdTo);
                }
            }
            catch (InvalidOperationException)
            {
                friendsObtained = null;
            }
            return friendsObtained;
        }

        /// <summary>
        /// gets the friends requests of the actual player
        /// </summary>
        /// <remarks>
        /// gets the id of the profiles with wich the actual player has a friendship and the status is equal to false, replaces the id for the nicknames of the profiles
        /// </remarks>
        /// <param name = "idProfile"> receives an int with de id profle of the actual player </param>
        /// <returns>List with the friends requests of the actual player</returns>
        /// <exception cref="InvalidOperationException">throw if idProfile is null</exception>
        public List<string> GetFriendsRequests(int idProfile)
        {
            List<string> friendsRequests = null;
            try
            {
                using (OpenCodeChemsContext context = new OpenCodeChemsContext())
                {
                    friendsRequests = (from Friends in context.Friends join Profiles in context.Profile on Friends.idProfileFrom equals Profiles.idProfile where Friends.idProfileTo == idProfile where Friends.status.Equals(false) select Profiles.nickname).ToList();
                }
            }
            catch (InvalidOperationException)
            {
                friendsRequests = null;
            }
            return friendsRequests;
        }

        /// <summary>
        /// gets an object of type Profile  
        /// </summary>
        /// <remarks>
        /// evaluates if there is a Profile with the specified nickname, if so it gets the Profile
        /// </remarks>
        /// <param name = "nickname"> receives an string with the nickname of the user </param>
        /// <returns> object of type Profile </returns>
        /// <exception cref="DbUpdateException">throw if lost connection with the database</exception>
        /// <exception cref="InvalidOperationException">throw if the nickname is null</exception>
        public Profile GetProfileByNickname(string nickname)
		{
            Profile profileObteined = null;
			try
			{
				using(OpenCodeChemsContext context = new OpenCodeChemsContext())
				{
					profileObteined = (from Profile in context.Profile where Profile.nickname == nickname select Profile).First();
					context.SaveChanges();
				}
			}
			catch (DbUpdateException)
			{
                profileObteined = null;
			}
            catch (InvalidOperationException)
            {
                profileObteined = null;
            }
            return profileObteined;
		}

        /// <summary>
        /// dalete a friendship in database 
        /// </summary>
        /// <remarks>
        /// delete a register of Friends table if exist a register with the idProfileFrom in the idProfileFrom column and idProfileTo in the idProfileTo column, else exist a register with the idProfileTo in the idProfileFrom column and idProfileTo in the idProfileFrom column and the status is equal to true;
        /// </remarks>
        /// <param name = "friendsForDelete"> receives an object of type Friends </param>
        /// <returns>boolean with true value if could delete the friends registerd</returns>
        /// <exception cref="DbUpdateException">throw if lost connection with the database</exception>
        /// <exception cref="InvalidOperationException">throw if object type friends is null</exception>
        public bool DeleteFriend(Friends friendsForDelete)
        {
            bool status = false;
            int idProfileActualPlayer = friendsForDelete.idProfileFrom;
            int idProfileFriend = friendsForDelete.idProfileTo;
            try
            {
                using(OpenCodeChemsContext context = new OpenCodeChemsContext())
                {
                    var friendDeleteIdFrom = (from Friends in context.Friends where Friends.idProfileFrom == idProfileActualPlayer && Friends.idProfileTo == idProfileFriend && Friends.status.Equals(true) select Friends).First();
                    context.Friends.Remove(friendDeleteIdFrom);
                    context.SaveChanges();
                    status = true;

                }
            }
            catch (DbUpdateException)
            {
                status = false;
            }
            catch (InvalidOperationException)
            {
                status = false;
            }
            return status;
        }

        /// <summary>
        /// dalete a user that played as invitated
        /// </summary>
        /// <param name = "username"> receives a string with de username of the player </param>
        /// <returns>boolean with true value if could delete the user registerd</returns>
        /// <exception cref="DbUpdateException">throw if lost connection with the database</exception>
        /// <exception cref="InvalidOperationException">throw if username is null</exception>
        public bool DeleteInvitatedPlayer(string username)
        {
            bool status = false;
            try{
                using (OpenCodeChemsContext context = new OpenCodeChemsContext())
                {
                    var invitatedPlayer = (from Users in context.User where Users.username == username select Users).First();
                    context.User.Remove(invitatedPlayer);
                    context.SaveChanges();
                    status = true;
                }
            }
            catch (DbUpdateException)
            {
                status = false;
            }
            catch (InvalidOperationException)
            {
                status = false;
            }
            return status;
        }

        /// <summary>
        /// add a victory to the player with de nickname provided 
        /// </summary>
        /// <param name = "nickname"> receives a string with the nickname of the user </param>
        /// <returns>boolean with true value if could update the victories of the user</returns>
        /// <exception cref="DbUpdateException">throw if lost connection with the database</exception>
        /// <exception cref="InvalidOperationException">throw nickname is null</exception>
        public bool AddVictory(string nickname)
        {
            bool status;
            try
            {
                using (OpenCodeChemsContext context = new OpenCodeChemsContext())
                {   
                    var profiles = (from Profile in context.Profile where Profile.nickname == nickname select Profile).First();
                    profiles.victories = profiles.victories + 1;
                    context.SaveChanges();
                    status = true;
                }
            }
            catch (DbUpdateException)
            {
                status = false;
            }
            catch(InvalidOperationException)
            {
                status = false;
            }
            return status;
        }

        /// <summary>
        /// add a defeat to the player with de nickname provided 
        /// </summary>
        /// <param name = "nickname"> receives a string with the nickname of the user </param>
        /// <returns>boolean with true value if could update the defeats of the user</returns>
        /// <exception cref="DbUpdateException">throw if lost connection with the database</exception>
        /// <exception cref="InvalidOperationException">throw nickname is null</exception>
        public bool AddDefeat(string nickname)
        {
            bool status;
            try
            {
                using (OpenCodeChemsContext context = new OpenCodeChemsContext())
                {   
                    var profiles = (from Profile in context.Profile where Profile.nickname == nickname select Profile).First();
                    profiles.defeats = profiles.defeats + 1;
                    context.SaveChanges();
                    status = true;
                }
            }
            catch (DbUpdateException)
            {
                status = false;
            }
            catch(InvalidOperationException)
            {
                status = false;
            }
            return status;
        }

        /// <summary>
        /// assigns a new password to the user it finds with the provided email
        /// </summary>
        /// <remarks>
        /// evaluates if there is a user with the specified email, if so it changes its password
        /// </remarks>
        /// <param name = "email"> receives an string with the email of the user </param>
        /// <param name = "newHashedPassword"> receives an string with the new password of the user </param>
        /// <returns> boolean with true value if it could update the new password </returns>
        /// <exception cref="DbUpdateException">throw if lost connection with the database</exception>
        /// <exception cref="InvalidOperationException">throw if the username or newHashedPassword is null</exception>
        public bool RestorePassword(string email, string newHashedPassword)
        {
            bool status = false;
            try
            {
                using(OpenCodeChemsContext context = new OpenCodeChemsContext())
                {
                    var users = (from User in context.User where User.email == email  select User).First();
                    users.password = newHashedPassword;
                    context.SaveChanges();
                    status = true;                           
                }
            }
            catch (DbUpdateException)
            {
                status = false;
            }
            catch(InvalidOperationException)
            {
                status = false;
            }
            return status;
        }

        /// <summary>
        /// check if exist a register in Friends the object provided
        /// </summary>
        /// <param name = "friends"> receives an object of type Friends for search the form of friendship </param>
        /// <returns>boolean with true value if exist a register with the friends</returns>
        public bool SearchFriends(Friends friends)
        {
            bool status = false;
            int idProfileActualPlayer = friends.idProfileFrom;
            int idProfileFriend = friends.idProfileTo;
            using(OpenCodeChemsContext context = new OpenCodeChemsContext())
            {
                int friendshipExist = (from Friends in context.Friends where Friends.idProfileFrom == idProfileActualPlayer && Friends.idProfileTo == idProfileFriend && Friends.status.Equals(true) select Friends).Count();
                if(friendshipExist > 0)
                {
                    status = true;
                }
            }
            return status;
        }
        
    }
}
