using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using OpenCodeChems.Server.DataAccess;
using OpenCodeChems.Server.BusinessLogic.OpenCodeChems.Server.Contracts;

namespace OpenCodeChems.Server.BusinessLogic
{
    internal class UserManagement : IUserManagement
    {
        public string ComputeSHA256Hash(string text)
        {
            using (var sha256 = new SHA256Managed())
            {
                return BitConverter.ToString(sha256.ComputeHash(Encoding.UTF8.GetBytes(text))).Replace("-", "");
            }
        }

        public bool RegisterUser(User user, string nickname)
        {
            bool status = false;
            using (OpenCodeChemsContext context = new OpenCodeChemsContext())
            {
                User newUser = context.Users.Add(new User() { username = user.username, email = user.email, name = user.name, password = ComputeSHA256Hash(user.password) });
                context.SaveChanges();
                Profile newProfile = context.Profiles.Add(new Profile() { nickname = nickname, username = user.username });
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
            string passwordHashed = ComputeSHA256Hash(password);
            using (OpenCodeChemsContext context = new OpenCodeChemsContext())
            {
                int foundUser = (from User in context.Users
                                 where User.username == username && User.password == passwordHashed
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

