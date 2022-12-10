using Godot;
using System;
using OpenCodeChems.Client.Server;
using static OpenCodeChems.Client.Resources.Objects;

public class CreateRoomController : Control
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";

	// Called when the node enters the scene tree for the first time.
	Network serverClient;
	int [] membersBalance = new int[4] {0,0,0,0};
	private ItemList redUsersList;
	private ItemList blueUsersList;
	private ItemList redMasterList;
	private ItemList blueMasterList;
	private AcceptDialog notificacion;
    private Profile currentPlayer = null;
	public override void _Ready()
	{  		
		serverClient = GetNode<Network>("/root/Network") as Network;
        currentPlayer = serverClient.profileByUsernameObtained;
		redMasterList = GetParent().GetNode<ItemList>("Control/RoomNinePatchRect/TeamRedColorRect/SpyMasteRedrItemList");
		redUsersList = GetParent().GetNode<ItemList>("Control/RoomNinePatchRect/TeamRedColorRect/SpiesRedItemList");
		blueMasterList = GetParent().GetNode<ItemList>("Control/RoomNinePatchRect/TeamBlueColorRect/SpyMasterBlueItemList");
		blueUsersList = GetParent().GetNode<ItemList>("Control/RoomNinePatchRect/TeamBlueColorRect/SpiesBlueItemList");
		
		notificacion = GetParent().GetNode<AcceptDialog>("Control/Notificacion");
		serverClient.Connect("RoomJoin", this, nameof(AddToList));
	}

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
  public override void _Process(float delta)
    {
        //serverClient.Connect("DiosTienePoder", this, nameof(SiLoTiene));
    }
	public void _on_CancelTextureButton_pressed()
	{
		GetTree().ChangeScene("res://Scenes/RoomSettings.tscn");
	}
	
	public void _on_JoinSpyMasterRedTextureButton_pressed()
	{
		redMasterList.AddItem(currentPlayer.nickname);
	}
	public void _on_JoinSpyMasterBlueTextureButton_pressed()
	{
		blueMasterList.AddItem(currentPlayer.nickname);
	}
    public void _on_JoinSpyRedTextureButton_pressed()
    {
        redUsersList.AddItem(currentPlayer.nickname);
    }
	
	public void _on_JoinSpyBlueTextureButton_pressed()
	{
		blueUsersList.AddItem(currentPlayer.nickname);
	}
	public void AddToList()
	{
		if(currentPlayer != null)
        { 
		    redUsersList.AddItem(currentPlayer.nickname);
        }
		
		
	}
}
