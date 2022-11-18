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

         public bool RegisterUser(User user, string nickname)
        {

            optionsBuilder.UseSqlServer(connectionstring);
            
            bool status = false;
            using (OpenCodeChemsContext context = new OpenCodeChemsContext(optionsBuilder.Options))
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

    }

}
