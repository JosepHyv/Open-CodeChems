using Godot;
using System;

public class interactionCShapGDScript : Control
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        
    }

    public void _on_TextureButton_pressed()
    {
        GD.Print("se presionó");
        GetParent().GetNode<Control>("pruebaEmail/emailSenderNode").Call("SendEmail", "miguelzinedinne@gmail.com", "entra a la sala", "sala de juego");
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
