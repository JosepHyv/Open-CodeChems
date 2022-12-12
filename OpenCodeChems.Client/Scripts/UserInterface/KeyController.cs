using Godot;
using System;
using OpenCodeChems.Client.Server;

public class KeyController : Control
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";

	// Called when the node enters the scene tree for the first time.
	Network serverClient;
	Random RandomClass = new Random();
	private int randomNumber = 0;
	public override void _Ready()
	{   
		serverClient = GetNode<Network>("/root/Network") as Network;
		LoadKey();
		randomNumber = RandomClass.Next(0,4);
		serverClient.SendSceneToServer(randomNumber);
	
	}

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }

	public void LoadKey()
	{   
		PackedScene packedScene;
		GD.Print(randomNumber);
		if(randomNumber == 0)
		{
			packedScene = (PackedScene)GD.Load("res://Scenes/MasterPlayer.tscn");
			Control key = (Control)packedScene.Instance();
			this.AddChild(key);
		}
		 if(randomNumber == 1)
		{
			packedScene = (PackedScene)GD.Load("res://Scenes/KeyRedOne.tscn");
			Control key = (Control)packedScene.Instance();
			this.AddChild(key);
		}
		 if(randomNumber == 2)
		{
			packedScene = (PackedScene)GD.Load("res://Scenes/KeyRedTwo.tscn");
			Control key = (Control)packedScene.Instance();
			this.AddChild(key);
		}
		 if(randomNumber == 3)
		{
			packedScene = (PackedScene)GD.Load("res://Scenes/KeyBlueTwo.tscn");
			Control key = (Control)packedScene.Instance();
			this.AddChild(key);
		}
	}

	
}
