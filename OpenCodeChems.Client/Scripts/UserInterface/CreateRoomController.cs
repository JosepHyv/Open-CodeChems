using Godot;
using System;
using System.Collections.Generic;
using OpenCodeChems.Client.Server;
using static OpenCodeChems.Client.Resources.Objects;

public class CreateRoomController : Control
{
	private int MAX_FIELD_MEMBERS = 3; 
	Network serverClient;
	//position 0 belongs to spy master red, position 1 belongs to red "field" spies, position 2 belongs to spy master blue, position 3 belongs to blue "field" spies
	
	
	private ItemList redUsersList;
	private ItemList blueUsersList;
	private ItemList redMasterList;
	private ItemList blueMasterList;
	private AcceptDialog notificacion;
   
	private String nameRoom = Network.currentRoom;
	public override void _Ready()
	{  		
		serverClient = GetNode<Network>("/root/Network") as Network;
		redMasterList = GetParent().GetNode<ItemList>("Control/RoomNinePatchRect/TeamRedColorRect/SpyMasteRedrItemList");
		redUsersList = GetParent().GetNode<ItemList>("Control/RoomNinePatchRect/TeamRedColorRect/SpiesRedItemList");
		blueMasterList = GetParent().GetNode<ItemList>("Control/RoomNinePatchRect/TeamBlueColorRect/SpyMasterBlueItemList");
		blueUsersList = GetParent().GetNode<ItemList>("Control/RoomNinePatchRect/TeamBlueColorRect/SpiesBlueItemList");
		notificacion = GetParent().GetNode<AcceptDialog>("Control/Notificacion");

		GetParent().GetNode<Label>("Control/RoomNinePatchRect/NameRoomLabel").SetText(nameRoom);
		serverClient.Connect("UpdatePlayersScreen", this, nameof(AddToList));
		serverClient.Connect("CleanRoom", this, nameof(ChangeToMainMenu));
	}

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
  //public override void _Process(float delta)
    //{
    //}
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
		
		
	}
	public void _on_JoinSpyRedTextureButton_pressed()
	{
		
	}
    public void _on_JoinSpyMasterBlueTextureButton_pressed()
    {
		
    }
	
	public void _on_JoinSpyBlueTextureButton_pressed()
	{
		
	}
	public void AddToList(string redMaster, string blueMaster, List<string> redPlayers, List<string>bluePlayers)
	{
		redMasterList.Clear();
		blueMasterList.Clear();
		redUsersList.Clear();
		blueUsersList.Clear();

		redMasterList.AddItem(redMaster);
		blueMasterList.AddItem(blueMaster);
		
		foreach(string name in redPlayers)
		{
			redUsersList.AddItem(name);
		}

		foreach(string name in bluePlayers)
		{
			blueUsersList.AddItem(name);
		}
		/*if(currentPlayer != null)
        { 
			//membersBalance[1]= membersBalance[1]+1;
        }*/
		
		
	}
}
