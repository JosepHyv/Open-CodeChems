using Godot;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using OpenCodeChems.Client.Resources;
using OpenCodeChems.Client.Server;

namespace OpenCodeChems.Client.UserInterface
{
	public class LogIn : Control
	{

		Network serverClient;
		public static string username = "";
		public override void _Ready()
		{
			serverClient = GetNode<Network>("/root/Network") as Network;
			serverClient.Connect("LoggedIn", this, nameof(LoggedAcepted));
			serverClient.Connect("LoggedFail", this, nameof(LoggedFailed));
			serverClient.Connect("InvitatedRegistered", this, nameof(RegisteredAsInvitatedAccepted));
			serverClient.Connect("InvitatedRegisteredFail", this, nameof(RegisteredAsInvitatedFail));
		}


		public void _on_RegisterButton_pressed()
		{
			GetTree().ChangeScene("res://Scenes/RegisterUser.tscn");
		}	

		
		public void _on_LogInButton_pressed()
		{
			username = GetParent().GetNode<LineEdit>("LogIn/NinePatchRect/UsernameLineEdit").Text;
			string password = GetParent().GetNode<LineEdit>("LogIn/NinePatchRect/PasswordLineEdit").Text;
			if (!String.IsNullOrWhiteSpace(username) && !String.IsNullOrWhiteSpace(password) )
			{
				Encryption PasswordHasher = new Encryption();
				string hashPassword = PasswordHasher.ComputeSHA256Hash(password);
				serverClient.Login(username, hashPassword);
			}
			else
			{
				GetParent().GetNode<AcceptDialog>("LogIn/EmptyFieldsAcceptDialog").WindowTitle = ("WARNING");
				GetParent().GetNode<AcceptDialog>("LogIn/EmptyFieldsAcceptDialog").DialogText = ("VERIFY_EMPTY_FIELDS");
				GetParent().GetNode<AcceptDialog>("LogIn/EmptyFieldsAcceptDialog").Visible = true;
			}
			

		}

		public void _on_PlayInvitatedTextureButton_pressed()
		{
			serverClient.RegisterUserInvitated();
		}
		public void _on_RecoverPasswordTextureButton_pressed()
		{
			GetTree().ChangeScene("res://Scenes/RecoverPassword.tscn");	
		}
		
		public void LoggedAcepted()
		{
			GetTree().ChangeScene("res://Scenes/MainMenu.tscn");	
		}
		
		public void LoggedFailed()
		{
			GetParent().GetNode<AcceptDialog>("LogIn/EmptyFieldsAcceptDialog").WindowTitle = ("WARNING");
			GetParent().GetNode<AcceptDialog>("LogIn/EmptyFieldsAcceptDialog").DialogText = ("WRONG_CREDENTIALS");
			GetParent().GetNode<AcceptDialog>("LogIn/EmptyFieldsAcceptDialog").Visible = true;
		}

		public void RegisteredAsInvitatedAccepted()
		{
			username = serverClient.usernamePlayerAsInvitated;
			GetParent().GetNode<AcceptDialog>("LogIn/EmptyFieldsAcceptDialog").WindowTitle = ("NOTIFICATION");	
			GetParent().GetNode<AcceptDialog>("LogIn/EmptyFieldsAcceptDialog").DialogText = ("REGISTER_COMPLETE");
			GetParent().GetNode<AcceptDialog>("LogIn/EmptyFieldsAcceptDialog").Visible = true;
			GetTree().ChangeScene("res://Scenes/MainMenu.tscn");
		}
		
		public void RegisteredAsInvitatedFail()
		{
			GetParent().GetNode<AcceptDialog>("RegisterUser/RegisterUserDialog").WindowTitle = ("WARNING");
			GetParent().GetNode<AcceptDialog>("RegisterUser/RegisterUserDialog").DialogText = ("WRONG_REGISTER");
			GetParent().GetNode<AcceptDialog>("RegisterUser/RegisterUserDialog").Visible = true;
		}
	}   
}