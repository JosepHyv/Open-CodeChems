using Godot;
using System;
using OpenCodeChems.Client.Server;
using System.Threading.Tasks;
using OpenCodeChems.Client.Resources;
using static OpenCodeChems.Client.Resources.Objects;
using System.IO;
using System.Collections.Generic;

public class MainMenu : Control
{
	public static string username = "";
	Network serverClient;
	int PEER_ID = 1; 
	public static Profile actualPlayer;
	public static string nicknameFriend = "";
	public string nicknameActualPlayer = "";
	private List<string> friendsOfActualPlayer;
	public static int idProfile = 0; 
	private bool STATUS_FRIENDS = true;


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
		if(statusPlayer == true)
		{
			serverClient.DeleteInvitatedPlayer(username);
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
			string nickname = actualPlayer.nickname;
			int victories = actualPlayer.victories;
			int defeats = actualPlayer.defeats;
			byte [] imageProfile = actualPlayer.imageProfile;
			string usernameObtained = actualPlayer.username;
			GetParent().GetNode<Label>("MainMenu/BackgroundMenuNinePatchRect/MenuColorRect/NicknameLabel").Text = nickname;
			serverClient.UpdateServerData(nickname);
			bool statusPlayer = validatePlayer();
			if(statusPlayer == true)
			{
				GetParent().GetNode<Button>("MainMenu/BackgroundMenuNinePatchRect/MenuColorRect/AddFriendButton").Disabled = true;
				GetParent().GetNode<Button>("MainMenu/BackgroundMenuNinePatchRect/MenuColorRect/FriendsRequestButton").Disabled = true;
				GetParent().GetNode<TextureButton>("MainMenu/BackgroundMenuNinePatchRect/MenuColorRect/ImageProfileTextureButton").Disabled = true;
				GetParent().GetNode<TextureButton>("MainMenu/BackgroundMenuNinePatchRect/CreateGameTextureButton").Disabled = true;
			}
		}
		serverClient.GetFriends(idProfile, STATUS_FRIENDS);
	}

	/*public Image ByteArrayToImage(byte[] data)
	{
		
	}*/
	
	public void GetProfileByUsernameFail()
	{
		GetParent().GetNode<AcceptDialog>("MainMenu/BackgroundMenuNinePatchRect/MainMenuAcceptDialog").SetText("ERROR_LOADING_PROFILE");
		GetParent().GetNode<AcceptDialog>("MainMenu/BackgroundMenuNinePatchRect/MainMenuAcceptDialog").SetTitle("ERROR");
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
		GetParent().GetNode<AcceptDialog>("MainMenu/BackgroundMenuNinePatchRect/MainMenuAcceptDialog").SetText("ERROR_LOADING_FRIENDS");
		GetParent().GetNode<AcceptDialog>("MainMenu/BackgroundMenuNinePatchRect/MainMenuAcceptDialog").SetTitle("ERROR");
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
		GetParent().GetNode<AcceptDialog>("MainMenu/BackgroundMenuNinePatchRect/MainMenuAcceptDialog").SetText("ERROR_DELETE_INVITATED");
		GetParent().GetNode<AcceptDialog>("MainMenu/BackgroundMenuNinePatchRect/MainMenuAcceptDialog").SetTitle("ERROR");
		GetParent().GetNode<AcceptDialog>("MainMenu/BackgroundMenuNinePatchRect/MainMenuAcceptDialog").Visible = true;
	}

}
