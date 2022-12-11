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
	int [] membersBalance = new int[4] {0,0,0,0};
	
	private ItemList redUsersList;
	private ItemList blueUsersList;
	private ItemList redMasterList;
	private ItemList blueMasterList;
	private AcceptDialog notificacion;
    private Profile currentPlayer = null;
	private String nameRoom = Network.currentRoom;
	public override void _Ready()
	{  		
		serverClient = GetNode<Network>("/root/Network") as Network;
        currentPlayer = serverClient.profileByUsernameObtained;
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
		if(membersBalance[0]==0)
		{
			redMasterList.AddItem(currentPlayer.nickname);
			membersBalance[0]=1;
		}
		
	}
	public void _on_JoinSpyRedTextureButton_pressed()
	{
		if(membersBalance[1]<MAX_FIELD_MEMBERS)
		{
			redUsersList.AddItem(currentPlayer.nickname);
			membersBalance[1]= membersBalance[1]+1;
		}
	}
    public void _on_JoinSpyMasterBlueTextureButton_pressed()
    {
		if(membersBalance[2]==0)
		{
			blueMasterList.AddItem(currentPlayer.nickname);
			membersBalance[2]=1;
		}
    }
	
	public void _on_JoinSpyBlueTextureButton_pressed()
	{
		if(membersBalance[3]<MAX_FIELD_MEMBERS)
		{
			blueUsersList.AddItem(currentPlayer.nickname);
			membersBalance[3]= membersBalance[3]+1;
		}
	}
	public void AddToList(List<string> playersInRoom)
	{
		redUsersList.Clear();
		foreach(string name in playersInRoom)
		{
			redUsersList.AddItem(name);
		}
		/*if(currentPlayer != null)
        { 
			//membersBalance[1]= membersBalance[1]+1;
        }*/
		
		
	}
}
