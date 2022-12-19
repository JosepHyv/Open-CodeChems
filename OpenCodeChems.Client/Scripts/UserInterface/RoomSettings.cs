using Godot;
using System;
using OpenCodeChems.Client.Server;

namespace OpenCodeChems.Client.UserInterface
{
	public class RoomSettings : Control
	{
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
		
		public void _on_CreateTextureButton_pressed()
		{
			if(!String.IsNullOrWhiteSpace(nameRoom.Text))
			{
				serverClient.ClientCreateRoom(nameRoom.Text);
			}
		}
		
		public void ChangeToRoom()
		{
			GetTree().ChangeScene("res://Scenes/CreateRoom.tscn");
			serverClient.RoomCreated();
		}

		
		public void DisplayAlert()
		{
			string warning = "ROOM_EXIST_WITH_NAME" + "\n" + nameRoom.Text;
			dialog.DialogText = (warning);
			dialog.Visible = true;
		}
	}
}