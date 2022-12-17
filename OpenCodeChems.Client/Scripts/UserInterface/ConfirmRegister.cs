using Godot;
using OpenCodeChems.Client.Resources;
using OpenCodeChems.Client.Server;
using System;
using System.Threading.Tasks;

namespace OpenCodeChems.Client.UserInterface
{
    public class ConfirmRegister : Control
    {
        Network serverClient;
        private readonly int VICTORIES_DEFAULT = 0;
        private readonly int DEFEATS_DEFAULT = 0;
        private readonly int IMAGE_PROFILE_DEFAULT = 0;
        private readonly string name = RegisterUser.name;
        private readonly string email = RegisterUser.email;
        private readonly string username = RegisterUser.username;
        private readonly string hashPassword = RegisterUser.hashPassword;
        private readonly string nickname = RegisterUser.nickname;
        private int codeRegistration = 0;
        private const string BODY_WITHOUT_CODE_EMAIL = "Your code for registration is: ";
        private const string SUBJECT_EMAIL = "Complete your register to OpenCode Chems";
        Random newRandom = new Random();

        public override void _Ready()
        {
            serverClient = GetNode<Network>("/root/Network") as Network;
            serverClient.Connect("Registered", this, nameof(RegisteredAccepted));
            serverClient.Connect("RegisteredFail", this, nameof(RegisteredFail));
            serverClient.Connect("EmailIsSent", this, nameof(EmailSent));
            codeRegistration = newRandom.Next(10000, 99999);
            string bodyEmail = BODY_WITHOUT_CODE_EMAIL + codeRegistration.ToString();
            serverClient.SendEmail(email, SUBJECT_EMAIL, bodyEmail);
        }

        public void _on_AcceptTextureButton_pressed()
        {
            string codeProvided = GetParent().GetNode<LineEdit>("ConfirmRegister/ConfirmRegisterNinePatchRect/ConfirmRegisterLineEdit").Text;
            if(validateCode(codeProvided) == true)
            {
                if(codeProvided == codeRegistration.ToString())
                {
                    serverClient.RegisterUser(name, email, username, hashPassword, nickname, IMAGE_PROFILE_DEFAULT, VICTORIES_DEFAULT, DEFEATS_DEFAULT);
                }
                else
                {
                    GetParent().GetNode<AcceptDialog>("ConfirmRegister/ConfirmRegisterNotificationAcceptDialog").WindowTitle = ("NOTIFICATION");	
                    GetParent().GetNode<AcceptDialog>("ConfirmRegister/ConfirmRegisterNotificationAcceptDialog").DialogText =("ERROR_CODE");
                    GetParent().GetNode<AcceptDialog>("ConfirmRegister/ConfirmRegisterNotificationAcceptDialog").Visible = true;
                }
            }
        }
        public void _on_CancelTextureButton_pressed()
        {
            GetTree().ChangeScene("res://Scenes/RegisterUser.tscn");
        }
        public void _on_ConfirmRegisterCompleteAcceptDialog_confirmed()
        {
            GetTree().ChangeScene("res://Scenes/LogIn.tscn");
        }
        public void _on_NoReciveCodeTextureButton_pressed()
        {
            codeRegistration = newRandom.Next(10000, 99999);
            string bodyEmail = BODY_WITHOUT_CODE_EMAIL + codeRegistration.ToString();
            serverClient.SendEmail(email, SUBJECT_EMAIL, bodyEmail);
        }
        public void RegisteredAccepted()
        {
            GetParent().GetNode<AcceptDialog>("ConfirmRegister/ConfirmRegisterCompleteAcceptDialog").WindowTitle =("NOTIFICATION");	
            GetParent().GetNode<AcceptDialog>("ConfirmRegister/ConfirmRegisterCompleteAcceptDialog").DialogText =("REGISTER_COMPLETE");
            GetParent().GetNode<AcceptDialog>("ConfirmRegister/ConfirmRegisterCompleteAcceptDialog").Visible = true;
        }
        
        public void RegisteredFail()
        {
            GetParent().GetNode<AcceptDialog>("ConfirmRegister/ConfirmRegisterNotificationAcceptDialog").WindowTitle = ("WARNING");
            GetParent().GetNode<AcceptDialog>("ConfirmRegister/ConfirmRegisterNotificationAcceptDialog").DialogText =("WRONG_REGISTER");
            GetParent().GetNode<AcceptDialog>("ConfirmRegister/ConfirmRegisterNotificationAcceptDialog").Visible = true;
        }
        public void EmailSent()
        {
            GetParent().GetNode<AcceptDialog>("ConfirmRegister/ConfirmRegisterNotificationAcceptDialog").WindowTitle =("NOTIFICATION");
            GetParent().GetNode<AcceptDialog>("ConfirmRegister/ConfirmRegisterNotificationAcceptDialog").DialogText = ("EMAIL_SENT");
            GetParent().GetNode<AcceptDialog>("ConfirmRegister/ConfirmRegisterNotificationAcceptDialog").Visible = true;
        }
        public bool validateCode(string code)
        {
            bool isValid = true;
            Validation validator = new Validation();
            if(String.IsNullOrWhiteSpace(code))
            {
                GetParent().GetNode<AcceptDialog>("ConfirmRegister/ConfirmRegisterNotificationAcceptDialog").WindowTitle = ("WARNING");
                GetParent().GetNode<AcceptDialog>("ConfirmRegister/ConfirmRegisterNotificationAcceptDialog").DialogText =("VERIFY_EMPTY_FIELDS");
                GetParent().GetNode<AcceptDialog>("ConfirmRegister/ConfirmRegisterNotificationAcceptDialog").Visible = true;
                isValid = false;
            }
            if(validator.ValidateCodeRegistration(code) == false)
            {
                GetParent().GetNode<AcceptDialog>("ConfirmRegister/ConfirmRegisterNotificationAcceptDialog").WindowTitle = ("WARNING");
                GetParent().GetNode<AcceptDialog>("ConfirmRegister/ConfirmRegisterNotificationAcceptDialog").DialogText = ("ONLY_NUMBERS");
                GetParent().GetNode<AcceptDialog>("ConfirmRegister/ConfirmRegisterNotificationAcceptDialog").Visible = true;
                isValid = false;
            }
            return isValid;
        }
    }
}