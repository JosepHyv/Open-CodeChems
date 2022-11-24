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
using 	System.Drawing.Imaging;

public class RegisterUser : Control
{
	Network serverClient;
	int PEER_ID = 1; 
	AcceptDialog dialogAccept = new AcceptDialog();
	Task<bool> registeredStatus = Task<bool>.FromResult(false);
	Task<bool> emailNotRegisteredStatus = Task<bool>.FromResult(false);
	Task<bool> usernameNotRegisteredStatus = Task<bool>.FromResult(false);
	Task<bool> nicknameNotRegisteredStatus = Task<bool>.FromResult(false);
	int VICTORIES_DEFAULT = 0;
	int DEFEATS_DEFAULT = 0;
	string name = "";
	string email = "";
	string username = "";
	string password = "";
	string confirmPassword = "";
	string nickname = "";
	public override void _Ready()
	{
		serverClient = GetNode<Network>("/root/Network") as Network;
		serverClient.Connect("Registered", this, nameof(RegisteredAccepted));
		serverClient.Connect("RegisteredFail", this, nameof(RegisteredFail));
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
		bool verifyEmailPassword = ValidatePasswordAndEmail();
		bool userExistence = CheckUserExistence(username, email, nickname);
		if(noEmptyFields == true)
		{
			if(verifyEmailPassword == true)
			{
				if(userExistence == true)
				{
					Encryption PasswordHasher = new Encryption();
					string hashPassword = PasswordHasher.ComputeSHA256Hash(password);
					byte [] imageProfile = GetDefaultImage();
					serverClient.RegisterUser(name, email, username, hashPassword, nickname, imageProfile, VICTORIES_DEFAULT, DEFEATS_DEFAULT);
				}
			}
		}
		else
		{
			GetParent().GetNode<AcceptDialog>("RegisterUser/RegisterUserDialog").SetTitle("WARNING");
			GetParent().GetNode<AcceptDialog>("RegisterUser/RegisterUserDialog").SetText("VERIFY_EMPTY_FIELDS");
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
	public bool ValidatePasswordAndEmail()
	{
		Validation validator = new Validation();
		bool isValid = false;
		if(validator.ValidateEmail(email))
		{
			GetParent().GetNode<AcceptDialog>("RegisterUser/RegisterUserDialog").SetTitle("WARNING");
			GetParent().GetNode<AcceptDialog>("RegisterUser/RegisterUserDialog").SetText("VERIFY_EMAIL");
			GetParent().GetNode<AcceptDialog>("RegisterUser/RegisterUserDialog").Visible = true;
			isValid = true;
		}
		if(validator.ValidatePassword(password))
		{
			GetParent().GetNode<AcceptDialog>("RegisterUser/RegisterUserDialog").SetTitle("WARNING");
			GetParent().GetNode<AcceptDialog>("RegisterUser/RegisterUserDialog").SetText("VERIFY_PASSWORD");
			GetParent().GetNode<AcceptDialog>("RegisterUser/RegisterUserDialog").Visible = true;
			isValid = true;
		}
		if(confirmPassword.Equals(password))
		{
			GetParent().GetNode<AcceptDialog>("RegisterUser/RegisterUserDialog").SetTitle("WARNING");
			GetParent().GetNode<AcceptDialog>("RegisterUser/RegisterUserDialog").SetText("VERIFY_CONFIRM_PASSWORD");
			GetParent().GetNode<AcceptDialog>("RegisterUser/RegisterUserDialog").Visible = true;
			isValid = true;
		}
		return isValid;
	}

	public byte[] GetDefaultImage()
	{
		string pathProfileImageDefault = "../../Scenes/Resources/Icons/imagePerfilDefault.jpg";
		FileStream imageProfileFileStream = new FileStream(pathProfileImageDefault, FileMode.OpenOrCreate, FileAccess.ReadWrite);
		Byte[] imageProfileDefault = new Byte[imageProfileFileStream.Length];
		BinaryReader readearToBinary = new BinaryReader(imageProfileFileStream);
		imageProfileDefault = readearToBinary.ReadBytes(Convert.ToInt32(imageProfileFileStream.Length));
		return imageProfileDefault;
	}
	
	public void RegisteredAccepted()
	{
		registeredStatus = Task<bool>.FromResult(true);
		GetParent().GetNode<AcceptDialog>("RegisterUser/RegisterUserDialog").SetTitle("NOTIFICATION");	
		GetParent().GetNode<AcceptDialog>("RegisterUser/RegisterUserDialog").SetText("REGISTER_COMPLETE");
		GetParent().GetNode<AcceptDialog>("RegisterUser/RegisterUserDialog").Visible = true;
		GetTree().ChangeScene("res://Scenes/LogIn.tscn");
	}
	
	public void RegisteredFail()
	{
		registeredStatus = Task<bool>.FromResult(false);
		GetParent().GetNode<AcceptDialog>("RegisterUser/RegisterUserDialog").SetTitle("WARNING");
		GetParent().GetNode<AcceptDialog>("RegisterUser/RegisterUserDialog").SetText("WRONG_REGISTER");
		GetParent().GetNode<AcceptDialog>("RegisterUser/RegisterUserDialog").Visible = true;
	}
	
	public void EmailNotRegistered()
	{
		emailNotRegisteredStatus = Task<bool>.FromResult(true);
	}
	public void EmailIsRegistered()
	{
		emailNotRegisteredStatus = Task<bool>.FromResult(false);
		GetParent().GetNode<AcceptDialog>("RegisterUser/RegisterUserDialog").SetTitle("WARNING");
		GetParent().GetNode<AcceptDialog>("RegisterUser/RegisterUserDialog").SetText("EMAIL_REGISTER");
	}
	public void UsernameNotRegistered()
	{
		usernameNotRegisteredStatus = Task<bool>.FromResult(true);
	}
	public void UsernameIsRegistered()
	{
		usernameNotRegisteredStatus = Task<bool>.FromResult(false);
		GetParent().GetNode<AcceptDialog>("RegisterUser/RegisterUserDialog").SetTitle("WARNING");
		GetParent().GetNode<AcceptDialog>("RegisterUser/RegisterUserDialog").SetText("USERNAME_REGISTER");
	}
	public void NicknameNotRegistered()
	{
		nicknameNotRegisteredStatus = Task<bool>.FromResult(true);
	}
	public void NicknameIsRegistered()
	{
		nicknameNotRegisteredStatus = Task<bool>.FromResult(false);
		GetParent().GetNode<AcceptDialog>("RegisterUser/RegisterUserDialog").SetTitle("WARNING");
		GetParent().GetNode<AcceptDialog>("RegisterUser/RegisterUserDialog").SetText("NICKNAME_REGISTER");
	}

	public bool CheckUserExistence(string username, string email, string nickname)
	{
		serverClient.EmailRegister(email);
		serverClient.UsernameRegister(username);
		serverClient.NicknameRegister(nickname);
		bool statusUserExistence = false;
		if (emailNotRegisteredStatus.Equals(true) && usernameNotRegisteredStatus.Equals(true) && nicknameNotRegisteredStatus.Equals(true))
		{
			statusUserExistence = true;
		}
		return statusUserExistence;
	}


}
