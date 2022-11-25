using Godot;
using System;
using OpenCodeChems.Client.Server;

public class MainMenu : Control
{
    string username = LogIn.username;
    Network serverClient;
	int PEER_ID = 1; 
    public override void _Ready()
    {
        serverClient = GetNode<Network>("/root/Network") as Network;
        
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
}
