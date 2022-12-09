using System;
using System.Text;
using System.Security.Cryptography;

namespace OpenCodeChems.Client.Resources
{
    public class Objects
    {
        public class User
        {

            public User(string username, string password, string name, string email)
            {
                this.username = username;
                this.password = password;
                this.name = name;
                this.email = email;
            }
            public string username { get; set; }
            public string password { get; set; }
            public string name { get; set; }
            public string email { get; set; }

        }

        public class Profile 
            {
                public Profile (int idProfile, string nickname, int victories, int defeats, byte[] imageProfile, string username)
                {
                    this.idProfile = idProfile;
                    this.nickname = nickname;
                    this.victories = victories;
                    this.defeats = defeats;
                    this.imageProfile = imageProfile;
                    this.username = username;
                }
                public int idProfile { get; set; }
                public string nickname { get; set; }
                public int victories { get; set; }
                public int defeats { get; set; }
                public byte[] imageProfile { get; set; }
                public string username { get; set;  }
        }
        public class Friends
        {
            public Friends(int idProfileFrom, int idProfileTo, bool status)
            {
                this.idProfileFrom = idProfileFrom;
                this.idProfileTo = idProfileTo;
                this.status = status;
            }
            public int idProfileFrom { get; set; }
            public int idProfileTo { get; set; }
            public bool status { get; set; }
        }
    }
}