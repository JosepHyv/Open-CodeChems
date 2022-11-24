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

        public bool EditProfileNickname(Profile profile, string newNickname)
        {
            bool status = false;
            try
            {
                string username = profile.username;
                using (OpenCodeChemsContext context = new OpenCodeChemsContext())
                {   
                    var profiles = (from Profile in context.Profile where Profile.username == username select Profile).First();
                    profiles.username = newNickname;
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

        public bool EditProfileImage(Profile profile, byte[] imageProfile)
        {
            bool status = false;
            try
            {
                string username = profile.username;
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
        public bool EditUserEmail(User user, string email)
        {
            bool status = false;
            try
            {
                string username = user.username;
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
        public string GetOldPassword(User user)
        {
            string username = user.username;
            string oldHashedPassword;
            using(OpenCodeChemsContext context = new OpenCodeChemsContext())
            {
                oldHashedPassword = (from User in context.User where User.username == username select User.password).First();
                
            }
            return oldHashedPassword;
        }
        public bool EditUserPassword(User user, string actualHashedPassword, string newHashedPassword)
        {
            bool status = false;
            try
            {
                string username = user.username;
                string oldHashedPassword = GetOldPassword(user);
                using(OpenCodeChemsContext context = new OpenCodeChemsContext())
                {
                    if (oldHashedPassword == actualHashedPassword)
                    {
                        var users = (from User in context.User where User.username == username  select User).First();
                        users.password = newHashedPassword;
                        context.SaveChanges();
                        status = true;
                    }
                    
                                                
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
			try
			{
				using(OpenCodeChemsContext context = new OpenCodeChemsContext())
				{
					Profile profileObteined = (from Profile in context.Profile where Profile.username == username select Profile).First();
					context.SaveChanges();
					return profileObteined;
				}
			}
			catch (DbUpdateException)
			{
				Exception exception = new Exception("PROFILE_NOT_FOUND");
				throw exception;
			}
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
    }
}