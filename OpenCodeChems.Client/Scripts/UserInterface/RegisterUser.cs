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
using OpenCodeChems.Objects;

public class RegisterUser : Control
{
	Network serverClient;
	int PEER_ID = 1; 
	AcceptDialog dialogAccept = new AcceptDialog();
	Task<bool> registeredStatus = Task<bool>.FromResult(false);
	public override void _Ready()
	{
		serverClient = GetNode<Network>("/root/Network") as Network;
		serverClient.ConnectToServer();
		serverClient.Connect("Registered", this, nameof(RegisteredAccepted));
		serverClient.Connect("RegisteredFail", this, nameof(RegisteredFail));
	}

	public void _on_CancelTextureButton_pressed()
	{
		GetTree().ChangeScene("res://Scenes/LogIn.tscn");
	}

	public async void _on_RegisterTextureButton_pressed()
	{
		string name = GetParent().GetNode<LineEdit>("RegisterUser/BackgroundRegisterNinePatchRect/NameLineEdit").Text;
		string email = GetParent().GetNode<LineEdit>("RegisterUser/BackgroundRegisterNinePatchRect/EmailLineEdit").Text;
		string username = GetParent().GetNode<LineEdit>("RegisterUser/BackgroundRegisterNinePatchRect/UsernameLineEdit").Text;
		string nickname = GetParent().GetNode<LineEdit>("RegisterUser/BackgroundRegisterNinePatchRect/NicknameLineEdit").Text;
		string password = GetParent().GetNode<LineEdit>("RegisterUser/BackgroundRegisterNinePatchRect/PasswordLineEdit").Text;
		string confirmPassword = GetParent().GetNode<LineEdit>("RegisterUser/BackgroundRegisterNinePatchRect/ConfirmPasswordLineEdit").Text;
		Validation validator = new Validation();
		if(validator.ValidateEmail(email) && validator.ValidatePassword(password) && confirmPassword.Equals(password) && !String.IsNullOrWhiteSpace(username) && !String.IsNullOrWhiteSpace(nickname) && !String.IsNullOrWhiteSpace(name))
		{
			Encryption PasswordHasher = new Encryption();
			string hashPassword = PasswordHasher.ComputeSHA256Hash(password);
			byte[] imageProfile = GetDefaultImage();
			User newUser = new User();
			newUser.username = username;
			newUser.name = name;
			newUser.email = email;
			newUser.password = password;
			Profile newProfile = new Profile();
			newProfile.defaults = 0;
			newProfile.nickname = nickname;
			newProfile.username = username;
			newProfile.imageProfile = imageProfile;
			newProfile.victories = 0;
			serverClient.RegisterUser(newUser, newProfile);
			await registeredStatus;
			if (registeredStatus.Result)
			{
				dialogAccept.SetText("REGISTER_SUCCESFULLY");
				dialogAccept.PopupCentered();
				dialogAccept.Visible = true;
			}
			else
			{
				dialogAccept.SetText("WRONG_REGISTER");
				dialogAccept.PopupCentered();
				dialogAccept.Visible = true;
			}
		}
		else
		{
			dialogAccept.SetText("VERIFY_FIELDS");
			dialogAccept.PopupCentered();
			dialogAccept.Visible = true;
		}
		
	}

	public byte[] GetDefaultImage()
	{
		var profilePicturePath = Path.GetDirectoryName("../../Icons/imagePerfilDefault.png");
		byte[] imageProfile = File.ReadAllBytes(profilePicturePath);
		return imageProfile;
	}
	
	public void RegisteredAccepted()
	{
		registeredStatus = Task<bool>.FromResult(true);
	}
	
	public void RegisteredFail()
	{
		registeredStatus = Task<bool>.FromResult(false);
	}

}
