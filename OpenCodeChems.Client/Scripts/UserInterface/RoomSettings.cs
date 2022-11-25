using Godot;
using System;
using OpenCodeChems.Client.Server;

public class RoomSettings : Control
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";

	// Called when the node enters the scene tree for the first time.
	Network serverClient;
	private LineEdit nameRoom;
	public override void _Ready()
	{
		nameRoom =  GetParent().GetNode<LineEdit>("RoomSettings/CreateRoomNinePatchRect/NameRoomLineEdit");
		serverClient = GetNode<Network>("/root/Network") as Network;
	}
	public void _on_CancelTextureButton_pressed()
	{
		GetTree().ChangeScene("res://Scenes/MainMenu.tscn");
	}
//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
	private void _on_CreateTextureButton_pressed()
	{
		if(!String.IsNullOrWhiteSpace(nameRoom.GetText()))
		{
			serverClient.ClientCreateRoom(nameRoom.GetText());
		}
		// Replace with function body.
	}
}






