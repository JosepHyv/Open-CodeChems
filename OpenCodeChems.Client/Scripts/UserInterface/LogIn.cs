using Godot;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using OpenCodeChems.Client.Resources;
using OpenCodeChems.Client.Server;

public class LogIn : Control
{

	Network serverClient;
	int PEER_ID = 1; 
	private Task<bool> loggedStatus = Task<bool>.FromResult(false);
	public static string username = "";
	private string password = "";
	public override void _Ready()
	{
		serverClient = GetNode<Network>("/root/Network") as Network;
		//serverClient.ConnectToServer();
		serverClient.Connect("LoggedIn", this, nameof(LoggedAcepted));
		serverClient.Connect("LoggedFail", this, nameof(LoggedFailed));
	}


	private void _on_RegisterButton_pressed()
	{
		GetTree().ChangeScene("res://Scenes/RegisterUser.tscn");
	}	

	
	private void _on_LogInButton_pressed()
	{
		username = GetParent().GetNode<LineEdit>("LogIn/NinePatchRect/UsernameLineEdit").Text;
		password = GetParent().GetNode<LineEdit>("LogIn/NinePatchRect/PasswordLineEdit").Text;
		if (!String.IsNullOrWhiteSpace(username) && !String.IsNullOrWhiteSpace(password) )
		{
			Encryption PasswordHasher = new Encryption();
			string hashPassword = PasswordHasher.ComputeSHA256Hash(password);
			serverClient.Login(username, hashPassword);
		}
		else
		{
			GetParent().GetNode<AcceptDialog>("LogIn/EmptyFieldsAcceptDialog").SetText("VERIFY_EMPTY_FIELDS");
			GetParent().GetNode<AcceptDialog>("LogIn/EmptyFieldsAcceptDialog").Visible = true;
		}
		

	}
	
	public void LoggedAcepted()
	{
		loggedStatus = Task<bool>.FromResult(true);
		GetTree().ChangeScene("res://Scenes/MainMenu.tscn");	
	}
	
	public void LoggedFailed()
	{
		GetParent().GetNode<AcceptDialog>("LogIn/EmptyFieldsAcceptDialog").SetTitle("WARNING");
		GetParent().GetNode<AcceptDialog>("LogIn/EmptyFieldsAcceptDialog").SetText("WRONG_CREDENTIALS");
		GetParent().GetNode<AcceptDialog>("LogIn/EmptyFieldsAcceptDialog").Visible = true;
		loggedStatus = Task<bool>.FromResult(false);
	}

	
}   



