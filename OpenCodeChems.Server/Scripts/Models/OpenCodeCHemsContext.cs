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
		

		/*public DbSet<User> User { get; set; }

		

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseSqlServer("Server=tcp:openchemsserver.database.windows.net,1433;Initial Catalog=opencodechems;Persist Security Info=False;User ID=chemsito;Password=MasterKey$123;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=True;Connection Timeout=30");   
		}
		
	}
	public class User
	{

		public User(string username, string password, string name, string email, int victories, int defeats, byte[] imageProfile)
		{
			this.username = username;
			this.password = password;
			this.name = name;
			this.email = email;
			this.victories = victories;
			this.defeats = defeats;
			this.imageProfile = imageProfile;
		}

		[Key]
		public string username { get; set; }
		public string password { get; set; }
		public string name { get; set; }
		public string email { get; set; }
		public int victories { get; set; }
		public int defeats { get; set; }
		public byte[] imageProfile { get; set; }

	*/}

}
