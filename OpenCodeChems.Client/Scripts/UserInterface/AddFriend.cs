using Godot;
using System;
using OpenCodeChems.Client.Server;
using System.Threading.Tasks;
using OpenCodeChems.Client.Resources;

public class AddFriend : Control
{
	Network serverClient;
	int PEER_ID = 1; 
	private Task<bool> usernameNotRegisteredStatus = Task<bool>.FromResult(false);
	bool validateRegister = true;
	private string username = MainMenu.username;
	private string usernameFriend = "";
 	public override void _Ready()
	{
		serverClient = GetNode<Network>("/root/Network") as Network;
		serverClient.Connect("UsernameRegistered", this, nameof(UsernameIsRegistered));
		serverClient.Connect("UsernameNotRegistered", this, nameof(UsernameNotRegistered));
	}



	public void _on_CancelTextureButton_pressed()
	{
		GetTree().ChangeScene("res://Scenes/MainMenu.tscn");
	}
	public void _on_AddFriendTextureButton_pressed()
	{
		usernameFriend = GetParent().GetNode<LineEdit>("AddFriend/AddFriendNinePatchRect/UsernameLineEdit").Text;
		Validation validator = new Validation();
		if(!String.IsNullOrWhiteSpace(usernameFriend))
		{
			if(validator.ValidateUsernameAndNickname(usernameFriend) == true)
			{
				serverClient.UsernameRegister(usernameFriend);
				if(validateRegister == true)
				{
					
				}
			}
		}
	}
	
	public void UsernameNotRegistered()
	{
		usernameNotRegisteredStatus = Task<bool>.FromResult(true);
		GetParent().GetNode<AcceptDialog>("RegisterUser/RegisterUserDialog").SetTitle("WARNING");
		GetParent().GetNode<AcceptDialog>("RegisterUser/RegisterUserDialog").SetText
		("USERNAME_NOT_FOUND");
		GetParent().GetNode<AcceptDialog>("RegisterUser/RegisterUserDialog").Visible = true;
		validateRegister = true;
	}
	public void UsernameIsRegistered()
	{
		usernameNotRegisteredStatus = Task<bool>.FromResult(false);
	}
}
