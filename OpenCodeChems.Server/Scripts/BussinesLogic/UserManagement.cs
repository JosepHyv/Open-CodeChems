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
    public class UserManagement : IUserManagement
    {
        

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
            }
            return status;
        }
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
			}
			return status;
		}



        public bool Login(string username, string password) 
        {
            
            bool status = false;
            using (OpenCodeChemsContext context = new OpenCodeChemsContext())
            {
                int foundUser = (from User in context.User where User.username == username && User.password == password   select User).Count();
                if (foundUser > 0)
                {
                    status = true;
                }
            }
            return status;
        }

        public bool EditProfileNickname(string username, string newNickname)
        {
            bool status = false;
            try
            {
                using (OpenCodeChemsContext context = new OpenCodeChemsContext())
                {   
                    var profiles = (from Profile in context.Profile where Profile.username == username select Profile).First();
                    profiles.nickname = newNickname;
                    context.SaveChanges();
                    status = true;
                }
            }
            catch (DbUpdateException)
            {
                status = false;
            }
            return status;
        }

        public bool EditProfileImage(string username, byte[] imageProfile)
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
            return status;
        }

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
            return status;
        }

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
			}
			return status;
		} 
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
            return status;
        } 
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
            return status;
        }
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
        public List<string> GetFriends(int idProfile, bool status)
        {
            List<string> friendsObtained = new List<string>();
            List<string> friendsIdFrom = new List<string>();
            List<string> friendsIdTo= new List<string>();
            using (OpenCodeChemsContext context = new OpenCodeChemsContext())
            {
                friendsIdFrom = (from Friends in context.Friends join Profiles in context.Profile on Friends.idProfileTo equals Profiles.idProfile where Friends.idProfileFrom == idProfile where Friends.status == status select Profiles.nickname).ToList();
                friendsIdTo = (from Friends in context.Friends join Profiles in context.Profile on Friends.idProfileFrom equals Profiles.idProfile where Friends.idProfileTo == idProfile where Friends.status == status select Profiles.nickname).ToList();
            }
            foreach(var friendIdFrom in friendsIdFrom)
            {
                friendsObtained.Add(friendIdFrom);
            }
            foreach(var friendIdTo in friendsIdTo)
            {
                friendsObtained.Add(friendIdTo);
            }
            return friendsObtained;
        }

        public List<string> GetFriendsRequests(int idProfile, bool status)
        {
            List<string> friendsRequests = new List<string>();
            using (OpenCodeChemsContext context = new OpenCodeChemsContext())
            {
                friendsRequests = (from Friends in context.Friends join Profiles in context.Profile on Friends.idProfileFrom equals Profiles.idProfile where Friends.idProfileTo == idProfile where Friends.status == status select Profiles.nickname).ToList();
            }
            return friendsRequests;
        }
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
    }
}