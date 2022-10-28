using Godot;
using System;
using OpenCodeChems.Client.Resources;

public class RegisterUser : Control
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
    public void _on_CancelTextureButton_pressed()
    {
        GetTree().ChangeScene("res://Scenes/LogIn.tscn");
    }

    public void _on_RegisterTextureButton_pressed()
    {
        GD.Print("Registrando");
        string name = GetParent().GetNode<LineEdit>("RegisterUser/BackgroundRegisterNinePatchRect/NameLineEdit").Text;
        string email = GetParent().GetNode<LineEdit>("RegisterUser/BackgroundRegisterNinePatchRect/EmailLineEdit").Text;
        string userName = GetParent().GetNode<LineEdit>("RegisterUser/BackgroundRegisterNinePatchRect/UsernameLineEdit").Text;
        string playerName = GetParent().GetNode<LineEdit>("RegisterUser/BackgroundRegisterNinePatchRect/NicknameLineEdit").Text;
        string password = GetParent().GetNode<LineEdit>("RegisterUser/BackgroundRegisterNinePatchRect/PasswordLineEdit").Text;
        string confirmPassword =
            GetParent().GetNode<LineEdit>("RegisterUser/BackgroundRegisterNinePatchRect/ConfirmPasswordLineEdit").Text;
        Validation validator = new Validation();
        if(validator.ValidateEmail(email) && validator.ValidatePassword(password) && confirmPassword.Equals(password))
        {
            
        }
        else
        {
            GD.Print("Validar los datos porfi");
        }
        
    }

}
