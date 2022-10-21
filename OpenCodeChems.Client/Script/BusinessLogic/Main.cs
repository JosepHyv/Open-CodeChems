using Godot;
using System;

public class Main : Node2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        GD.Print("Testing Main Node");
        GD.Print("Checkin Nodes");
        var SceneTree = GetTree().ChangeScene("res://Scenes/UserInterface/LogIn.tscn");
        GD.Print("Probando el ToString del Tree de la escena = " , SceneTree.ToString());
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
