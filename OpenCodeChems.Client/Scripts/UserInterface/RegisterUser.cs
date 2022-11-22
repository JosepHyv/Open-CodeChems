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
	AcceptDialog dialogAccept = new AcceptDialog();
	Task<bool> registeredStatus = Task<bool>.FromResult(false);
	public override void _Ready()
	{
		serverClient = GetNode<Network>("/root/Network") as Network;
		serverClient.Connect("Registered", this, nameof(RegisteredAccepted));
		serverClient.Connect("RegisteredFail", this, nameof(RegisteredFail));
	}

	public void _on_CancelTextureButton_pressed()
	{
		GetTree().ChangeScene("res://Scenes/LogIn.tscn");
	}

	public async void _on_RegisterTextureButton_pressed()
	{
		GD.Print("Precionaon el Register Texture Buton");
		string name = GetParent().GetNode<LineEdit>("RegisterUser/BackgroundRegisterNinePatchRect/NameLineEdit").Text;
		string email = GetParent().GetNode<LineEdit>("RegisterUser/BackgroundRegisterNinePatchRect/EmailLineEdit").Text;
		string username = GetParent().GetNode<LineEdit>("RegisterUser/BackgroundRegisterNinePatchRect/UsernameLineEdit").Text;
		string password = GetParent().GetNode<LineEdit>("RegisterUser/BackgroundRegisterNinePatchRect/PasswordLineEdit").Text;
		string confirmPassword = GetParent().GetNode<LineEdit>("RegisterUser/BackgroundRegisterNinePatchRect/ConfirmPasswordLineEdit").Text;
		Validation validator = new Validation();
		if(validator.ValidateEmail(email) && validator.ValidatePassword(password) && confirmPassword.Equals(password) && !String.IsNullOrWhiteSpace(username) && !String.IsNullOrWhiteSpace(name))
		{
			Encryption PasswordHasher = new Encryption();
			string hashPassword = PasswordHasher.ComputeSHA256Hash(password);
			byte[] imageProfile = null;
			int victories = 0;
			int defaults = 0;
			serverClient.RegisterUser(name, email, username, hashPassword, imageProfile, victories, defaults);
			await registeredStatus;
			if (registeredStatus.Result == true)
			{
				GD.Print("REGISTER_SUCCESFULLY");	
			}
			else
			{
				GD.Print("WRONG_REGISTER");
			}
		}
		else
		{
			GD.Print("VERIFY_FIELDS");
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
