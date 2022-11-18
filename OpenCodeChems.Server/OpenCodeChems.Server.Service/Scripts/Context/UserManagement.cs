using System;
using System.Linq;
using System.Data.Entity;
using System.Collections;
using System.Collections.Generic;
using OpenCodeChems.Server.Models;

namespace OpenCodeChems.Server.Context
{
    public class UserManagement
    {
        public bool UserExists(User currentUser)
            {
                bool status = false;
                using (OpenCodeChemsContext context = new OpenCodeChemsContext())
                {
                    int foundUser = (from User in context.Users
                                    where User.username == currentUser.username || User.email == currentUser.email
                                    select User).Count();
                    status = foundUser > 0;
                }
                return status;
            }
            /*public bool RegisterUser(User user, string nickname)
            {
                bool status = false;
                if (!UserExists(user))
                {
                    using (OpenCodeChemsContext context = new OpenCodeChemsContext())
                    {
                        EntityEntry<User> newUser = context.Users.Add(new User(user.username, user.password, user.name, user.email));
                        context.SaveChanges();
                        EntityEntry<Profile> newProfile = context.Profiles.Add(new Profiles(nickname, 0, null, 0, user.username));
                        context.SaveChanges();
                        if (newUser != null && newProfile != null)
                        {
                            status = true;
                        }
                    }
                }
                return status;
            }*/
            public bool RegisterUser(User user)
            {
                bool status = false;
                int afectedColumns = 0;
                SqlConnection connectionDataBase = new SqlConnection("connectionString");
                connectionDataBase.Open();
                try
                {
                    if (!UserExists(user))
                    {
                        try
                        {

                            SqlCommand sqlCommand = new SqlCommand("INSERT INTO dbo.User (username, password, email, name) VALUES (@username, @password, @email, @name)");
                            sqlCommand.Parameters.AddWithValue("@username", user.username);
                            sqlCommand.Parameters.AddWithValue("@password", user.password);
                            sqlCommand.Parameters.AddWithValue("@email", user.email);
                            sqlCommand.Parameters.AddWithValue("@name", user.name);
                            afectedColumns = sqlCommand.ExecuteNonQuery();
                        }
                        catch (SqlException messageException)
                        {
                            Exception errorRegisterMessage = new Exception("ERROR_REGISTER_USER");
                            throw errorRegisterMessage;
                        }

                    }
                    else
                    {
                        Exception userExistMessage = new Exception("ERROR_EXIST_USER");
                        throw userExistMessage;
                    }

                    if (afectedColumns > 0)
                    {
                        status = true;
                    }
                }
                finally
                {
                    connectionDataBase.Close();
                }
                return status;
                
            }

    }


}
