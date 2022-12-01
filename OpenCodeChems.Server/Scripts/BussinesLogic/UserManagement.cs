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
        public bool EditUserEmail(string username, string email)
        {
            bool status = false;
            try
            {
                using (OpenCodeChemsContext context = new OpenCodeChemsContext())
                {   
                    var users = (from User in context.User where User.username == username select User).First();
                    users.email = email;
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

        public Profile GetProfile(string username)
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
        public bool AcceptFriendRequest(Friends friends)
        {
            bool status = false;
            try
            {
                string nicknameFrom = friends.nicknameFrom;
                string nicknameTo = friends.nicknameTo;
                using (OpenCodeChemsContext context = new OpenCodeChemsContext())
                {   
                    var friendsUpdate = (from Friends in context.Friends where Friends.nicknameFrom == nicknameFrom && Friends.nicknameTo ==  nicknameTo select Friends).First();
                    friendsUpdate.state = friends.state;
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
        public bool DenyFriendRequest(Friends friends)
        {
            bool status = false;
            try{
                using (OpenCodeChemsContext context = new OpenCodeChemsContext())
                {
                    string nicknameFrom = friends.nicknameFrom;
                    string nicknameTo = friends.nicknameTo;
                    var friendsDelete = (from Friends in context.Friends where friends.nicknameFrom == nicknameFrom && friends.nicknameTo == nicknameTo select friends).First();
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
    }
}