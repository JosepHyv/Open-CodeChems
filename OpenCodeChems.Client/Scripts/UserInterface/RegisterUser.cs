using Godot;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using OpenCodeChems.Client.Resources;
using OpenCodeChems.Client.Server;
using System.IO;
using Path = System.IO.Path;
using File = System.IO.File;


public class RegisterUser : Control
{
	Network serverClient;
	int PEER_ID = 1; 
	private AcceptDialog dialogAccept = new AcceptDialog();
	private Task<bool> emailNotRegisteredStatus = Task<bool>.FromResult(false);
	private Task<bool> usernameNotRegisteredStatus = Task<bool>.FromResult(false);
	private Task<bool> nicknameNotRegisteredStatus = Task<bool>.FromResult(false);
	private bool validateRegister = false;
	public static string name = "";
	public static string email = "";
	public static string username = "";
	private string password = "";
	private string confirmPassword = "";
	public static string hashPassword = "";
	public static string nickname = "";
	public override void _Ready()
	{
		serverClient = GetNode<Network>("/root/Network") as Network;
		serverClient.Connect("EmailRegistered", this, nameof(EmailIsRegistered));
		serverClient.Connect("EmailNotRegistered", this, nameof(EmailNotRegistered));
		serverClient.Connect("UsernameRegistered", this, nameof(UsernameIsRegistered));
		serverClient.Connect("UsernameNotRegistered", this, nameof(UsernameNotRegistered));
		serverClient.Connect("NicknameRegistered", this, nameof(NicknameIsRegistered));
		serverClient.Connect("NicknameNotRegistered", this, nameof(NicknameNotRegistered));
	}

	public void _on_CancelTextureButton_pressed()
	{
		GetTree().ChangeScene("res://Scenes/LogIn.tscn");
	}

	public  void _on_RegisterTextureButton_pressed()
	{
		name = GetParent().GetNode<LineEdit>("RegisterUser/BackgroundRegisterNinePatchRect/NameLineEdit").Text;
		email = GetParent().GetNode<LineEdit>("RegisterUser/BackgroundRegisterNinePatchRect/EmailLineEdit").Text;
		username = GetParent().GetNode<LineEdit>("RegisterUser/BackgroundRegisterNinePatchRect/UsernameLineEdit").Text;
		password = GetParent().GetNode<LineEdit>("RegisterUser/BackgroundRegisterNinePatchRect/PasswordLineEdit").Text;
		confirmPassword = GetParent().GetNode<LineEdit>("RegisterUser/BackgroundRegisterNinePatchRect/ConfirmPasswordLineEdit").Text;
		nickname = GetParent().GetNode<LineEdit>("RegisterUser/BackgroundRegisterNinePatchRect/NicknameLineEdit").Text;
		bool noEmptyFields = ValidateEmptyFields();
		bool verifyEmailPassword = ValidateFields();
		if(noEmptyFields == true)
		{
			if(verifyEmailPassword == true)
			{
				serverClient.EmailRegister(email);
				serverClient.UsernameRegister(username);
				serverClient.NicknameRegister(nickname);
				if(validateRegister == true)
				{
					Encryption PasswordHasher = new Encryption();
					hashPassword = PasswordHasher.ComputeSHA256Hash(password);
					GetTree().ChangeScene("res://Scenes/ConfirmRegister.tscn");
				}
			}
		}
		else
		{
			GetParent().GetNode<AcceptDialog>("RegisterUser/RegisterUserDialog").WindowTitle = ("WARNING");
			GetParent().GetNode<AcceptDialog>("RegisterUser/RegisterUserDialog").DialogText = ("VERIFY_EMPTY_FIELDS");
			GetParent().GetNode<AcceptDialog>("RegisterUser/RegisterUserDialog").Visible = true;
		}
		
	}

	public bool ValidateEmptyFields()
	{ 
		bool validFields = false;
		if(!String.IsNullOrWhiteSpace(name) && !String.IsNullOrWhiteSpace(email) && !String.IsNullOrWhiteSpace(username) && !String.IsNullOrWhiteSpace(nickname) && !String.IsNullOrWhiteSpace(password) && !String.IsNullOrWhiteSpace(confirmPassword))
		{
			validFields = true;
		}
		return validFields;
	}
	public bool ValidateFields()
	{
		Validation validator = new Validation();
		bool isValid = true;
		if(validator.ValidateEmail(email) == false)
		{
			GetParent().GetNode<AcceptDialog>("RegisterUser/RegisterUserDialog").WindowTitle = ("WARNING");
			GetParent().GetNode<AcceptDialog>("RegisterUser/RegisterUserDialog").DialogText = ("VERIFY_EMAIL");
			GetParent().GetNode<AcceptDialog>("RegisterUser/RegisterUserDialog").Visible = true;
			isValid = false;
		}
		if(validator.ValidatePassword(password) == false)
		{
			GetParent().GetNode<AcceptDialog>("RegisterUser/RegisterUserDialog").WindowTitle = ("WARNING");
			GetParent().GetNode<AcceptDialog>("RegisterUser/RegisterUserDialog").DialogText = ("VERIFY_PASSWORD");
			GetParent().GetNode<AcceptDialog>("RegisterUser/RegisterUserDialog").Visible = true;
			isValid = false;
		}
		if(validator.ValidateUsernameAndNickname(username) == false)
		{
			GetParent().GetNode<AcceptDialog>("RegisterUser/RegisterUserDialog").WindowTitle = ("WARNING");
			GetParent().GetNode<AcceptDialog>("RegisterUser/RegisterUserDialog").DialogText = ("VERIFY_USERNAME");
			GetParent().GetNode<AcceptDialog>("RegisterUser/RegisterUserDialog").Visible = true;
			isValid = false;
		}
		if(validator.ValidateUsernameAndNickname(nickname) == false)
		{
			GetParent().GetNode<AcceptDialog>("RegisterUser/RegisterUserDialog").WindowTitle = ("WARNING");
			GetParent().GetNode<AcceptDialog>("RegisterUser/RegisterUserDialog").DialogText = ("VERIFY_NICKNAME");
			GetParent().GetNode<AcceptDialog>("RegisterUser/RegisterUserDialog").Visible = true;
			isValid = false;
		}
		if(validator.ValidateName(name) == false)
		{
			GetParent().GetNode<AcceptDialog>("RegisterUser/RegisterUserDialog").WindowTitle = ("WARNING");
			GetParent().GetNode<AcceptDialog>("RegisterUser/RegisterUserDialog").DialogText = ("VERIFY_NAME");
			GetParent().GetNode<AcceptDialog>("RegisterUser/RegisterUserDialog").Visible = true;
			isValid = false;
		}
		if(confirmPassword.Equals(password) == false)
		{
			GetParent().GetNode<AcceptDialog>("RegisterUser/RegisterUserDialog").WindowTitle = ("WARNING");
			GetParent().GetNode<AcceptDialog>("RegisterUser/RegisterUserDialog").DialogText = ("VERIFY_CONFIRM_PASSWORD");
			GetParent().GetNode<AcceptDialog>("RegisterUser/RegisterUserDialog").Visible = true;
			isValid = false;
		}
		return isValid;
	}

	
	public void EmailNotRegistered()
	{
		emailNotRegisteredStatus = Task<bool>.FromResult(true);
		validateRegister = true;
	}
	public void EmailIsRegistered()
	{
		GetParent().GetNode<AcceptDialog>("RegisterUser/RegisterUserDialog").WindowTitle = ("WARNING");
		GetParent().GetNode<AcceptDialog>("RegisterUser/RegisterUserDialog").DialogText = ("EMAIL_REGISTER");
		GetParent().GetNode<AcceptDialog>("RegisterUser/RegisterUserDialog").Visible = true;
		emailNotRegisteredStatus = Task<bool>.FromResult(false);
	}
	public void UsernameNotRegistered()
	{
		usernameNotRegisteredStatus = Task<bool>.FromResult(true);
		validateRegister = true;
	}
	public void UsernameIsRegistered()
	{
		GetParent().GetNode<AcceptDialog>("RegisterUser/RegisterUserDialog").WindowTitle = ("WARNING");
		GetParent().GetNode<AcceptDialog>("RegisterUser/RegisterUserDialog").DialogText = 
		("USERNAME_REGISTER");
		GetParent().GetNode<AcceptDialog>("RegisterUser/RegisterUserDialog").Visible = true;
		usernameNotRegisteredStatus = Task<bool>.FromResult(false);
	}
	public void NicknameNotRegistered()
	{
		nicknameNotRegisteredStatus = Task<bool>.FromResult(true);
		validateRegister = true;
	}
	public void NicknameIsRegistered()
	{
		GetParent().GetNode<AcceptDialog>("RegisterUser/RegisterUserDialog").WindowTitle = ("WARNING");
		GetParent().GetNode<AcceptDialog>("RegisterUser/RegisterUserDialog").DialogText = ("NICKNAME_REGISTER");
		GetParent().GetNode<AcceptDialog>("RegisterUser/RegisterUserDialog").Visible = true;
		usernameNotRegisteredStatus = Task<bool>.FromResult(false);
	}
}
