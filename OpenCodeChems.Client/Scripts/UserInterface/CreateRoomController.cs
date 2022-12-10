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
	private ItemList usersList;
	private AcceptDialog notificacion;
    private Profile currentPlayer = null;
	public override void _Ready()
	{  		
		serverClient = GetNode<Network>("/root/Network") as Network;
        currentPlayer = serverClient.profileByUsernameObtained;
		usersList = GetParent().GetNode<ItemList>("Control/RoomNinePatchRect/TeamRedColorRect/SpiesRedItemList");
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
	
    public void _on_JoinSpyRedTextureButton_pressed()
    {
        usersList.SetItemText(0,"care");
        serverClient.Esoterismo();
    }
	
	public void AddToList()
	{
		if(currentPlayer != null)
        { 
		    usersList.AddItem(currentPlayer.nickname);
        }
		
		
	}
}
