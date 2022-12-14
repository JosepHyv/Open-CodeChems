using Godot;
using OpenCodeChems.Client.Resources;
using OpenCodeChems.Client.Server;
using System;
using System.Threading.Tasks;

public class RestorePassword : Control
{
    Network serverClient;
	int PEER_ID = 1; 
    private string password = "";
    private string confirmPassword = "";
    private string hashPassword = "";
    private string email = ConfirmRecoverPassword.email;
    Task<bool> recoverPasswordIsCorrect = Task<bool>.FromResult(false);
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
        if(ValidateFields() == true)
        {
            Encryption PasswordHasher = new Encryption();
			hashPassword = PasswordHasher.ComputeSHA256Hash(password);
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
		recoverPasswordIsCorrect = Task<bool>.FromResult(true);
		GetParent().GetNode<AcceptDialog>("RestorePassword/RestorePasswordCompleteAcceptDialog").SetTitle("NOTIFICATION");
		GetParent().GetNode<AcceptDialog>("RestorePassword/RestorePasswordCompleteAcceptDialog").SetText("CORRECT_PASSWORD_UPDATE");
		GetParent().GetNode<AcceptDialog>("RestorePassword/RestorePasswordCompleteAcceptDialog").Visible = true;
	}
	public void IncorrectRecoverPassword()
	{
		recoverPasswordIsCorrect = Task<bool>.FromResult(true);
		GetParent().GetNode<AcceptDialog>("RestorePassword/RestorePasswordNotificationAcceptDialog").SetTitle("ERROR");
		GetParent().GetNode<AcceptDialog>("RestorePassword/RestorePasswordNotificationAcceptDialog").SetText("ERROR_PASSWORD_UPDATE");
		GetParent().GetNode<AcceptDialog>("RestorePassword/RestorePasswordNotificationAcceptDialog").Visible = true;
	}

    public bool ValidateFields()
	{
		Validation validator = new Validation();
		bool isValid = true;
		if(validator.ValidatePassword(password) == false)
		{
			GetParent().GetNode<AcceptDialog>("RestorePassword/RestorePasswordNotificationAcceptDialog").SetTitle("WARNING");
			GetParent().GetNode<AcceptDialog>("RestorePassword/RestorePasswordNotificationAcceptDialog").SetText("VERIFY_PASSWORD");
			GetParent().GetNode<AcceptDialog>("RestorePassword/RestorePasswordNotificationAcceptDialog").Visible = true;
			isValid = false;
		}
		if(confirmPassword.Equals(password) == false)
		{
			GetParent().GetNode<AcceptDialog>("RestorePassword/RestorePasswordNotificationAcceptDialog").SetTitle("WARNING");
			GetParent().GetNode<AcceptDialog>("RestorePassword/RestorePasswordNotificationAcceptDialog").SetText("VERIFY_CONFIRM_PASSWORD");
			GetParent().GetNode<AcceptDialog>("RestorePassword/RestorePasswordNotificationAcceptDialog").Visible = true;
			isValid = false;
		}
		return isValid;
	}
}
