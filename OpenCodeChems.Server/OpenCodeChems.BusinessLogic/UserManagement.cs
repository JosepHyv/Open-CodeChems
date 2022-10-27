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
    internal class UserManagement : IUserManagement
    {
        public bool RegisterUser(User user, string nikname)
        {
            bool status = false;
            using (OpenCodeChemsContext context = new OpenCodeChemsContext())
            {
               EntityEntry<User> newUser = context.User.Add(new User
               {
                   username = user.username, email = user.email, name = user.name, password = user.password
               });
                context.SaveChanges();
                EntityEntry<Profile> newProfile = context.Profile.Add(new Profile { nickname = user.name, username = user.username });
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
