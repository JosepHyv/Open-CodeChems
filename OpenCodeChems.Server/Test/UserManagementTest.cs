using Godot;
using System;
using OpenCodeChems.BussinesLogic;
using OpenCodeChems.DataAccess;
using OpenCodeChems.Server.Utils;
using System.IO;
using System.Collections.Generic;

namespace OpenCodeChems.Test
{

	[Title("UserManagement Tests")]
	public class UserManagementTest : WAT.Test
	{
		private readonly UserManagement USER_MANAGEMENT = new UserManagement();
		private static Encryption PasswordHasher = new Encryption();
		private const string USERNAME = "UsernameTest";
		private readonly string PASSWORD = PasswordHasher.ComputeSHA256Hash("Passw0rd!");
		private readonly string NEW_PASSWORD = PasswordHasher.ComputeSHA256Hash("Passw0rd!2");
		private const string NAME = "Carlos Test";
		private const string EMAIL = "test@gmail.com";
		private const string NICKNAME = "NicknameTest";
		private const string NEW_NICKNAME = "NicknameUpdateTest";
		private const int VICTORIES = 0;
		private const int DEFEATS = 0;
		private const int IMAGE_PROFILE = 0;
		private const int NEW_IMAGE_PROFILE = 1;
		private const int ID_PROFILE_CARSI12 = 2;
		private const int ID_PROFILE_ZINE = 3;
		
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
			Profile newProfile = new Profile( NICKNAME, VICTORIES, DEFEATS, IMAGE_PROFILE, USERNAME);
			bool expectedStatus = true;
			bool obtainedStatus = USER_MANAGEMENT.RegisterProfile(newProfile);
			Assert.IsEqual(expectedStatus, obtainedStatus);
		}
		[Test]
		public void RegisterProfileInvalidOperationException()
		{
			Profile newProfile = new Profile( null, -123, -123, -123, null);
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
		public void NicknameRegisteredCorrect()
		{
			bool expectedStatus = true;
			bool obtainedStatus = USER_MANAGEMENT.NicknameRegistered(NICKNAME);
			Assert.IsEqual(expectedStatus, obtainedStatus);
		}
		[Test]
		public void NicknameRegisteredNotCorrect()
		{
			bool expectedStatus = false;
			bool obtainedStatus = USER_MANAGEMENT.NicknameRegistered(NEW_NICKNAME);
			Assert.IsEqual(expectedStatus, obtainedStatus);
		}
		[Test]
		public void EditProfileNicknameCorrect()
		{
			bool expectedStatus = true;
			bool obtainedStatus = USER_MANAGEMENT.EditProfileNickname(USERNAME, NEW_NICKNAME);
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
			
			bool expectedStatus = true;
			bool obtainedStatus = USER_MANAGEMENT.EditProfileImage(USERNAME, NEW_IMAGE_PROFILE);
			Assert.IsEqual(expectedStatus, obtainedStatus);
		}
		[Test]
		public void EditProfileImageInvalidOperationException()
		{
			bool expectedStatus = false;
			bool obtainedStatus = USER_MANAGEMENT.EditProfileImage(null, -123);
			Assert.IsEqual(expectedStatus, obtainedStatus);
		}
		[Test]
		public void PasswordExistCorrect()
		{
			bool expectedStatus = true;
			bool obtainedStatus = USER_MANAGEMENT.PasswordExist(USERNAME, PASSWORD);
			Assert.IsEqual(expectedStatus, obtainedStatus);
		}
		[Test]
		public void PasswordExistNotCorrect()
		{
			bool expectedStatus = false;
			bool obtainedStatus = USER_MANAGEMENT.PasswordExist(USERNAME, NEW_PASSWORD);
			Assert.IsEqual(expectedStatus, obtainedStatus);
		}
		[Test]
		public void EditUserPasswordCorrect()
		{
			bool expectedStatus = true;
			bool obtainedStatus = USER_MANAGEMENT.EditUserPassword(USERNAME, NEW_PASSWORD);
			Assert.IsEqual(expectedStatus, obtainedStatus);
		}
		[Test]
		public void EditUserPasswordInvalidOperationException()
		{
			bool expectedStatus = false;
			bool obtainedStatus = USER_MANAGEMENT.EditUserPassword(null, null);
			Assert.IsEqual(expectedStatus, obtainedStatus);
		}
		[Test]
		public void GetProfileByUsernameCorrect()
		{
			Profile profileExpected = new Profile(NEW_NICKNAME, VICTORIES, DEFEATS, NEW_IMAGE_PROFILE, USERNAME);
			Profile profileObtained = USER_MANAGEMENT.GetProfileByUsername(USERNAME);
			profileExpected.idProfile = profileObtained.idProfile;
			bool statusExpected = false;
			bool statusObtained = true;
			if(profileObtained.nickname == profileExpected.nickname)
			{
				statusExpected = true;
			}
			Assert.IsEqual(statusExpected, statusObtained);
		}
		[Test]
		public void GetProfileByUsernameNotCorrect()
		{
			Profile profileExpected = new Profile(NEW_NICKNAME, VICTORIES, DEFEATS, NEW_IMAGE_PROFILE, USERNAME);
			bool statusExpected = false;
			bool statusObtained = false;
			try
			{
				Profile profileObtained = USER_MANAGEMENT.GetProfileByUsername("null");
				profileExpected.idProfile = profileObtained.idProfile;
				if(profileObtained.Equals(profileExpected))
				{
					statusExpected = true;
				}
			}
			catch(NullReferenceException)
			{
				statusExpected = false;
			}
			Assert.IsEqual(statusExpected, statusObtained);
		}
		[Test]
		public void GetProfileByNicknameCorrect()
		{
			Profile profileExpected = new Profile(NEW_NICKNAME, VICTORIES, DEFEATS, NEW_IMAGE_PROFILE, USERNAME);
			Profile profileObtained = USER_MANAGEMENT.GetProfileByNickname(NEW_NICKNAME);
			profileExpected.idProfile = profileObtained.idProfile;
			bool statusExpected = false;
			bool statusObtained = true;
			if(profileObtained.username == profileExpected.username)
			{
				statusExpected = true;
			}
			Assert.IsEqual(statusExpected, statusObtained);
		}
		[Test]
		public void GetProfileByNicknameNotCorrect()
		{
			Profile profileExpected = new Profile(NEW_NICKNAME, VICTORIES, DEFEATS, NEW_IMAGE_PROFILE, USERNAME);
			bool statusExpected = false;
			bool statusObtained = false;
			try
			{
				Profile profileObtained = USER_MANAGEMENT.GetProfileByNickname("null");
				profileExpected.idProfile = profileObtained.idProfile;
				if(profileObtained.Equals(profileExpected))
				{
					statusExpected = true;
				}
			}
			catch(NullReferenceException)
			{
				statusExpected = false;
			}
			Assert.IsEqual(statusExpected, statusObtained);
		}
		[Test]
		public void EmailRegisteredCorrect()
		{
			bool expectedStatus = true;
			bool obtainedStatus = USER_MANAGEMENT.EmailRegistered(EMAIL);
			Assert.IsEqual(expectedStatus, obtainedStatus);
		}
		[Test]
		public void EmailRegisteredNotCorrect()
		{
			bool expectedStatus = false;
			string incorrectEmail = "test@hotmail.com";
			bool obtainedStatus = USER_MANAGEMENT.EmailRegistered(incorrectEmail);
			Assert.IsEqual(expectedStatus, obtainedStatus);
		}
		[Test]
		public void UsernameRegisteredCorrect()
		{
			bool expectedStatus = true;
			bool obtainedStatus = USER_MANAGEMENT.UsernameRegistered(USERNAME);
			Assert.IsEqual(expectedStatus, obtainedStatus);
		}
		[Test]
		public void UsernameRegisteredNotCorrect()
		{
			bool expectedStatus = false;
			string incorrectUsername = "usernameTest2";
			bool obtainedStatus = USER_MANAGEMENT.UsernameRegistered(incorrectUsername);
			Assert.IsEqual(expectedStatus, obtainedStatus);
		}
		[Test]
		public void AddFriendCorrect()
		{
			Profile profileTest = USER_MANAGEMENT.GetProfileByUsername(USERNAME);
			Friends friendsTest = new Friends(profileTest.idProfile, ID_PROFILE_CARSI12, false);
			bool expectedStatus = true;
			bool obtainedStatus = USER_MANAGEMENT.AddFriend(friendsTest);
			Assert.IsEqual(expectedStatus, obtainedStatus);
		}
		[Test]
		public void AddFriendNotCorrect()
		{
			Friends friendsTest = new Friends(-123, -1234, false);
			bool expectedStatus = false;
			bool obtainedStatus = USER_MANAGEMENT.AddFriend(friendsTest);
			Assert.IsEqual(expectedStatus, obtainedStatus);
		}
		[Test]
		public void GetFriendsRequestCorrect()
		{
			List<string> obtainedFriendsRequests = USER_MANAGEMENT.GetFriendsRequests(ID_PROFILE_CARSI12);
			bool expectedStatus = false;
			bool obtainedStatus = true;
			for (int i = 0; i < obtainedFriendsRequests.Count; i++) 
 			{
				if(obtainedFriendsRequests[i].Contains(NEW_NICKNAME))
				{         
					expectedStatus = true;
				} 
			}
			Assert.IsEqual(expectedStatus, obtainedStatus);
		}
		[Test]
		public void GetFriendsRequestNotCorrect()
		{
			List<string> obtainedFriendsRequests = USER_MANAGEMENT.GetFriendsRequests(-123);
			bool expectedStatus = false;
			bool obtainedStatus = false;
			for (int i = 0; i < obtainedFriendsRequests.Count; i++) 
 			{
				if(obtainedFriendsRequests[i].Contains(NEW_NICKNAME))
				{         
					expectedStatus = true;
				} 
			}
			Assert.IsEqual(expectedStatus, obtainedStatus);
		}
		[Test]
		public void AcceptFriendCorrect()
		{
			Profile profileTest = USER_MANAGEMENT.GetProfileByUsername(USERNAME);
			Friends friendsTest = new Friends(profileTest.idProfile, ID_PROFILE_CARSI12, true);
			bool expectedStatus = true;
			bool obtainedStatus = USER_MANAGEMENT.AcceptFriend(friendsTest);
			Assert.IsEqual(expectedStatus, obtainedStatus);
		}
		[Test]
		public void AcceptFriendInvalidOperationException()
		{
			Friends friendsTest = new Friends(-123, -123, true);
			bool expectedStatus = false;
			bool obtainedStatus = USER_MANAGEMENT.AcceptFriend(friendsTest);
			Assert.IsEqual(expectedStatus, obtainedStatus);
		}
		[Test]
		public void FriendshipExistCorrect()
		{
			Profile profileTest = USER_MANAGEMENT.GetProfileByUsername(USERNAME);
			bool expectedStatus = true;
			bool obtainedStatus = USER_MANAGEMENT.FriendshipExist(profileTest.idProfile, ID_PROFILE_CARSI12);
			Assert.IsEqual(expectedStatus, obtainedStatus);
		}
		[Test]
		public void FriendshipExistNotCorrect()
		{
			Profile profileTest = USER_MANAGEMENT.GetProfileByUsername(USERNAME);
			bool expectedStatus = false;
			bool obtainedStatus = USER_MANAGEMENT.FriendshipExist(profileTest.idProfile, ID_PROFILE_ZINE);
			Assert.IsEqual(expectedStatus, obtainedStatus);
		}
		[Test]
		public void GetFriendsCorrect()
		{
			Profile profileTest = USER_MANAGEMENT.GetProfileByUsername(USERNAME);
			List<string> obtainedFriends = USER_MANAGEMENT.GetFriends(profileTest.idProfile);
			bool expectedStatus = false;
			bool obtainedStatus = true;
			for (int i = 0; i < obtainedFriends.Count; i++) 
 			{
				if(obtainedFriends[i].Contains("Mordekarsi"))
				{         
					expectedStatus = true;
				} 
			}
			Assert.IsEqual(expectedStatus, obtainedStatus);
		}
		[Test]
		public void GetFriendsNotCorrect()
		{
			List<string> obtainedFriends = USER_MANAGEMENT.GetFriends(-123);
			bool expectedStatus = false;
			bool obtainedStatus = false;
			for (int i = 0; i < obtainedFriends.Count; i++) 
 			{
				if(obtainedFriends[i].Contains("Mordekarsi"))
				{         
					expectedStatus = true;
				} 
			}
			Assert.IsEqual(expectedStatus, obtainedStatus);
		}
		[Test]
		public void DenyFriendCorrect()
		{
			Profile profileTest = USER_MANAGEMENT.GetProfileByUsername(USERNAME);
			Friends friendsTest = new Friends(profileTest.idProfile, ID_PROFILE_CARSI12, true);
			bool expectedStatus = true;
			bool obtainedStatus = USER_MANAGEMENT.DenyFriend(friendsTest);
			Assert.IsEqual(expectedStatus, obtainedStatus);
		}
		[Test]
		public void DenyFriendInvalidOperationException()
		{
			Friends friendsTest = new Friends(-123, -123, true);
			bool expectedStatus = false;
			bool obtainedStatus = USER_MANAGEMENT.DenyFriend(friendsTest);
			Assert.IsEqual(expectedStatus, obtainedStatus);
		}
		[Test]
		public void DeleteFriendCorrect()
		{
			Profile profileTest = USER_MANAGEMENT.GetProfileByUsername(USERNAME);
			Friends friendsTest = new Friends(profileTest.idProfile, ID_PROFILE_CARSI12, true);
			USER_MANAGEMENT.AddFriend(friendsTest);
			bool expectedStatus = true;
			bool obtainedStatus = USER_MANAGEMENT.DeleteFriend(friendsTest);
			Assert.IsEqual(expectedStatus, obtainedStatus);
		}
		[Test]
		public void DeleteFriendNotCorrect()
		{
			Profile profileTest = USER_MANAGEMENT.GetProfileByUsername(USERNAME);
			Friends friendsTest = new Friends(profileTest.idProfile, -123, true);
			bool expectedStatus = false;
			bool obtainedStatus = USER_MANAGEMENT.DeleteFriend(friendsTest);
			Assert.IsEqual(expectedStatus, obtainedStatus);
		}
	}
}
