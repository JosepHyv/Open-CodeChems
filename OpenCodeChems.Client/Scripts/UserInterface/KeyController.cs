using Godot;
using System;
using OpenCodeChems.Client.Server;
using OpenCodeChems.Client.Resources;


public class KeyController : Control
{

	Network serverClient;
	Random RandomClass = new Random();
	private int randomNumber = 0;
	public override void _Ready()
	{   
		serverClient = GetNode<Network>("/root/Network") as Network;
		randomNumber = RandomClass.Next(0,4);
		serverClient.SendSceneToServer(randomNumber);
		serverClient.Connect("UpdateGameClient", this, nameof(ChangeScreen));
	
	}



	public void ChangeScreen(string rool, int number)
	{
		randomNumber = number;
		ModifyScene(rool);
	}

	public void ModifyScene(string rool)
	{
		GD.Print($"Changing to Scene {rool}");
		if(rool == Constants.BLUE_SPY_MASTER || rool == Constants.RED_SPY_MASTER)
		{
			LoadKey();
		}
		else
		{
			GetTree().ChangeScene("res://Scenes/SpyPlayer.tscn");
		}
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
