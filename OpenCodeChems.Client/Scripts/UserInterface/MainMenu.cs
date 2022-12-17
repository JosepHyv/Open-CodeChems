using Godot;
using System;
using OpenCodeChems.Client.Server;
using System.Threading.Tasks;
using OpenCodeChems.Client.Resources;
using OpenCodeChems.Client.UserInterface;
using static OpenCodeChems.Client.Resources.Objects;
using System.IO;
using System.Collections.Generic;

namespace OpenCodeChems.Client.UserInterface
{
	public class MainMenu : Control
	{
		public static string username = "";
		Network serverClient;
		public static Profile actualPlayer;
		public static string nicknameFriend = "";
		public string nicknameActualPlayer = "";
		private List<string> friendsOfActualPlayer;
		public static int idProfile = 0; 
		public static int imageProfile = 0;
		public static string pathImageProfile = "";
		private readonly ImageTexture textureImageProfile = new ImageTexture(); 
		private readonly Image image = new Image();


		public override void _Ready()
		{
			serverClient = GetNode<Network>("/root/Network") as Network;
			username = LogIn.username;
			serverClient.Connect("ProfileByUsernameFound", this, nameof(GetProfileByUsernameComplete));
			serverClient.Connect("ProfileByUsernameNotFound", this, nameof(GetProfileByUsernameFail));
			serverClient.GetProfileByUsername(username);
			serverClient.Connect("FriendsFound", this, nameof(GetFriendsComplete));
			serverClient.Connect("FriendsNotFound", this, nameof(GetFriendsFail));
			serverClient.Connect("CorrectDeleteInvitated", this, nameof(DeletePlayerCorrect));
			serverClient.Connect("DeleteInvitatedFail", this, nameof(DeletePlayerFail));
		}
		public void _on_SettingsTextureButton_pressed()
		{
			GetTree().ChangeScene("res://Scenes/SelectLenguage.tscn");	
		}
		public void _on_LogOutTextureButton_pressed()
		{
			bool statusPlayer = validatePlayer();
			if(statusPlayer == false)
			{
				serverClient.DeleteInvitatedPlayer(username);
				serverClient.LogOut();
			}
			else
			{
				GetTree().ChangeScene("res://Scenes/LogIn.tscn");
				serverClient.LogOut();
			}
		}
		public void _on_CreateGameTextureButton_pressed()
		{
			GetTree().ChangeScene("res://Scenes/RoomSettings.tscn");
		}
		public void _on_JoinGameTextureButton_pressed()
		{
			GetTree().ChangeScene("res://Scenes/JoinGame.tscn");
		}
		public void _on_AddFriendButton_pressed()
		{
			GetTree().ChangeScene("res://Scenes/AddFriend.tscn");
			serverClient.profileByUsernameObtained = null;
		}
		public void _on_FriendsRequestButton_pressed()
		{
			GetTree().ChangeScene("res://Scenes/FriendRequests.tscn");
		}
		public void _on_ImageProfileTextureButton_pressed()
		{
			GetTree().ChangeScene("res://Scenes/EditProfile.tscn");
		}
		public void _on_FriendsItemList_item_selected(int indexFriendSelected)
		{
			nicknameFriend = GetParent().GetNode<ItemList>("MainMenu/BackgroundMenuNinePatchRect/MenuColorRect/FriendsItemList").GetItemText(indexFriendSelected);
			GetTree().ChangeScene("res://Scenes/SeeFriend.tscn");
		}


		public void GetProfileByUsernameComplete()
		{
			if(serverClient.profileByUsernameObtained != null)
			{
				actualPlayer = serverClient.profileByUsernameObtained;
				idProfile = actualPlayer.idProfile;
				nicknameActualPlayer = actualPlayer.nickname;
				int victories = actualPlayer.victories;
				int defeats = actualPlayer.defeats;
				imageProfile = actualPlayer.imageProfile;
				string usernameObtained = actualPlayer.username;
				GetParent().GetNode<Label>("MainMenu/BackgroundMenuNinePatchRect/MenuColorRect/NicknameLabel").Text = nicknameActualPlayer;
				pathImageProfile = GetImageProfilePath(imageProfile);
				image.Load(pathImageProfile);
				textureImageProfile.CreateFromImage(image);
				GetParent().GetNode<TextureButton>("MainMenu/BackgroundMenuNinePatchRect/MenuColorRect/ImageProfileTextureButton").TextureNormal = (textureImageProfile);
				serverClient.UpdateServerData(nicknameActualPlayer);
				bool statusPlayer = validatePlayer();
				if(statusPlayer == false)
				{
					GetParent().GetNode<Button>("MainMenu/BackgroundMenuNinePatchRect/MenuColorRect/AddFriendButton").Disabled = true;
					GetParent().GetNode<Button>("MainMenu/BackgroundMenuNinePatchRect/MenuColorRect/FriendsRequestButton").Disabled = true;
					GetParent().GetNode<TextureButton>("MainMenu/BackgroundMenuNinePatchRect/MenuColorRect/ImageProfileTextureButton").Disabled = true;
					GetParent().GetNode<TextureButton>("MainMenu/BackgroundMenuNinePatchRect/CreateGameTextureButton").Disabled = true;
				}
			}
			serverClient.GetFriends(idProfile);
		}

		
		
		public void GetProfileByUsernameFail()
		{
			GetParent().GetNode<AcceptDialog>("MainMenu/BackgroundMenuNinePatchRect/MainMenuAcceptDialog").DialogText =("ERROR_LOADING_PROFILE");
			GetParent().GetNode<AcceptDialog>("MainMenu/BackgroundMenuNinePatchRect/MainMenuAcceptDialog").WindowTitle = ("ERROR");
			GetParent().GetNode<AcceptDialog>("MainMenu/BackgroundMenuNinePatchRect/MainMenuAcceptDialog").Visible = true;
		}
		public void GetFriendsComplete()
		{
			if(serverClient.friendsObtained !=null)
			{
				friendsOfActualPlayer = serverClient.friendsObtained;
				foreach(var friend in friendsOfActualPlayer)
				{
					GetParent().GetNode<ItemList>("MainMenu/BackgroundMenuNinePatchRect/MenuColorRect/FriendsItemList").AddItem(friend.ToString(), null, true);
				}
			}
		}
		public void GetFriendsFail()
		{
			GetParent().GetNode<AcceptDialog>("MainMenu/BackgroundMenuNinePatchRect/MainMenuAcceptDialog").DialogText =("ERROR_LOADING_FRIENDS");
			GetParent().GetNode<AcceptDialog>("MainMenu/BackgroundMenuNinePatchRect/MainMenuAcceptDialog").WindowTitle = ("ERROR");
			GetParent().GetNode<AcceptDialog>("MainMenu/BackgroundMenuNinePatchRect/MainMenuAcceptDialog").Visible = true;
		}
		public bool validatePlayer()
		{
			bool userRegistered = true;
			if(nicknameActualPlayer.Contains("Chemsito"))
			{
				userRegistered = false;
			}
			return userRegistered;
		}
		public void DeletePlayerCorrect()
		{
			GetTree().ChangeScene("res://Scenes/LogIn.tscn");
		}
		public void DeletePlayerFail()
		{
			GetParent().GetNode<AcceptDialog>("MainMenu/BackgroundMenuNinePatchRect/MainMenuAcceptDialog").DialogText =("ERROR_DELETE_INVITATED");
			GetParent().GetNode<AcceptDialog>("MainMenu/BackgroundMenuNinePatchRect/MainMenuAcceptDialog").WindowTitle = ("ERROR");
			GetParent().GetNode<AcceptDialog>("MainMenu/BackgroundMenuNinePatchRect/MainMenuAcceptDialog").Visible = true;
		}
		public string GetImageProfilePath(int imageProfile)
		{
			switch (imageProfile) 
			{
				case Constants.IMAGE_PROFILE_DEFAULT:
				pathImageProfile = "res://Scenes/Resources/ImagesProfile/ImagePerfilDefault.jpg";
				break;
				case Constants.IMAGE_PROFILE_CHEMS_GAMER:
				pathImageProfile = "res://Scenes/Resources/ImagesProfile/chemsGamer.jpg";
				break;
				case Constants.IMAGE_PROFILE_CHEMS_CHRISTMAS:
				pathImageProfile = "res://Scenes/Resources/ImagesProfile/chemsNavidad.jpg";
				break;
				case Constants.IMAGE_PROFILE_DRAVEN:
				pathImageProfile = "res://Scenes/Resources/ImagesProfile/draven.jpeg";
				break;
				case Constants.IMAGE_PROFILE_KITTY_MINECRAFT:
				pathImageProfile = "res://Scenes/Resources/ImagesProfile/kittyMinecraft.jpeg";
				break;
				case Constants.IMAGE_PROFILE_LINK:
				pathImageProfile = "res://Scenes/Resources/ImagesProfile/link.jpeg";
				break;
				case Constants.IMAGE_PROFILE_MECH:
				pathImageProfile = "res://Scenes/Resources/ImagesProfile/mech.png";
				break;
				case Constants.IMAGE_PROFILE_MECH_CHIVA:
				pathImageProfile = "res://Scenes/Resources/ImagesProfile/mechsChivahermano.png";
				break;
				case Constants.IMAGE_PROFILE_COLD_TEAM:
				pathImageProfile = "res://Scenes/Resources/ImagesProfile/teamFrio.jpg";
				break;
				case Constants.IMAGE_PROFILE_WINDOWS:
				pathImageProfile = "res://Scenes/Resources/ImagesProfile/windows.jpeg";
				break;
			}
			return pathImageProfile;
		}

	}
}