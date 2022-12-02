using Godot;
using System;
using OpenCodeChems.Client.Server;
using System.Threading.Tasks;
using OpenCodeChems.Client.Resources;
using static OpenCodeChems.Client.Resources.Objects;
using System.IO;

public class MainMenu : Control
{
	public static string username = LogIn.username;
	Network serverClient;
	int PEER_ID = 1; 
	public Profile actualPlayer;


	public override void _Ready()
	{
		serverClient = GetNode<Network>("/root/Network") as Network;
		serverClient.Connect("ProfileFound", this, nameof(GetProfileComplete));
		serverClient.Connect("ProfileNotFound", this, nameof(GetProfileFail));
		serverClient.GetProfile(username);
	}
	public void _on_SettingsTextureButton_pressed()
	{
		GetTree().ChangeScene("res://Scenes/SelectLenguage.tscn");	
	}
	public void _on_LogOutTextureButton_pressed()
	{
		GetTree().ChangeScene("res://Scenes/LogIn.tscn");
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
	}
	public void _on_FriendRequestButton_pressed()
	{
		GetTree().ChangeScene("res://Scenes/FriendRequests.tscn");
	}
	public void _on_ImageProfileTextureButton_pressed()
	{
		GetTree().ChangeScene("res://Scenes/EditProfile.tscn");
	}


	public void GetProfileComplete()
	{
		if(serverClient.profileObtained != null)
		{
			actualPlayer = serverClient.profileObtained;
			string nickname = actualPlayer.nickname;
			int victories = actualPlayer.victories;
			int defeats = actualPlayer.defeats;
			byte [] imageProfile = actualPlayer.imageProfile;
			string usernameObtained = actualPlayer.username;
			GetParent().GetNode<Label>("MainMenu/BackgroundMenuNinePatchRect/MenuColorRect/NicknameLabel").Text = nickname;
		}
	}

	/*public Image ByteArrayToImage(byte[] data)
	{
		
	}*/
	
	public void GetProfileFail()
	{
		GetParent().GetNode<AcceptDialog>("MainMenu/BackgroundMenuNinePatchRect/MainMenuAcceptDialog").SetText("ERROR_LOADING_PROFILE");
		GetParent().GetNode<AcceptDialog>("MainMenu/BackgroundMenuNinePatchRect/MainMenuAcceptDialog").SetTitle("WARNING");
		GetParent().GetNode<AcceptDialog>("MainMenu/BackgroundMenuNinePatchRect/MainMenuAcceptDialog").Visible = true;
	}

}
