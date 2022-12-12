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
	private AcceptDialog dialog;
	public override void _Ready()
	{
		nameRoom =  GetParent().GetNode<LineEdit>("RoomSettings/CreateRoomNinePatchRect/NameRoomLineEdit");
		dialog = GetParent().GetNode<AcceptDialog>("RoomSettings/AcceptDialog");
		serverClient = GetNode<Network>("/root/Network") as Network;
		serverClient.Connect("RoomCreation", this, nameof(ChangeToRoom));
		serverClient.Connect("RoomCreationFail", this, nameof(DisplayAlert));
	}
	public void _on_CancelTextureButton_pressed()
	{
		GetTree().ChangeScene("res://Scenes/MainMenu.tscn");
	}
	
	private void _on_CreateTextureButton_pressed()
	{
		if(!String.IsNullOrWhiteSpace(nameRoom.GetText()))
		{
			serverClient.ClientCreateRoom(nameRoom.GetText());
		}
		// Replace with function body.
	}
	
	public void ChangeToRoom()
	{
		GetTree().ChangeScene("res://Scenes/CreateRoom.tscn");
		serverClient.RoomCreated();
	}

	
	public void DisplayAlert()
	{
		string warning = "ROOM_EXIST_WITH_NAME" + "\n" + nameRoom.GetText();
		dialog.SetText(warning);
		dialog.Visible = true;
	}
}






