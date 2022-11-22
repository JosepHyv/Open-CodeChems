using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenCodeChems.DataAccess;
using OpenCodeChems.BusinessLogic.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;



namespace OpenCodeChems.BussinesLogic
{
    public class UserManagement : IUserManagement
    {
        

        public bool RegisterUser(User user, Profile profile)
        {  
            bool status = false;
            using (OpenCodeChemsContext context = new OpenCodeChemsContext())
            {
                
                try
                {
                    context.User.Add(user);
                    context.SaveChanges();
                    context.Profile.Add(profile);
                    context.SaveChanges();
                }
                catch (DbUpdateException)
                {
                    status = false;
                }
                catch (NullReferenceException)
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
                int foundUser = (from User in context.User
                    where User.username == username && User.password == password
                    select User).Count();
                if (foundUser > 0)
                {
                    status = true;
                }
            }
            return status;
        }

        public bool EditProfileNickname(Profile profile, string nickname)
        {
            bool status = false;
            try
            {
                string username = profile.username;
                using (OpenCodeChemsContext context = new OpenCodeChemsContext())
                {   
                    var profiles = (from Profile in context.Profile
                                    where Profile.username == username
                                    select Profile).First();
                    profiles.nickname = nickname;
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
                    var profiles = (from Profile in context.Profile
                                    where Profile.username == username
                                    select Profile).First();
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
                    var users = (from User in context.User
                                    where User.username == username
                                    select User).First();
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
                oldHashedPassword = (from User in context.User
                                            where User.username == username
                                            select User.password).First();
                
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
                        var users = (from User in context.User
                                    where User.username == username
                                    select User).First();
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

    }
}