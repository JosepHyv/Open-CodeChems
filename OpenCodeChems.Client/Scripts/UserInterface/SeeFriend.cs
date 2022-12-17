using Godot;
using System;
using OpenCodeChems.Client.Server;
using OpenCodeChems.Client.UserInterface;
using static OpenCodeChems.Client.Resources.Objects;
using System.Threading.Tasks;
using OpenCodeChems.Client.Resources;

namespace OpenCodeChems.Client.UserInterface
{
	public class SeeFriend : Control
	{
		Network serverClient;
		private readonly string nicknameFriend = MainMenu.nicknameFriend;
		private string nicknameFriendFound = "";
		private int victories = 0;
		private int defeats = 0;
		private int idProfileFound = 0;
		private readonly int idProfileActualPlayer = MainMenu.idProfile;
		private Profile profileObtained = null;
		private const bool STATUS_DELETE_FRIEND = true;
		private Task<bool> deleteFriendIsCorrect = Task<bool>.FromResult(false);
		public static string pathImageProfile = "";
		private readonly ImageTexture textureImageProfile = new ImageTexture(); 
		private readonly Image image = new Image();
		public override void _Ready()
		{
			serverClient = GetNode<Network>("/root/Network") as Network;
			serverClient.Connect("ProfileByNicknameFound", this, nameof(GetProfileByNicknameComplete));
			serverClient.Connect("ProfileByNicknameNotFound", this, nameof(GetProfileByNicknameFail));
			serverClient.GetProfileByNickname(nicknameFriend);
			serverClient.Connect("CorrectDeleteFriend", this, nameof(DeleteFriendCorrect));
			serverClient.Connect("DeleteFriendFail", this, nameof(DeleteFriendNotCorrect));
		}
		public void _on_CancelTextureButton_pressed()
		{
			GetTree().ChangeScene("res://Scenes/MainMenu.tscn");
		}
		public void _on_DeleteFriendTextureButton_pressed()
		{
			GetParent().GetNode<AcceptDialog>("SeeFriend/DeleteFriendConfirmationDialog").DialogText = ("CONFIRMATION_DELETE_FRIEND");
			GetParent().GetNode<AcceptDialog>("SeeFriend/DeleteFriendConfirmationDialog").WindowTitle = ("WARNING");
			GetParent().GetNode<AcceptDialog>("SeeFriend/DeleteFriendConfirmationDialog").Visible = true;
		}
		public void _on_DeleteFriendConfirmationDialog_confirmed()
		{
			serverClient.DeleteFriend(idProfileActualPlayer, idProfileFound, STATUS_DELETE_FRIEND);
		}
		public void _on_DeleteFriendAcceptDialog_confirmed()
		{
			GetTree().ChangeScene("res://Scenes/MainMenu.tscn");
		}
		public void GetProfileByNicknameComplete()
		{
			if(serverClient.profileByNicknameObtained != null)
			{
				profileObtained = serverClient.profileByNicknameObtained;
				idProfileFound = profileObtained.idProfile;
				nicknameFriendFound = profileObtained.nickname;
				victories = profileObtained.victories;
				defeats = profileObtained.defeats;
				int imageProfile = profileObtained.imageProfile;
				string username = profileObtained.username;
				pathImageProfile = GetImageProfilePath(imageProfile);
				image.Load(pathImageProfile);
				textureImageProfile.CreateFromImage(image);
				GetParent().GetNode<TextureButton>("SeeFriend/SeeFriendNinePatchRect/ImageProfileTextureButton").TextureNormal =(textureImageProfile);
			}
			GetParent().GetNode<Label>("SeeFriend/SeeFriendNinePatchRect/NicknameFriendLabel").Text = (nicknameFriendFound);
			GetParent().GetNode<Label>("SeeFriend/SeeFriendNinePatchRect/VictoriesLabel").Text = (victories.ToString());
			GetParent().GetNode<Label>("SeeFriend/SeeFriendNinePatchRect/DefeatsLabel").Text = (defeats.ToString());

		}
		public void GetProfileByNicknameFail()
		{
			GetParent().GetNode<AcceptDialog>("SeeFriend/DeleteFriendAcceptDialog").DialogText = ("ERROR_LOADING_PROFILE");
			GetParent().GetNode<AcceptDialog>("SeeFriend/DeleteFriendAcceptDialog").WindowTitle = ("ERROR");
			GetParent().GetNode<AcceptDialog>("SeeFriend/DeleteFriendAcceptDialog").Visible = true;
		}

		public void DeleteFriendCorrect()
		{
			GetParent().GetNode<AcceptDialog>("SeeFriend/DeleteFriendAcceptDialog").DialogText = ("DELETE_FRIEND_CORRECT");
			GetParent().GetNode<AcceptDialog>("SeeFriend/DeleteFriendAcceptDialog").WindowTitle = ("NOTIFICATION");
			GetParent().GetNode<AcceptDialog>("SeeFriend/DeleteFriendAcceptDialog").Visible = true;
			deleteFriendIsCorrect = Task<bool>.FromResult(true);
		}
		public void DeleteFriendNotCorrect()
		{
			GetParent().GetNode<AcceptDialog>("SeeFriend/DeleteFriendAcceptDialog").DialogText = ("ERROR_DELETE_FRIEND");
			GetParent().GetNode<AcceptDialog>("SeeFriend/DeleteFriendAcceptDialog").WindowTitle = ("ERROR");
			GetParent().GetNode<AcceptDialog>("SeeFriend/DeleteFriendAcceptDialog").Visible = true;
			deleteFriendIsCorrect = Task<bool>.FromResult(false);
		}
		public string GetImageProfilePath(int imageProfile)
		{
			switch (imageProfile) 
			{
				case Constants.IMAGE_PROFILE_DEFAULT:
				pathImageProfile = "res://Scenes/Resources/ImagesProfile/ImagePerfilDefault.jpg";
				break;
				case Constants.IMAGE_PROFILE_CHEMS_GAMER:
				pathImageProfile = "res://Scenes/Resources/ImagesProfile/chemsGamer.jpg";
				break;
				case Constants.IMAGE_PROFILE_CHEMS_CHRISTMAS:
				pathImageProfile = "res://Scenes/Resources/ImagesProfile/chemsNavidad.jpg";
				break;
				case Constants.IMAGE_PROFILE_DRAVEN:
				pathImageProfile = "res://Scenes/Resources/ImagesProfile/draven.jpeg";
				break;
				case Constants.IMAGE_PROFILE_KITTY_MINECRAFT:
				pathImageProfile = "res://Scenes/Resources/ImagesProfile/kittyMinecraft.jpeg";
				break;
				case Constants.IMAGE_PROFILE_LINK:
				pathImageProfile = "res://Scenes/Resources/ImagesProfile/link.jpeg";
				break;
				case Constants.IMAGE_PROFILE_MECH:
				pathImageProfile = "res://Scenes/Resources/ImagesProfile/mech.png";
				break;
				case Constants.IMAGE_PROFILE_MECH_CHIVA:
				pathImageProfile = "res://Scenes/Resources/ImagesProfile/mechsChivahermano.png";
				break;
				case Constants.IMAGE_PROFILE_COLD_TEAM:
				pathImageProfile = "res://Scenes/Resources/ImagesProfile/teamFrio.jpg";
				break;
				case Constants.IMAGE_PROFILE_WINDOWS:
				pathImageProfile = "res://Scenes/Resources/ImagesProfile/windows.jpeg";
				break;
			}
			return pathImageProfile;
		}

	}
}