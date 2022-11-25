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
	public override void _Ready()
	{
		serverClient = GetNode<Network>("/root/Network") as Network;
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
}
