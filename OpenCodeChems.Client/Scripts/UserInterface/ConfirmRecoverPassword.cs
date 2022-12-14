using Godot;
using OpenCodeChems.Client.Resources;
using OpenCodeChems.Client.Server;
using System;

public class ConfirmRecoverPassword : Control
{
    Network serverClient;
	int PEER_ID = 1;
    private int codeRegistration = 0;
    public static string email = RecoverPassword.email;
    private string BODY_WITHOUT_CODE_EMAIL = "Your code for recover your password is: ";
	private string SUBJECT_EMAIL = "Recover your password of OpenCode Chems account";
    Random newRandom = new Random();
    public override void _Ready()
    {
        serverClient = GetNode<Network>("/root/Network") as Network;
        serverClient.Connect("EmailIsSent", this, nameof(EmailSent));
        codeRegistration = newRandom.Next(10000, 99999);
        string bodyEmail = BODY_WITHOUT_CODE_EMAIL + codeRegistration.ToString();
        serverClient.SendEmail(email, SUBJECT_EMAIL, bodyEmail);
    }

    public void _on_NoReciveCodeTextureButton_pressed()
    {
        codeRegistration = newRandom.Next(10000, 99999);
        string bodyEmail = BODY_WITHOUT_CODE_EMAIL + codeRegistration.ToString();
        serverClient.SendEmail(email, SUBJECT_EMAIL, bodyEmail);
    }
    public void _on_CancelTextureButton_pressed()
    {
        GetTree().ChangeScene("res://Scenes/RecoverPassword.tscn");
    }
    public void _on_AcceptTextureButton_pressed()
    {
        string codeProvided = GetParent().GetNode<LineEdit>("ConfirmRecoverPassword/ConfirmRecoverNinePatchRect/ConfirmRecoverLineEdit").Text;
        if(validateCode(codeProvided) == true)
        {
            if(codeProvided == codeRegistration.ToString())
            {
                GetTree().ChangeScene("res://Scenes/RestorePassword.tscn");
            }
            else
            {
                GetParent().GetNode<AcceptDialog>("ConfirmRecoverPassword/ConfirmRecoverNotificationAcceptDialog").SetTitle("NOTIFICATION");	
                GetParent().GetNode<AcceptDialog>("ConfirmRecoverPassword/ConfirmRecoverNotificationAcceptDialog").SetText("ERROR_CODE");
                GetParent().GetNode<AcceptDialog>("ConfirmRecoverPassword/ConfirmRecoverNotificationAcceptDialog").Visible = true;
            }
        }
    }   
    public void EmailSent()
	{
		GetParent().GetNode<AcceptDialog>("ConfirmRecoverPassword/ConfirmRecoverNotificationAcceptDialog").SetTitle("NOTIFICATION");
		GetParent().GetNode<AcceptDialog>("ConfirmRecoverPassword/ConfirmRecoverNotificationAcceptDialog").SetText("EMAIL_SENT");
		GetParent().GetNode<AcceptDialog>("ConfirmRecoverPassword/ConfirmRecoverNotificationAcceptDialog").Visible = true;
	}
    public bool validateCode(string code)
    {
        bool isValid = true;
        Validation validator = new Validation();
        if(String.IsNullOrWhiteSpace(code))
        {
            GetParent().GetNode<AcceptDialog>("ConfirmRecoverPassword/ConfirmRecoverNotificationAcceptDialog").SetTitle("WARNING");
		    GetParent().GetNode<AcceptDialog>("ConfirmRecoverPassword/ConfirmRecoverNotificationAcceptDialog").SetText("VERIFY_EMPTY_FIELDS");
		    GetParent().GetNode<AcceptDialog>("ConfirmRecoverPassword/ConfirmRecoverNotificationAcceptDialog").Visible = true;
            isValid = false;
        }
        if(validator.ValidateCodeRegistration(code) == false)
        {
            GetParent().GetNode<AcceptDialog>("ConfirmRecoverPassword/ConfirmRecoverNotificationAcceptDialog").SetTitle("WARNING");
            GetParent().GetNode<AcceptDialog>("ConfirmRecoverPassword/ConfirmRecoverNotificationAcceptDialog").SetText("ONLY_NUMBERS");
            GetParent().GetNode<AcceptDialog>("ConfirmRecoverPassword/ConfirmRecoverNotificationAcceptDialog").Visible = true;
            isValid = false;
        }
        return isValid;
    }
}
