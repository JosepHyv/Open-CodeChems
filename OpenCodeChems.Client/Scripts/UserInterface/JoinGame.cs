using Godot;
using System;
using OpenCodeChems.Client.Server;

namespace OpenCodeChems.Client.UserInterface
{
	public class JoinGame : Control
	{
		
		Network serverClient = null;
		LineEdit code  = null;
		AcceptDialog notification = null;
		
		public override void _Ready()
		{
			notification = GetParent().GetNode<AcceptDialog>("JoinGame/NotificationAcceptDialog");
			code = GetParent().GetNode<LineEdit>("JoinGame/JoinGameNinePatchRect/CodeLineEdit");
			serverClient = GetNode<Network>("/root/Network") as Network;
			serverClient.Connect("RoomJoinFail", this, nameof(FailJoin));
			serverClient.Connect("RoomJoin", this, nameof(ChangeScene));
		}
		public void _on_CancelTextureButton_pressed()
		{
			GetTree().ChangeScene("res://Scenes/MainMenu.tscn");
		}
		public void _on_JoinTextureButton_pressed()
		{
			string roomCode = code.Text;
			if(!String.IsNullOrWhiteSpace(roomCode))
			{
				serverClient.ClientJoinRoom(roomCode);
			}
			else
			{
				notification.DialogText = ("ROOM_CODE_INVALID");
				notification.Visible = true;	
			}
			
		}

		public void ChangeScene()
		{
			GetTree().ChangeScene("res://Scenes/CreateRoom.tscn");
			serverClient.RoomCreated();
		}
		
		public void FailJoin()
		{
			notification.DialogText = ("IMPOSSIBLE_TO_CONNECT");
			notification.Visible = true;
		}

	}
}