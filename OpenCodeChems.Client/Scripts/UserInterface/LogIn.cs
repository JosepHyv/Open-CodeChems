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
	private Task<bool> registeredStatus = Task<bool>.FromResult(false);
	public static string username = "";
	private string password = "";
	public override void _Ready()
	{
		serverClient = GetNode<Network>("/root/Network") as Network;
		serverClient.Connect("LoggedIn", this, nameof(LoggedAcepted));
		serverClient.Connect("LoggedFail", this, nameof(LoggedFailed));
		serverClient.Connect("InvitatedRegistered", this, nameof(RegisteredAsInvitatedAccepted));
		serverClient.Connect("InvitatedRegisteredFail", this, nameof(RegisteredAsInvitatedFail));
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
			GetParent().GetNode<AcceptDialog>("LogIn/EmptyFieldsAcceptDialog").SetTitle("WARNING");
			GetParent().GetNode<AcceptDialog>("LogIn/EmptyFieldsAcceptDialog").SetText("VERIFY_EMPTY_FIELDS");
			GetParent().GetNode<AcceptDialog>("LogIn/EmptyFieldsAcceptDialog").Visible = true;
		}
		

	}

	private void _on_PlayInvitatedTextureButton_pressed()
	{
		serverClient.RegisterUserInvitated();
	}
	private void _on_RecoverPasswordTextureButton_pressed()
	{
		GetTree().ChangeScene("res://Scenes/RecoverPassword.tscn");
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

	public void RegisteredAsInvitatedAccepted()
	{
		registeredStatus = Task<bool>.FromResult(true);
		username = Network.usernamePlayerAsInvitated;
		GetParent().GetNode<AcceptDialog>("LogIn/EmptyFieldsAcceptDialog").SetTitle("NOTIFICATION");	
		GetParent().GetNode<AcceptDialog>("LogIn/EmptyFieldsAcceptDialog").SetText("REGISTER_COMPLETE");
		GetParent().GetNode<AcceptDialog>("LogIn/EmptyFieldsAcceptDialog").Visible = true;
		GetTree().ChangeScene("res://Scenes/MainMenu.tscn");
	}
	
	public void RegisteredAsInvitatedFail()
	{
		registeredStatus = Task<bool>.FromResult(false);
		GetParent().GetNode<AcceptDialog>("RegisterUser/RegisterUserDialog").SetTitle("WARNING");
		GetParent().GetNode<AcceptDialog>("RegisterUser/RegisterUserDialog").SetText("WRONG_REGISTER");
		GetParent().GetNode<AcceptDialog>("RegisterUser/RegisterUserDialog").Visible = true;
	}
}   



