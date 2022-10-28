using Godot;
using System;

public class SpyPlayer : Control
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        
    }

    public void _on_CodeNamesItemList_item_selected()
    {
        GD.Print("aqui va la palabra");
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
