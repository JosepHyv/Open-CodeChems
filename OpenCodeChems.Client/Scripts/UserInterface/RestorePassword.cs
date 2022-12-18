using Godot;
using OpenCodeChems.Client.Resources;
using OpenCodeChems.Client.Server;
using OpenCodeChems.Client.UserInterface;
using System;
using System.Threading.Tasks;

namespace OpenCodeChems.Client.UserInterface
{
	public class RestorePassword : Control
	{
		Network serverClient;
		private string password = "";
		private string confirmPassword = "";
		private string email = ConfirmRecoverPassword.email;
		public override void _Ready()
		{
			serverClient = GetNode<Network>("/root/Network") as Network;
			serverClient.Connect("CorrectRestorePassword", this, nameof(CorrectRecoverPassword));
			serverClient.Connect("RestorePasswordFail", this, nameof(IncorrectRecoverPassword));
		}
		public void _on_RestoreTextureButton_pressed()
		{
			password = GetParent().GetNode<LineEdit>("RestorePassword/RestorePasswordNinePatchRect/PasswordLineEdit").Text;
			confirmPassword = GetParent().GetNode<LineEdit>("RestorePassword/RestorePasswordNinePatchRect/ConfirmPasswordLineEdit").Text;
			if(ValidateFields())
			{
				Encryption PasswordHasher = new Encryption();
				string hashPassword = PasswordHasher.ComputeSHA256Hash(password);
				serverClient.RestorePassword(email, hashPassword);
			}
		}
		public void _on_CancelTextureButton_pressed()
		{
			GetTree().ChangeScene("res://Scenes/LogIn.tscn");
		}
		public void _on_RestorePasswordCompleteAcceptDialog_confirmed()
		{
			GetTree().ChangeScene("res://Scenes/LogIn.tscn");
		}
		public void CorrectRecoverPassword()
		{
			GetParent().GetNode<AcceptDialog>("RestorePassword/RestorePasswordCompleteAcceptDialog").WindowTitle = ("NOTIFICATION");
			GetParent().GetNode<AcceptDialog>("RestorePassword/RestorePasswordCompleteAcceptDialog").DialogText =("CORRECT_PASSWORD_UPDATE");
			GetParent().GetNode<AcceptDialog>("RestorePassword/RestorePasswordCompleteAcceptDialog").Visible = true;
		}
		public void IncorrectRecoverPassword()
		{
			GetParent().GetNode<AcceptDialog>("RestorePassword/RestorePasswordNotificationAcceptDialog").WindowTitle = ("ERROR");
			GetParent().GetNode<AcceptDialog>("RestorePassword/RestorePasswordNotificationAcceptDialog").DialogText =("ERROR_PASSWORD_UPDATE");
			GetParent().GetNode<AcceptDialog>("RestorePassword/RestorePasswordNotificationAcceptDialog").Visible = true;
		}

		public bool ValidateFields()
		{
			Validation validator = new Validation();
			bool isValid = true;
			if(validator.ValidatePassword(password).Equals(false))
			{
				GetParent().GetNode<AcceptDialog>("RestorePassword/RestorePasswordNotificationAcceptDialog").WindowTitle = ("WARNING");
				GetParent().GetNode<AcceptDialog>("RestorePassword/RestorePasswordNotificationAcceptDialog").DialogText =("VERIFY_PASSWORD");
				GetParent().GetNode<AcceptDialog>("RestorePassword/RestorePasswordNotificationAcceptDialog").Visible = true;
				isValid = false;
			}
			if(confirmPassword.Equals(password).Equals(false))
			{
				GetParent().GetNode<AcceptDialog>("RestorePassword/RestorePasswordNotificationAcceptDialog").WindowTitle =("WARNING");
				GetParent().GetNode<AcceptDialog>("RestorePassword/RestorePasswordNotificationAcceptDialog").DialogText =("VERIFY_CONFIRM_PASSWORD");
				GetParent().GetNode<AcceptDialog>("RestorePassword/RestorePasswordNotificationAcceptDialog").Visible = true;
				isValid = false;
			}
			return isValid;
		}
	}
}