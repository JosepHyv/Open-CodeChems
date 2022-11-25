using Godot;
using System;


public class KeyController : Control
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{   
		LoadKey();
	
	}

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }

	public void LoadKey()
	{   
		PackedScene packedScene;
		Random RandomClass = new Random();
		int randomNumber = RandomClass.Next(0,4);
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
