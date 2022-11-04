using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace OpenCodeChems.DataAccess
{
    public class OpenCodeChemsContext:DbContext
    {
        public DbSet<User> User { get; set; }
        public DbSet <Profile> Profile { get; set; }
        protected override void OnConfiguring (DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring (optionsBuilder);
            optionsBuilder.UseSqlServer("Server=tcp:opencodechems.database.windows.net,1433;Initial Catalog=OpenCodeChems;" +
                "Persist Security Info=False;User ID=administrador;Password=MasterKey$123;MultipleActiveResultSets=False;" +
                "Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
        }
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
}
