using Godot;
using OpenCodeChems.Client.Resources;
using OpenCodeChems.Client.Server;
using System;

namespace OpenCodeChems.Client.UserInterface
{
    public class ConfirmRecoverPassword : Control
    {
        Network serverClient;
        private int codeRegistration = 0;
        public static string email = RecoverPassword.email;
        private const string BODY_WITHOUT_CODE_EMAIL = "Your code for recover your password is: ";
        private const string SUBJECT_EMAIL = "Recover your password of OpenCode Chems account";
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
            if(validateCode(codeProvided))
            {
                if(codeProvided == codeRegistration.ToString())
                {
                    GetTree().ChangeScene("res://Scenes/RestorePassword.tscn");
                }
                else
                {
                    GetParent().GetNode<AcceptDialog>("ConfirmRecoverPassword/ConfirmRecoverNotificationAcceptDialog").WindowTitle = ("NOTIFICATION");	
                    GetParent().GetNode<AcceptDialog>("ConfirmRecoverPassword/ConfirmRecoverNotificationAcceptDialog").DialogText = ("ERROR_CODE");
                    GetParent().GetNode<AcceptDialog>("ConfirmRecoverPassword/ConfirmRecoverNotificationAcceptDialog").Visible = true;
                }
            }
        }   
        public void EmailSent()
        {
            GetParent().GetNode<AcceptDialog>("ConfirmRecoverPassword/ConfirmRecoverNotificationAcceptDialog").WindowTitle = ("NOTIFICATION");
            GetParent().GetNode<AcceptDialog>("ConfirmRecoverPassword/ConfirmRecoverNotificationAcceptDialog").DialogText = ("EMAIL_SENT");
            GetParent().GetNode<AcceptDialog>("ConfirmRecoverPassword/ConfirmRecoverNotificationAcceptDialog").Visible = true;
        }
        public bool validateCode(string code)
        {
            bool isValid = true;
            Validation validator = new Validation();
            if(String.IsNullOrWhiteSpace(code))
            {
                GetParent().GetNode<AcceptDialog>("ConfirmRecoverPassword/ConfirmRecoverNotificationAcceptDialog").WindowTitle = ("WARNING");
                GetParent().GetNode<AcceptDialog>("ConfirmRecoverPassword/ConfirmRecoverNotificationAcceptDialog").DialogText = ("VERIFY_EMPTY_FIELDS");
                GetParent().GetNode<AcceptDialog>("ConfirmRecoverPassword/ConfirmRecoverNotificationAcceptDialog").Visible = true;
                isValid = false;
            }
            if(validator.ValidateCodeRegistration(code).Equals(false))
            {
                GetParent().GetNode<AcceptDialog>("ConfirmRecoverPassword/ConfirmRecoverNotificationAcceptDialog").WindowTitle = ("WARNING");
                GetParent().GetNode<AcceptDialog>("ConfirmRecoverPassword/ConfirmRecoverNotificationAcceptDialog").DialogText = ("ONLY_NUMBERS");
                GetParent().GetNode<AcceptDialog>("ConfirmRecoverPassword/ConfirmRecoverNotificationAcceptDialog").Visible = true;
                isValid = false;
            }
            return isValid;
        }
    }
}