using Godot;
using System;
using OpenCodeChems.Client.Server;
using System.Threading.Tasks;
using OpenCodeChems.Client.Resources;
using static OpenCodeChems.Client.Resources.Objects;

namespace OpenCodeChems.Client.UserInterface
{
	public class AddFriend : Control
	{
		Network serverClient;
		private string usernameActualPlayer = MainMenu.username;
		private string usernameFriend = "";
		private int idProfileActualPlayer = MainMenu.idProfile;
		private int idProfilePlayerFound = 0;
		private const bool STATUS_SEND_FRIEND_REQUEST = false;
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
					if(validator.ValidateUsernameAndNickname(usernameFriend))
					{
						serverClient.UsernameRegister(usernameFriend);
					}
					else
					{
						GetParent().GetNode<AcceptDialog>("AddFriend/AddFriendAcceptDialog").WindowTitle = ("WARNING");
						GetParent().GetNode<AcceptDialog>("AddFriend/AddFriendAcceptDialog").DialogText = ("VERIFY_USERNAME");
						GetParent().GetNode<AcceptDialog>("AddFriend/AddFriendAcceptDialog").Visible = true;
					}
				}
				else
				{
					GetParent().GetNode<AcceptDialog>("AddFriend/AddFriendAcceptDialog").WindowTitle = ("ERROR");
					GetParent().GetNode<AcceptDialog>("AddFriend/AddFriendAcceptDialog").DialogText = ("FRIEND_YOURSELF");
					GetParent().GetNode<AcceptDialog>("AddFriend/AddFriendAcceptDialog").Visible = true;
				}
			}
			else
			{
				GetParent().GetNode<AcceptDialog>("AddFriend/AddFriendAcceptDialog").WindowTitle = ("WARNING");
				GetParent().GetNode<AcceptDialog>("AddFriend/AddFriendAcceptDialog").DialogText = ("VERIFY_EMPTY_FIELDS");
				GetParent().GetNode<AcceptDialog>("AddFriend/AddFriendAcceptDialog").Visible = true;
			}
		}
		
		public void UsernameNotRegistered()
		{
			GetParent().GetNode<AcceptDialog>("AddFriend/AddFriendAcceptDialog").WindowTitle = ("WARNING");
			GetParent().GetNode<AcceptDialog>("AddFriend/AddFriendAcceptDialog").DialogText = ("USERNAME_NOT_FOUND");
			GetParent().GetNode<AcceptDialog>("AddFriend/AddFriendAcceptDialog").Visible = true;
		}
		public void UsernameIsRegistered()
		{
			serverClient.GetProfileByUsername(usernameFriend);
		}	
		public void AddFriendCorrect()
		{
			GetParent().GetNode<AcceptDialog>("AddFriend/AddFriendAcceptDialog").WindowTitle = ("NOTIFICATION");
			GetParent().GetNode<AcceptDialog>("AddFriend/AddFriendAcceptDialog").DialogText =
			("FRIEND_REQUEST_SENT");
			GetParent().GetNode<AcceptDialog>("AddFriend/AddFriendAcceptDialog").Visible = true;
		}
		public void AddFriendNotCorrect()
		{
			GetParent().GetNode<AcceptDialog>("AddFriend/AddFriendAcceptDialog").WindowTitle = ("ERROR");
			GetParent().GetNode<AcceptDialog>("AddFriend/AddFriendAcceptDialog").DialogText =
			("FRIEND_REQUEST_NOT_SENT");
			GetParent().GetNode<AcceptDialog>("AddFriend/AddFriendAcceptDialog").Visible = true;
		}
		public void GetProfileByUsernameComplete()
		{
			if(serverClient.profileByUsernameObtained != null)
			{
				Profile playerFound = serverClient.profileByUsernameObtained;
				idProfilePlayerFound = playerFound.idProfile;
			}
			serverClient.FriendshipExist(idProfileActualPlayer, idProfilePlayerFound);
		}
		public void GetProfileByUsernameFail()
		{
			GetParent().GetNode<AcceptDialog>("AddFriend/AddFriendAcceptDialog").DialogText = ("ERROR_LOADING_PROFILE");
			GetParent().GetNode<AcceptDialog>("AddFriend/AddFriendAcceptDialog").WindowTitle = ("ERROR");
			GetParent().GetNode<AcceptDialog>("AddFriend/AddFriendAcceptDialog").Visible = true;
		}
		public void FriendshipNotRegistered()
		{
			serverClient.AddFriend(idProfileActualPlayer, idProfilePlayerFound, STATUS_SEND_FRIEND_REQUEST);
		}
		public void FriendshipIsRegistered()
		{
			GetParent().GetNode<AcceptDialog>("AddFriend/AddFriendAcceptDialog").WindowTitle = ("WARNING");
			GetParent().GetNode<AcceptDialog>("AddFriend/AddFriendAcceptDialog").DialogText =
			("FRIENDSHIP_EXIST");
			GetParent().GetNode<AcceptDialog>("AddFriend/AddFriendAcceptDialog").Visible = true;
		}

	}
}
