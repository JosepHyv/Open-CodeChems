using Godot;
using System;
using OpenCodeChems.Client.Server;
using System.Threading.Tasks;
using OpenCodeChems.Client.Resources;
using static OpenCodeChems.Client.Resources.Objects;

public class MainMenu : Control
{
    string username = LogIn.username;
    Network serverClient;
	int PEER_ID = 1; 
    public static Profile actualPlayer;
    private Task<bool> getProfileStatus = Task<bool>.FromResult(false);
    

    public override void _Ready()
    {
        GD.Print(username);
        serverClient = GetNode<Network>("/root/Network") as Network;
        serverClient.Connect("ProfileFound", this, nameof(GetProfileComplete));
        serverClient.Connect("ProfileNotFound", this, nameof(GetProfileFail));
        serverClient.GetProfile(username);
        GetParent().GetNode<Label>("MainMenu/BackgroundMenuNinePatchRect/MenuColorRect/NicknameLabel").Text = username;
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



    public void _on_FriendsRequestButton_pressed()
    {
        GD.Print("No Implemented Yet");
    }

    public void GetProfileComplete(string nickname, int victories, int defeats, byte[] imageProfile, string username)
	{
        nickname = actualPlayer.nickname;
        victories = actualPlayer.victories;
        defeats = actualPlayer.defeats;
        imageProfile = actualPlayer.imageProfile;
        username = actualPlayer.username;
        GetParent().GetNode<Label>("MainMenu/BackgroundMenuNinePatchRect/MenuColorRect/NicknameLabel").Text = nickname;
        getProfileStatus = Task<bool>.FromResult(true);
	}
	
	public void GetProfileFail()
	{
        getProfileStatus = Task<bool>.FromResult(false);
	}
    public void GettingProfile(string usernameFind)
    {
        serverClient.GetProfile(usernameFind);
    }
}
