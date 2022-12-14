using Godot;
using System;
using System.Collections.Generic;
using OpenCodeChems.Client.Server;
using static OpenCodeChems.Client.Resources.Objects;
using OpenCodeChems.Client.Resources;


public class KeyController : Control
{

	Network serverClient;
	
	private int randomNumber = 0;
	public override void _Ready()
	{   
		GD.Print("Ready de Keys Controller");
		serverClient = GetNode<Network>("/root/Network") as Network;
		serverClient.Connect("UpdateBoardSignal", this, nameof(ChangeScreen));
	}



	public void ChangeScreen()
	{
		//randomNumber = number;
		GD.Print("Changing to Scene");
		/*if(rool == Constants.BLUE_SPY_MASTER || rool == Constants.RED_SPY_MASTER)
		{
			LoadKey();
		}
		else
		{
			GetTree().ChangeScene("res://Scenes/SpyPlayer.tscn");
		}*/
	}

	

	public void LoadKey()
	{   
		PackedScene packedScene;
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
