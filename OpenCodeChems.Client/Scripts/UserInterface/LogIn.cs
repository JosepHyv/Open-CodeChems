using Godot;
using System;
using System.Runtime.Remoting.Messaging;
using OpenCodeChems.Client.Resources;
using OpenCodeChems.Client.Server;

public class LogIn : Control
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";

	// Called when the node enters the scene tree for the first time.
    Network serverClient;
	public override void _Ready()
	{
        serverClient = GetNode<Network>("/root/Network") as Network;
        serverClient.ConnectToServer();
      
        // GD.Print("Intentando conectar al server");
        
       // serverClient.ConnectToServer();
		
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
       // GD.Print($"Estoy conectado? {serverClient.estaConectado()}");
        serverClient.estaConectado();
		string username = GetParent().GetNode<LineEdit>("LogIn/NinePatchRect/UsernameLineEdit").Text;
		string password = GetParent().GetNode<LineEdit>("LogIn/NinePatchRect/PasswordLineEdit").Text;
        GD.Print($"en el cliente tengo username = {username} password = {password}");
		//serverClient.LoginPlayer(username, password);
        serverClient.login(username, password);
		/*if (!String.IsNullOrWhiteSpace(username) && !String.IsNullOrWhiteSpace(password) )
        {


            var peer = new NetworkedMultiplayerENet();
            peer.CreateClient("localhost", 7290);
            GetTree().NetworkPeer = peer;
			Encryption PasswordHasher = new Encryption();
			string hashPassword = PasswordHasher.ComputeSHA256Hash(password);
            GetTree().ChangeScene("res://Scenes/MainMenu.tscn");
		}*/

	}

	
}   



