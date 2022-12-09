using Godot;
using System;
using OpenCodeChems.Client.Server;

public class CreateRoom : Control
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";

	// Called when the node enters the scene tree for the first time.
	Network serverClient;
	private ItemList usersList;
	public override void _Ready()
	{
		serverClient = GetNode<Network>("/root/Network") as Network;
		usersList = GetParent().GetNode<ItemList>("Control/RoomNinePatchRect/TeamRedColorRect/SpiesRedItemList");
<<<<<<< HEAD
		serverClient.Connect("DiosTienePoder", this, nameof(SiLoTiene));
=======
		serverClient.Connect("RoomJoin", this, nameof(AddToList));
>>>>>>> main
	}

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
	public void _on_CancelTextureButton_pressed()
	{
		GetTree().ChangeScene("res://Scenes/RoomSettings.tscn");
	}
	
	public void SiLoTiene()
	{
		GD.Print("Amen");
	}
	public void AddToList(/*int sender*/)
	{
		//string idSender = Convert.ToString(sender);
		GD.Print("entrando al add list"); 
		//GetParent().GetNode<ItemList>("Control/RoomNinePatchRect/TeamRedColorRect/SpiesRedItemList").AddItem("caca");
		//GD.Print($"Coso Realizado, se metió sender = {sender}");
	}
}
