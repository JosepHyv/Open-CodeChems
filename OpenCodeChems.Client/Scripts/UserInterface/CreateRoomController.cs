using Godot;
using System;
using System.Collections.Generic;
using OpenCodeChems.Client.Server;
using static OpenCodeChems.Client.Resources.Objects;
using OpenCodeChems.Client.Resources;

namespace OpenCodeChems.Client.UserInterface
{
	public class CreateRoomController : Control
	{
		private int MAX_FIELD_MEMBERS = 3; 
		Network serverClient;
		private ItemList redUsersList;
		private ItemList blueUsersList;
		private ItemList redMasterList;
		private ItemList blueMasterList;
		private AcceptDialog notificacion;
		private ConfirmationDialog banPlayerDialog;
	
		private String nameRoom = Network.currentRoom;

		private string banName = null;
		public override void _Ready()
		{  		
			serverClient = GetNode<Network>("/root/Network") as Network;
			banPlayerDialog = GetParent().GetNode<ConfirmationDialog>("Control/BanPlayerConfirmationDialog");
			redMasterList = GetParent().GetNode<ItemList>("Control/RoomNinePatchRect/TeamRedColorRect/SpyMasteRedrItemList");
			redUsersList = GetParent().GetNode<ItemList>("Control/RoomNinePatchRect/TeamRedColorRect/SpiesRedItemList");
			blueMasterList = GetParent().GetNode<ItemList>("Control/RoomNinePatchRect/TeamBlueColorRect/SpyMasterBlueItemList");
			blueUsersList = GetParent().GetNode<ItemList>("Control/RoomNinePatchRect/TeamBlueColorRect/SpiesBlueItemList");
			notificacion = GetParent().GetNode<AcceptDialog>("Control/Notificacion");

			banPlayerDialog.GetCancel().Connect("pressed", this, nameof(CancelBan));

			GetParent().GetNode<Label>("Control/RoomNinePatchRect/NameRoomLabel").Text = (nameRoom);
			serverClient.Connect("UpdatePlayersScreen", this, nameof(AddToList));
			serverClient.Connect("CleanRoom", this, nameof(ChangeToMainMenu));
			serverClient.Connect("CantChangeRol", this, nameof(NoRolChange));
			serverClient.Connect("CanBan", this, nameof(AvailableToBan));
			serverClient.Connect("BanFail", this, nameof(FailToBan));
			serverClient.Connect("YesStartGame", this, nameof(AvailableToStart));
			serverClient.Connect("NoStartGame", this, nameof(UnavailableToStart));

		}


		public void FailToBan()
		{
			notificacion.DialogText = ("CANT_BAN_USER");
			notificacion.Visible = true;
		}

		public void CancelBan()
		{
			banPlayerDialog.Visible = false;
		}

		public void AppliBan()
		{
			if(banName != null)
			{
				serverClient.BanPlayer(banName);
			}
		}

		public void AvailableToBan()
		{
			banPlayerDialog.DialogText = ($"Do You Want Ban To {banName}");
			banPlayerDialog.Visible = true;
		}

		public void NoRolChange()
		{
			notificacion.DialogText = ("CANT_CHANGE_ROL");
			notificacion.Visible = true;
		}

		public void ChangeToMainMenu()
		{
			GetTree().ChangeScene("res://Scenes/MainMenu.tscn");
		}
		public void _on_CancelTextureButton_pressed()
		{
			GetTree().ChangeScene("res://Scenes/RoomSettings.tscn");
			serverClient.LeftRoom();
		}
		
		public void _on_JoinSpyMasterRedTextureButton_pressed()
		{	
			serverClient.ChangeRolTo(Constants.RED_SPY_MASTER);
		}
		public void _on_JoinSpyRedTextureButton_pressed()
		{
			serverClient.ChangeRolTo(Constants.RED_PLAYER);
		}
		public void _on_JoinSpyMasterBlueTextureButton_pressed()
		{
			serverClient.ChangeRolTo(Constants.BLUE_SPY_MASTER);
		}
		
		public void _on_JoinSpyBlueTextureButton_pressed()
		{
			serverClient.ChangeRolTo(Constants.BLUE_PLAYER);
		}
		public void _on_SpyMasteRedrItemList_item_selected(int indexSelected)
		{
			banName = GetParent().GetNode<ItemList>("Control/RoomNinePatchRect/TeamRedColorRect/SpyMasteRedrItemList").GetItemText(indexSelected);
			serverClient.BanPermission();
		}

		public void _on_SpiesRedItemList_item_selected(int indexSelected)
		{
			banName = GetParent().GetNode<ItemList>("Control/RoomNinePatchRect/TeamRedColorRect/SpiesRedItemList").GetItemText(indexSelected);
			serverClient.BanPermission();
		}

		public void _on_SpyMasterBlueItemList_item_selected(int indexSelected)
		{
			banName = GetParent().GetNode<ItemList>("Control/RoomNinePatchRect/TeamBlueColorRect/SpyMasterBlueItemList").GetItemText(indexSelected);
			serverClient.BanPermission();
		}

		public void _on_SpiesBlueItemList_item_selected(int indexSelected)
		{
			banName = GetParent().GetNode<ItemList>("Control/RoomNinePatchRect/TeamBlueColorRect/SpiesBlueItemList").GetItemText(indexSelected);
			serverClient.BanPermission();
		}

		public void _on_StartGameTextureButton_pressed()
		{
			serverClient.StartGame();
			
		}

		public void AvailableToStart()
		{
			GD.Print("Change Scene Request");
			serverClient.ClientsChangeScene();
			
		}

		public void UnavailableToStart()
		{
			notificacion.DialogText = ("CANT_START_GAME");
			notificacion.Visible = true;
		}
		public void AddToList(string redMaster, string blueMaster, List<string> redPlayers, List<string>bluePlayers)
		{

			redMasterList.Clear();
			blueMasterList.Clear();
			redUsersList.Clear();
			blueUsersList.Clear();

			if(redMaster != null)
			{
				redMasterList.AddItem(redMaster);
			}

			if(blueMaster != null)
			{
				blueMasterList.AddItem(blueMaster);
			}
			
			foreach(string name in redPlayers)
			{
				redUsersList.AddItem(name);
			}

			foreach(string name in bluePlayers)
			{
				blueUsersList.AddItem(name);
			}

			
			
		}

	}
}