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
	/// <summary>
	/// Specifies the objects to be connected in the database
	/// </summary>
	public class OpenCodeChemsContext: DbContext
	{
		
		/// <summary>
		/// Gets and sets of the object type User
		/// </summary>
		public DbSet<User> User { get; set; }

		/// <summary>
		/// Gets and sets of the object type Profile
		/// </summary>
		public DbSet<Profile> Profile { get; set; }

		/// <summary>
		/// Gets and sets of the object type Friends
		/// </summary>
		public DbSet<Friends> Friends { get; set; }

		
		/// <summary>
		/// configure the conection with database
		/// </summary>
		/// <param name="optionsBuilder"> connect with the database </param>
		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseSqlServer("Server=tcp:openchemsserver.database.windows.net,1433;Initial Catalog=opencodechems;Persist Security Info=False;User ID=chemsito;Password=MasterKey$123;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=True;Connection Timeout=30");   
		}
		
	}
	/// <summary>
	/// class of the object type User
	/// </summary>
	public class User
	{
		/// <summary>
		/// create the constructor of the object type User
		/// </summary>
		/// <param name="username"> username of the User </param>
		/// <param name="password"> encrypted password of the User </param>
		/// <param name="name"> name of the User </param>
		/// <param name="email"> email for the User</param>

		public User(string username, string password, string name, string email)
		{
			this.username = username;
			this.password = password;
			this.name = name;
			this.email = email;
		}

		/// <summary>
		/// Gets and sets of the username parameter
		/// </summary>
		/// <value>string</value>
		[Key]
		public string username { get; set; }
		/// <summary>
		/// Gets and sets of the password parameter
		/// </summary>
		/// <value>string</value>
		public string password { get; set; }
		/// <summary>
		/// Gets and sets of the name parameters
		/// </summary>
		/// <value> string </value>
		public string name { get; set; }
		/// <summary>
		/// Gets and sets of the name parameters
		/// </summary>
		/// <value> string </value>
		public string email { get; set; }

	}

	/// <summary>
	/// class of the object type Profile
	/// </summary>
  	public class Profile 
  	{
		/// <summary>
		/// create the constructor of the object type User
		/// </summary>
		/// <param name="nickname"> nickname of the Profile of the user </param>
		/// <param name="victories"> number of victories of the Profile of the user </param>
		/// <param name="defeats"> number of defeats of the Profile of the user </param>
		/// <param name="imageProfile"> number of the image profile of the Profile of the user </param>
		/// <param name="username"></param>
      	public Profile (string nickname, int victories, int defeats, int imageProfile, string username)
      	{
          this.nickname = nickname;
          this.victories = victories;
          this.defeats = defeats;
          this.imageProfile = imageProfile;
          this.username = username;
      	}


		/// <summary>
		/// Gets and sets of the idProfile
		/// </summary>
		/// <value>int</value>
		[Key]
		public int idProfile { get; set; }
		/// <summary>
		/// Gets and sets of the nickname
		/// </summary>
		/// <value>string</value>
		public string nickname { get; set; }
		/// <summary>
		/// Gets and sets of the victories
		/// </summary>
		/// <value>int</value>
		public int victories { get; set; }
		/// <summary>
		/// Gets and sets of defeats
		/// </summary>
		/// <value>int</value>
		public int defeats { get; set; }
		/// <summary>
		/// Gets and sets of imageProfile
		/// </summary>
		/// <value>int</value>
		public int imageProfile { get; set; }

		/// <summary>
		/// Gets and sets of the username
		/// </summary>
		/// <value>string</value>
		public string username { get; set;  }
  	}
    
	/// <summary>
	/// class of the object type Friends
	/// </summary>
	public class Friends
	{
		/// <summary>
		/// create the constructorof the object tye Friends
		/// </summary>
		/// <param name="idProfileFrom">is the id of the profile that send the friend request</param>
		/// <param name="idProfileTo">is the id of the profile that recive the friend request</param>
		/// <param name="status"> is the status of the friendship</param>
		public Friends(int idProfileFrom, int idProfileTo, bool status)
		{
			this.idProfileFrom = idProfileFrom;
			this.idProfileTo = idProfileTo;
			this.status = status;
		}
		/// <summary>
		/// Gets and sets idProfileFrom
		/// </summary>
		/// <value>int</value>
		[Key]
		public int idProfileFrom { get; set; }
		/// <summary>
		/// Gets and sets of idProfileTo
		/// </summary>
		/// <value>int</value>
		public int idProfileTo { get; set; }
		/// <summary>
		/// Gets and sets of the status
		/// </summary>
		/// <value>bool</value>
		public bool status { get; set; }
	}

}
