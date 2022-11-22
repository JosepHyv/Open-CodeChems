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
	Task<bool> loggedStatus = Task<bool>.FromResult(false);
	public override void _Ready()
	{
		serverClient = GetNode<Network>("/root/Network") as Network;
		serverClient.ConnectToServer();
		serverClient.Connect("LoggedIn", this, nameof(LoggedAcepted));
		serverClient.Connect("LoggedFail", this, nameof(LoggedFailed));
	}


	private void _on_RegisterButton_pressed()
	{
		
		GetTree().ChangeScene("res://Scenes/RegisterUser.tscn");
		
	}

	
	private async void _on_LogInButton_pressed()
	{
		string username = GetParent().GetNode<LineEdit>("LogIn/NinePatchRect/UsernameLineEdit").Text;
		string password = GetParent().GetNode<LineEdit>("LogIn/NinePatchRect/PasswordLineEdit").Text;
		if (!String.IsNullOrWhiteSpace(username) && !String.IsNullOrWhiteSpace(password) )
		{
			Encryption PasswordHasher = new Encryption();
			string hashPassword = PasswordHasher.ComputeSHA256Hash(password);
			serverClient.Login(username, hashPassword);
			await loggedStatus;
			if (loggedStatus.Result)
			{
				GetTree().ChangeScene("res://Scenes/MainMenu.tscn");
			}
			else
			{
				GetParent().GetNode<AcceptDialog>("LogIn/EmptyFieldsAcceptDialog").SetText("WRONG_CREDENTIALS");
				GetParent().GetNode<AcceptDialog>("LogIn/EmptyFieldsAcceptDialog").Visible = true;
			}
			
		}
		else
		{
			GetParent().GetNode<AcceptDialog>("LogIn/EmptyFieldsAcceptDialog").SetText("EMPTY_FIELDS");
			GetParent().GetNode<AcceptDialog>("LogIn/EmptyFieldsAcceptDialog").Visible = true;
			GD.Print("Empty fields");
		}

	}

	private void LoginRequest(string username, string hashPassword)
	{
		RpcId(PEER_ID, "LoginRequest", username, hashPassword);
	}
	private void ReturnLoginRequest(bool results)
	{
		if(results == true)
		{
			GetTree().ChangeScene("res://Scenes/MainMenu.tscn");
		}
		else 
		{
			GD.Print("Login error");
		}
	}
	
	public void LoggedAcepted()
	{
		loggedStatus = Task<bool>.FromResult(true);
	}
	
	public void LoggedFailed()
	{
		loggedStatus = Task<bool>.FromResult(false);
	}

	
}   



