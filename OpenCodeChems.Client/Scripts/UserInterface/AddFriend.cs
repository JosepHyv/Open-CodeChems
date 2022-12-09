using Godot;
using System;
using OpenCodeChems.Client.Server;
using System.Threading.Tasks;
using OpenCodeChems.Client.Resources;
using static OpenCodeChems.Client.Resources.Objects;

public class AddFriend : Control
{
	Network serverClient;
	private int PEER_ID = 1; 
	private Task<bool> usernameRegisteredStatus = Task<bool>.FromResult(false);
	private Task<bool> friendshipRegisteredStatus = Task<bool>.FromResult(false);
	private Task<bool> addFriendIsCorrect = Task<bool>.FromResult(false);
	private string usernameActualPlayer = MainMenu.username;
	private string usernameFriend = "";
	private int idProfileActualPlayer = MainMenu.idProfile;
	public int idProfilePlayerFound = 0;
	public Profile playerFound;
	private bool STATUS_SEND_FRIEND_REQUEST = false;
 	public override void _Ready()
	{
		serverClient = GetNode<Network>("/root/Network") as Network;
		serverClient.Connect("UsernameRegistered", this, nameof(UsernameIsRegistered));
		serverClient.Connect("UsernameNotRegistered", this, nameof(UsernameNotRegistered));
		serverClient.Connect("CorrectAddFriend", this, nameof(AddFriendCorrect));
		serverClient.Connect("AddFriendFail", this, nameof(AddFriendNotCorrect));
		serverClient.Connect("ProfileByUsernameFound", this, nameof(GetProfileByUsernameComplete));
		serverClient.Connect("ProfileByUsernameNotFound", this, nameof(GetProfileByUsernameFail));
		serverClient.Connect("FriendshipNotRegistered", this, nameof(FriendshipNotRegistered));
		serverClient.Connect("FriendshipRegistered", this, nameof(FriendshipIsRegistered));
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
			if(usernameActualPlayer != usernameFriend)
			{
				if(validator.ValidateUsernameAndNickname(usernameFriend) == true)
				{
					serverClient.UsernameRegister(usernameFriend);
				}
				else
				{
					GetParent().GetNode<AcceptDialog>("AddFriend/AddFriendAcceptDialog").SetTitle("WARNING");
					GetParent().GetNode<AcceptDialog>("AddFriend/AddFriendAcceptDialog").SetText("VERIFY_USERNAME");
					GetParent().GetNode<AcceptDialog>("AddFriend/AddFriendAcceptDialog").Visible = true;
				}
			}
			else
			{
				GetParent().GetNode<AcceptDialog>("AddFriend/AddFriendAcceptDialog").SetTitle("ERROR");
				GetParent().GetNode<AcceptDialog>("AddFriend/AddFriendAcceptDialog").SetText("FRIEND_YOURSELF");
				GetParent().GetNode<AcceptDialog>("AddFriend/AddFriendAcceptDialog").Visible = true;
			}
		}
		else
		{
			GetParent().GetNode<AcceptDialog>("AddFriend/AddFriendAcceptDialog").SetTitle("WARNING");
			GetParent().GetNode<AcceptDialog>("AddFriend/AddFriendAcceptDialog").SetText("VERIFY_EMPTY_FIELDS");
			GetParent().GetNode<AcceptDialog>("AddFriend/AddFriendAcceptDialog").Visible = true;
		}
	}
	
	public void UsernameNotRegistered()
	{
		usernameRegisteredStatus = Task<bool>.FromResult(false);
		GetParent().GetNode<AcceptDialog>("AddFriend/AddFriendAcceptDialog").SetTitle("WARNING");
		GetParent().GetNode<AcceptDialog>("AddFriend/AddFriendAcceptDialog").SetText("USERNAME_NOT_FOUND");
		GetParent().GetNode<AcceptDialog>("AddFriend/AddFriendAcceptDialog").Visible = true;
	}
	public void UsernameIsRegistered()
	{
		usernameRegisteredStatus = Task<bool>.FromResult(true);
		serverClient.GetProfileByUsername(usernameFriend);
	}	
	public void AddFriendCorrect()
	{
		addFriendIsCorrect = Task<bool>.FromResult(true);
		GetParent().GetNode<AcceptDialog>("AddFriend/AddFriendAcceptDialog").SetTitle("NOTIFICATION");
		GetParent().GetNode<AcceptDialog>("AddFriend/AddFriendAcceptDialog").SetText
		("FRIEND_REQUEST_SENT");
		GetParent().GetNode<AcceptDialog>("AddFriend/AddFriendAcceptDialog").Visible = true;
	}
	public void AddFriendNotCorrect()
	{
		GetParent().GetNode<AcceptDialog>("AddFriend/AddFriendAcceptDialog").SetTitle("ERROR");
		GetParent().GetNode<AcceptDialog>("AddFriend/AddFriendAcceptDialog").SetText
		("FRIEND_REQUEST_NOT_SENT");
		GetParent().GetNode<AcceptDialog>("AddFriend/AddFriendAcceptDialog").Visible = true;
		addFriendIsCorrect = Task<bool>.FromResult(false);
	}
	public void GetProfileByUsernameComplete()
	{
		if(serverClient.profileByUsernameObtained != null)
		{
			playerFound = serverClient.profileByUsernameObtained;
			idProfilePlayerFound = playerFound.idProfile;
			string nickname = playerFound.nickname;
			int victories = playerFound.victories;
			int defeats = playerFound.defeats;
			byte [] imageProfile = playerFound.imageProfile;
			string usernameObtained = playerFound.username;
		}
		serverClient.FriendshipExist(idProfileActualPlayer, idProfilePlayerFound);
	}
	public void GetProfileByUsernameFail()
	{
		GetParent().GetNode<AcceptDialog>("AddFriend/AddFriendAcceptDialog").SetText("ERROR_LOADING_PROFILE");
		GetParent().GetNode<AcceptDialog>("AddFriend/AddFriendAcceptDialog").SetTitle("ERROR");
		GetParent().GetNode<AcceptDialog>("AddFriend/AddFriendAcceptDialog").Visible = true;
	}
	public void FriendshipNotRegistered()
	{
		friendshipRegisteredStatus = Task<bool>.FromResult(false);
		serverClient.AddFriend(idProfileActualPlayer, idProfilePlayerFound, STATUS_SEND_FRIEND_REQUEST);
	}
	public void FriendshipIsRegistered()
	{
		friendshipRegisteredStatus = Task<bool>.FromResult(true);
		GetParent().GetNode<AcceptDialog>("AddFriend/AddFriendAcceptDialog").SetTitle("WARNING");
		GetParent().GetNode<AcceptDialog>("AddFriend/AddFriendAcceptDialog").SetText
		("FRIENDSHIP_EXIST");
		GetParent().GetNode<AcceptDialog>("AddFriend/AddFriendAcceptDialog").Visible = true;
	}

}
