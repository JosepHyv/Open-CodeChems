using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;


namespace OpenCodeChems.DataAccess
{
	public class OpenCodeChemsContext: DbContext
	{
		

		public DbSet<User> User { get; set; }
		public DbSet<Profile> Profile { get; set; }
        public DbSet<Friends> Friends { get; set; }

		

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseSqlServer("Server=tcp:openchemsserver.database.windows.net,1433;Initial Catalog=opencodechems;Persist Security Info=False;User ID=chemsito;Password=MasterKey$123;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=True;Connection Timeout=30");   
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
        public Profile (int idProfile, string nickname, int victories, int defeats, byte[] imageProfile, string username)
        {
            this.idProfile = idProfile;
            this.nickname = nickname;
            this.victories = victories;
            this.defeats = defeats;
            this.imageProfile = imageProfile;
            this.username = username;
        }

        [Key]
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
        [Key]
        public int idProfileFrom { get; set; }
        public int idProfileTo { get; set; }
        public bool status { get; set; }
    }

}
