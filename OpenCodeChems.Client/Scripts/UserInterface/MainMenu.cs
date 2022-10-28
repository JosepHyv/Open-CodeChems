using Godot;
using System;

public class MainMenu : Control
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        
    }
    public void _on_SettingsTextureButton_pressed()
    {
        
        GetTree().ChangeScene("res://Scenes/SelectLenguage.tscn");
        
    }
    public void _on_LogOutTextureButton_pressed()
    {
        GetTree().ChangeScene("res://Scenes/LogIn.tscn");
        // ToDo
        // implementing logout in server for secure connection
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

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }

    public void _on_FriendsRequestButton_pressed()
    {
        GD.Print("No Implemented Yet");
    }
}
