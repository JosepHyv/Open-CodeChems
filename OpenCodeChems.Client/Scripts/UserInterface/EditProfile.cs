using Godot;
using System;
using OpenCodeChems.Client.Server;
using static OpenCodeChems.Client.Resources.Objects;
using System.Threading.Tasks;
using OpenCodeChems.Client.Resources;
using System.IO;

namespace OpenCodeChems.Client.UserInterface
{
	public class EditProfile : Control
	{
		
		private string username = MainMenu.username;
		Network serverClient;
		private bool validateOldPassword = false;
		private string oldPassword = "";
		private string newPassword = "";
		private string confirmPassword = "";
		private string newNickname = "";
		private string pathImageProfile = "";
		int imageProfile = MainMenu.imageProfile;
		private readonly ImageTexture textureImageProfile = new ImageTexture(); 
		private readonly Image image = new Image();
		public override void _Ready()
		{
			serverClient = GetNode<Network>("/root/Network") as Network;
			serverClient.Connect("CorrectOldPassword", this, nameof(CorrectPassword));
			serverClient.Connect("IncorrectOldPassword", this, nameof(IncorrectPassword));
			serverClient.Connect("CorrectEditPassword", this, nameof(CorrectEditPassword));
			serverClient.Connect("EditPasswordFail", this, nameof(IncorrectEditPassword));
			serverClient.Connect("CorrectEditNickname", this, nameof(CorrectEditNickname));
			serverClient.Connect("EditNicknameFail", this, nameof(IncorrectEditNickname));
			pathImageProfile = GetImageProfilePath(imageProfile);
			image.Load(pathImageProfile);
			textureImageProfile.CreateFromImage(image);
			GetParent().GetNode<TextureButton>("EditProfile/EditProfileNinePatchRect/ProfileInformationTransparentFrame/ProfilePhotoTextureButton").TextureNormal = (textureImageProfile);
		}


		public void _on_CancelTextureButton_pressed()
		{
			GetTree().ChangeScene("res://Scenes/MainMenu.tscn");
		}
		public void _on_SavePasswordTextureButton_pressed()
		{
			oldPassword = GetParent().GetNode<LineEdit>("EditProfile/EditProfileNinePatchRect/ActualPasswordLineEdit").Text;
			newPassword = GetParent().GetNode<LineEdit>("EditProfile/EditProfileNinePatchRect/NewPasswordLineEdit").Text;
			confirmPassword = GetParent().GetNode<LineEdit>("EditProfile/EditProfileNinePatchRect/ConfirmPasswordLineEdit").Text;
			bool noEmptyFields = ValidateEmptyFields();
			bool verifyPassword = ValidatePassword();
			if(noEmptyFields)
			{
				if(verifyPassword)
				{
					Encryption PasswordHasher = new Encryption();
					string oldPasswordHashed = PasswordHasher.ComputeSHA256Hash(oldPassword);
					string newPasswordHashed = PasswordHasher.ComputeSHA256Hash(newPassword);
					serverClient.PasswordExist(username, oldPasswordHashed);
					if(validateOldPassword)
					{
						serverClient.EditPassword(username, newPasswordHashed);
						CleanFields();
					}
					else
					{
						GetParent().GetNode<AcceptDialog>("EditProfile/EditProfileAcceptDialog").WindowTitle = ("ERROR");
						GetParent().GetNode<AcceptDialog>("EditProfile/EditProfileAcceptDialog").DialogText = ("INCORRECT_OLD_PASSWORD");
						GetParent().GetNode<AcceptDialog>("EditProfile/EditProfileAcceptDialog").Visible = true;
					}
				}
			}
			else
			{
				GetParent().GetNode<AcceptDialog>("EditProfile/EditProfileAcceptDialog").WindowTitle = ("WARNING");
				GetParent().GetNode<AcceptDialog>("EditProfile/EditProfileAcceptDialog").DialogText = ("VERIFY_EMPTY_FIELDS");
				GetParent().GetNode<AcceptDialog>("EditProfile/EditProfileAcceptDialog").Visible = true;
			}
		}
		public void _on_SaveProfileInformationTextureButton_pressed()
		{
			newNickname = GetParent().GetNode<LineEdit>("EditProfile/EditProfileNinePatchRect/NicknameLineEdit").Text;
			bool verifyNickname = ValidateNickname();
			if(!String.IsNullOrWhiteSpace(newNickname))
			{
				if(verifyNickname)
				{
					serverClient.EditNickname(username, newNickname);
				}
				else
				{
					GetParent().GetNode<AcceptDialog>("RegisterUser/RegisterUserDialog").WindowTitle = ("WARNING");
					GetParent().GetNode<AcceptDialog>("RegisterUser/RegisterUserDialog").DialogText = ("VERIFY_NICKNAME");
					GetParent().GetNode<AcceptDialog>("RegisterUser/RegisterUserDialog").Visible = true;
				}
			}
		}
		public void _on_ProfilePhotoTextureButton_pressed()
		{
			GetTree().ChangeScene("res://Scenes/SelectImage.tscn");
		}


		
		public bool ValidatePassword()
		{
			Validation validator = new Validation();
			bool isValid = true;
			if(validator.ValidatePassword(newPassword).Equals(false))
			{
				GetParent().GetNode<AcceptDialog>("EditProfile/EditProfileAcceptDialog").WindowTitle = ("WARNING");
				GetParent().GetNode<AcceptDialog>("EditProfile/EditProfileAcceptDialog").DialogText = ("VERIFY_PASSWORD");
				GetParent().GetNode<AcceptDialog>("EditProfile/EditProfileAcceptDialog").Visible = true;
				isValid = false;
			}
			if(confirmPassword.Equals(newPassword).Equals(false))
			{
				GetParent().GetNode<AcceptDialog>("EditProfile/EditProfileAcceptDialog").WindowTitle = ("WARNING");
				GetParent().GetNode<AcceptDialog>("EditProfile/EditProfileAcceptDialog").DialogText = ("VERIFY_CONFIRM_PASSWORD");
				GetParent().GetNode<AcceptDialog>("EditProfile/EditProfileAcceptDialog").Visible = true;
				isValid = false;
			}
			return isValid;
		}
		public bool ValidateNickname()
		{
			Validation validator = new Validation();
			bool isValid = true;
			if(validator.ValidateUsernameAndNickname(newNickname).Equals(false))
			{
				GetParent().GetNode<AcceptDialog>("EditProfile/EditProfileAcceptDialog").WindowTitle = ("WARNING");
				GetParent().GetNode<AcceptDialog>("EditProfile/EditProfileAcceptDialog").DialogText = ("VERIFY_NICKNAME");
				isValid = false;
			}
			return isValid;
		}
		public bool ValidateEmptyFields()
		{ 
			bool validFields = false;
			if(!String.IsNullOrWhiteSpace(oldPassword) && !String.IsNullOrWhiteSpace(newPassword) && !String.IsNullOrWhiteSpace(confirmPassword))
			{
				validFields = true;
			}
			return validFields;
		}
		public void CleanFields()
		{
			GetParent().GetNode<LineEdit>("EditProfile/EditProfileNinePatchRect/ActualPasswordLineEdit").Text = ("");
			GetParent().GetNode<LineEdit>("EditProfile/EditProfileNinePatchRect/NewPasswordLineEdit").Text = ("");
			GetParent().GetNode<LineEdit>("EditProfile/EditProfileNinePatchRect/ConfirmPasswordLineEdit").Text = ("");
		}
		public void CorrectPassword()
		{
			validateOldPassword = true;
			
		}
		public void IncorrectPassword()
		{
			validateOldPassword = false;
		}
		public void CorrectEditPassword()
		{
			GetParent().GetNode<AcceptDialog>("EditProfile/EditProfileAcceptDialog").WindowTitle = ("NOTIFICATION");
			GetParent().GetNode<AcceptDialog>("EditProfile/EditProfileAcceptDialog").DialogText = ("CORRECT_PASSWORD_UPDATE");
			GetParent().GetNode<AcceptDialog>("EditProfile/EditProfileAcceptDialog").Visible = true;
		}
		public void IncorrectEditPassword()
		{
			GetParent().GetNode<AcceptDialog>("EditProfile/EditProfileAcceptDialog").WindowTitle = ("ERROR");
			GetParent().GetNode<AcceptDialog>("EditProfile/EditProfileAcceptDialog").DialogText = ("ERROR_PASSWORD_UPDATE");
			GetParent().GetNode<AcceptDialog>("EditProfile/EditProfileAcceptDialog").Visible = true;
		}
		public void CorrectEditNickname()
		{
			GetParent().GetNode<AcceptDialog>("EditProfile/EditProfileAcceptDialog").WindowTitle =("NOTIFICATION");
			GetParent().GetNode<AcceptDialog>("EditProfile/EditProfileAcceptDialog").DialogText = ("CORRECT_NICKNAME_UPDATE");
			GetParent().GetNode<AcceptDialog>("EditProfile/EditProfileAcceptDialog").Visible = true;
		}
		public void IncorrectEditNickname()
		{
			GetParent().GetNode<AcceptDialog>("EditProfile/EditProfileAcceptDialog").WindowTitle = ("ERROR");
			GetParent().GetNode<AcceptDialog>("EditProfile/EditProfileAcceptDialog").DialogText = ("ERROR_NICKNAME_UPDATE");
			GetParent().GetNode<AcceptDialog>("EditProfile/EditProfileAcceptDialog").Visible = true;
		}
		public string GetImageProfilePath(int imageProfile)
		{
			switch (imageProfile) 
			{
				case Constants.IMAGE_PROFILE_DEFAULT:
				pathImageProfile = "res://Scenes/Resources/ImagesProfile/imagePerfilDefault.jpg";
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