using Godot;
using System;
using System.Runtime.Remoting.Messaging;
using OpenCodeChems.Client.Resources;

public class LogIn : Control
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
        
	}

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
    private void _on_RegisterButton_pressed()
    {
        // Replace with function body.
        //Owo?	
        GetTree().ChangeScene("res://Scenes/RegisterUser.tscn");
        
    }

    private void _on_LogInButton_pressed()
    {
        GD.Print("Hola Mundo andamos en el login");

        string username = GetParent().GetNode<LineEdit>("LogIn/NinePatchRect/UsernameLineEdit").Text;
        string password = GetParent().GetNode<LineEdit>("LogIn/NinePatchRect/PasswordLineEdit").Text;

        
        if (!String.IsNullOrWhiteSpace(username) && !String.IsNullOrWhiteSpace(password) )
        {
            Encryption PasswordHasher = new Encryption();
            string hashPassword = PasswordHasher.ComputeSHA256Hash(password);
            GD.Print("Usuario atenticado");
            GetTree().ChangeScene("res://Scenes/MainMenu.tscn");
        }

    }

    
}   



