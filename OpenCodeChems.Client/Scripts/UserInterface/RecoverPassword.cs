using Godot;
using OpenCodeChems.Client.Resources;
using System;
using System.Text;

namespace OpenCodeChems.Client.UserInterface
{
    public class RecoverPassword : Control
    { 
        public static string email = "";

        public void _on_RecoverTextureButton_pressed()
        {
            Validation validator = new Validation();
            email = GetParent().GetNode<LineEdit>("RecoverPassword/RecoverPasswordNinePatchRect/EmailLineEdit").Text;
            if(validator.ValidateEmail(email) == false)
            {
                GetParent().GetNode<AcceptDialog>("RecoverPassword/RecoverPasswordAcceptDialog").WindowTitle =("WARNING");
                GetParent().GetNode<AcceptDialog>("RecoverPassword/RecoverPasswordAcceptDialog").DialogText = ("VERIFY_EMAIL");
                GetParent().GetNode<AcceptDialog>("RecoverPassword/RecoverPasswordAcceptDialog").Visible = true;
            }
            else
            {
                GetTree().ChangeScene("res://Scenes/ConfirmRecoverPassword.tscn");
            }
        }
    }
}