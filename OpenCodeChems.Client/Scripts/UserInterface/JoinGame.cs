using Godot;
using System;
using OpenCodeChems.Client.Server;

public class JoinGame : Control
{
	
	Network serverClient = null;
	LineEdit code  = null;
	AcceptDialog notification = null;
	
	public override void _Ready()
	{
		notification = GetParent().GetNode<AcceptDialog>("control/JoinGameNinePatchRect/Notification");
		code = GetParent().GetNode<LineEdit>("control/JoinGameNinePatchRect/CodeLineEdit");
		serverClient = GetNode<Network>("/root/Network") as Network;
		serverClient.Connect("RoomJoinFail", this, nameof(FailJoin));
	}
	public void _on_CancelTextureButton_pressed()
	{
		GetTree().ChangeScene("res://Scenes/MainMenu.tscn");
	}
	public void _on_JoinButton_pressed()
	{
		string roomCode = code.GetText();
		if(!String.IsNullOrWhiteSpace(roomCode))
		{
			serverClient.ClientJoinRoom(roomCode);
		}
		else
		{
			notification.SetText("ROOM_CODE_INVALID");
			notification.Visible = true;	
		}
		
	}
	
	public void FailJoin()
	{
		notification.SetText("IMPOSSIBLE_TO_CONNECT");
		notification.Visible = true;
	}

}
