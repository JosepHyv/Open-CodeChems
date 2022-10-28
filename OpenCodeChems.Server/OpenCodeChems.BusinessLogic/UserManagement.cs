using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using OpenCodeChems.BusinessLogic.Interface;
using OpenCodeChems.DataAccess;

namespace OpenCodeChems.BusinessLogic
{
    public class UserManagement : IUserManagement
    {
        public bool RegisterUser(User user, string nickname)
        {
            bool status = false;
            using (OpenCodeChemsContext context = new OpenCodeChemsContext())
            {
                EntityEntry<User> newUser = context.User.Add(new User (user.username, user.password, user.name, user.email));
                context.SaveChanges();
                EntityEntry<Profile> newProfile = context.Profile.Add(new Profile (nickname, 0, null, 0, user.username));
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
    }
}
