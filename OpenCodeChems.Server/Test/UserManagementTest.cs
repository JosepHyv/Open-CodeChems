using Godot;
using System;
using OpenCodeChems.BussinesLogic;
using OpenCodeChems.DataAccess;
using OpenCodeChems.Server.Utils;
using System.IO;

namespace OpenCodeChems.Test
{

	[Title("UserManagement Tests")]
	public class UserManagementTest : WAT.Test
	{
		private UserManagement USER_MANAGEMENT = new UserManagement();
		private static Encryption PasswordHasher = new Encryption();
		private string USERNAME = "UsernameTest";
		private string PASSWORD = PasswordHasher.ComputeSHA256Hash("Passw0rd!");
		private string NAME = "Carlos Test";
		private string EMAIL = "test@gmail.com";
		private string NICKNAME = "NicknameTest";
		private int VICTORIES = 0;
		private int DEFEATS = 0;
		private byte[] IMAGE_PROFILE = null;
		
		[Test]
		public void RegisterUserCorrect()
		{
			User newUser = new User(USERNAME, PASSWORD, NAME, EMAIL);
			bool expectedStatus = true;
			bool obtainedStatus = USER_MANAGEMENT.RegisterUser(newUser);
			Assert.IsEqual(expectedStatus, obtainedStatus);
		}
		[Test]
		public void RegisterUserInvalidOperationException()
		{
			User newUser = new User(null, null, null, null);
			bool expectedStatus = false;
			bool obtainedStatus = USER_MANAGEMENT.RegisterUser(newUser);
			Assert.IsEqual(expectedStatus, obtainedStatus);
		}
		[Test]
		public void RegisterUserDuplicated()
		{
			User newUser = new User(USERNAME, PASSWORD, NAME, EMAIL);
			bool expectedStatus = false;
			bool obtainedStatus = USER_MANAGEMENT.RegisterUser(newUser);
			Assert.IsEqual(expectedStatus, obtainedStatus);
		}
		[Test]
		public void RegisterProfileCorrect()
		{
			Profile newProfile = new Profile(0, NICKNAME, VICTORIES, DEFEATS, IMAGE_PROFILE, USERNAME);
			bool expectedStatus = true;
			bool obtainedStatus = USER_MANAGEMENT.RegisterProfile(newProfile);
			Assert.IsEqual(expectedStatus, obtainedStatus);
		}
		[Test]
		public void RegisterProfileInvalidOperationException()
		{
			Profile newProfile = new Profile(-123, null, -123, -123, null, null);
			bool expectedStatus = false;
			bool obtainedStatus = USER_MANAGEMENT.RegisterProfile(newProfile);
			Assert.IsEqual(expectedStatus, obtainedStatus);
		}
		[Test]
		public void LoginCorrect()
		{
			bool expectedStatus = true;
			bool obtainedStatus = USER_MANAGEMENT.Login(USERNAME, PASSWORD);
			Assert.IsEqual(expectedStatus, obtainedStatus);
		}
		[Test]
		public void LoginNotCorrect()
		{
			bool expectedStatus = false;
			bool obtainedStatus = USER_MANAGEMENT.Login("test", "passwordTest");
			Assert.IsEqual(expectedStatus, obtainedStatus);
		}
		[Test]
		public void LoginNullUsernameAndPassword()
		{
			bool expectedStatus = false;
			bool obtainedStatus = USER_MANAGEMENT.Login(null, null);
			Assert.IsEqual(expectedStatus, obtainedStatus);
		}
		[Test]
		public void EditProfileNicknameCorrect()
		{
			string newNickname = "NicknameUpdateTest";
			bool expectedStatus = true;
			bool obtainedStatus = USER_MANAGEMENT.EditProfileNickname(USERNAME, newNickname);
			Assert.IsEqual(expectedStatus, obtainedStatus);
		}
		[Test]
		public void EditProfileNicknameInvalidOperationException()
		{
			bool expectedStatus = false;
			bool obtainedStatus = USER_MANAGEMENT.EditProfileNickname(null, null);
			Assert.IsEqual(expectedStatus, obtainedStatus);
		}
		[Test]
		public void EditProfileImageCorrect()
		{   
			var directory = new Godot.Directory();
			FileStream imageProfileFileStream = new FileStream("Test/imagePerfilDefault.jpg", FileMode.OpenOrCreate, FileAccess.ReadWrite);
			Byte[] imageProfile = new Byte[imageProfileFileStream.Length];
			BinaryReader readearToBinary = new BinaryReader(imageProfileFileStream);
			imageProfile = readearToBinary.ReadBytes(Convert.ToInt32(imageProfileFileStream.Length));
			imageProfileFileStream.Close(); 
			bool expectedStatus = true;
			bool obtainedStatus = USER_MANAGEMENT.EditProfileImage(USERNAME, imageProfile);
			Assert.IsEqual(expectedStatus, obtainedStatus);
		}
		[Test]
		public void EditProfileImageInvalidOperationException()
		{
			bool expectedStatus = false;
			bool obtainedStatus = USER_MANAGEMENT.EditProfileImage(null, null);
			Assert.IsEqual(expectedStatus, obtainedStatus);
		}
	}
}
