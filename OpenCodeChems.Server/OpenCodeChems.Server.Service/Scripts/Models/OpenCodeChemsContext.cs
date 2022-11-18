using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace OpenCodeChems.Server.Models
{
    public class OpenCodeChemsContext: DbContext
    {
        public OpenCodeChemsContext() : base("OpenCodeChemsContext")
        {
        }
        public DbSet<User> User { get; set; }
        public DbSet <Profile> Profile { get; set; }
    }
    public class Profile
    {
         public Profile(string nickname, int victories, byte[] imageProfile, int defaults, string username)
        {
            this.nickname = nickname;
            this.victories = victories;
            this.imageProfile = imageProfile;
            this.defaults = defaults;
            this.username = username;
        }
        [Key]
        public string nickname { get; set; }
        public int victories { get; set; }
        public byte[] imageProfile { get; set; }
        public int defaults { get; set; }
        public string username { get; set; }
    }

    public class User
    {
         public User(string username, string password, string name, string email)
        {
            this.username = username;
            this.password = password;
            this.name = name;
            this.email = email;
        }
        [Key]
        public string username { get; set; }
        public string password { get; set; }
        public string name { get; set; }
        public string email { get; set; }
    }

}
