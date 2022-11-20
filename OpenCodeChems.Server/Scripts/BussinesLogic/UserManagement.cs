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
        private string connectionstring = "Connection string";

        private DbContextOptionsBuilder<OpenCodeChemsContext> optionsBuilder = new DbContextOptionsBuilder<OpenCodeChemsContext>();
        /*optionsBuilder.UseSqlServer(connectionstring);


        OpenCodeChemsContext dbContext = new OpenCodeChemsContext(optionsBuilder.Options);*/

        public bool RegisterUser(User user, Profile profile)
        {

            optionsBuilder.UseSqlServer(connectionstring);
            
            bool status = false;
            using (OpenCodeChemsContext context = new OpenCodeChemsContext(optionsBuilder.Options))
            {
                EntityEntry<User> newUser = context.User.Add(new User (user.username, user.password, user.name, user.email));
                context.SaveChanges();
                EntityEntry<Profile> newProfile = context.Profile.Add(new Profile (profile.nickname, profile.victories, profile.imageProfile, profile.defaults, user.username));
                context.SaveChanges();
                if (newUser != null && newProfile != null)
                {
                    status = true;
                }
            }
            return status;
        }

        public bool Login(string username, string password)
        {
            optionsBuilder.UseSqlServer(connectionstring);
            bool status = false;
            using (OpenCodeChemsContext context = new OpenCodeChemsContext(optionsBuilder.Options))
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
                optionsBuilder.UseSqlServer(connectionstring);
                using (OpenCodeChemsContext context = new OpenCodeChemsContext(optionsBuilder.Options))
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
                optionsBuilder.UseSqlServer(connectionstring);
                using (OpenCodeChemsContext context = new OpenCodeChemsContext(optionsBuilder.Options))
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
                optionsBuilder.UseSqlServer(connectionstring);
                using (OpenCodeChemsContext context = new OpenCodeChemsContext(optionsBuilder.Options))
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
        public string getOldPassword(User user)
        {
            string username = user.username;
            string oldHashedPassword;
            optionsBuilder.UseSqlServer(connectionstring);
            using(OpenCodeChemsContext context = new OpenCodeChemsContext(optionsBuilder.Options))
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
                string oldHashedPassword = getOldPassword(user);
                optionsBuilder.UseSqlServer(connectionstring);
                using(OpenCodeChemsContext context = new OpenCodeChemsContext(optionsBuilder.Options))
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