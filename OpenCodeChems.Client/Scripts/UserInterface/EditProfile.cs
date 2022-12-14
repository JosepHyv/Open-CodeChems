using Godot;
using System;
using OpenCodeChems.Client.Server;
using static OpenCodeChems.Client.Resources.Objects;
using System.Threading.Tasks;
using OpenCodeChems.Client.Resources;
using System.IO;

public class EditProfile : Control
{
	
	public static string username = MainMenu.username;
	Network serverClient;
	int PEER_ID = 1; 
	private Task<bool> passwordIsCorrect = Task<bool>.FromResult(false);
	private Task<bool> editPasswordIsCorrect = Task<bool>.FromResult(false);
	private Task<bool> editNicknameIsCorrect = Task<bool>.FromResult(false);
	private bool validateOldPassword = false;
	private string oldPassword = "";
	private string newPassword = "";
	private string confirmPassword = "";
	private string newEmail = "";
	private string newNickname = "";
	private int newImageProfile = 0;
	private ImageTexture textureImageProfile = new ImageTexture(); 
	private Image imageProfile = new Image();
	public override void _Ready()
	{
		serverClient = GetNode<Network>("/root/Network") as Network;
		serverClient.Connect("CorrectOldPassword", this, nameof(CorrectPassword));
		serverClient.Connect("IncorrectOldPassword", this, nameof(IncorrectPassword));
		serverClient.Connect("CorrectEditPassword", this, nameof(CorrectEditPassword));
		serverClient.Connect("EditPasswordFail", this, nameof(IncorrectEditPassword));
		serverClient.Connect("CorrectEditNickname", this, nameof(CorrectEditNickname));
		serverClient.Connect("EditNicknameFail", this, nameof(IncorrectEditNickname));
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
		if(noEmptyFields == true)
		{
			if(verifyPassword == true)
			{
				Encryption PasswordHasher = new Encryption();
				string oldPasswordHashed = PasswordHasher.ComputeSHA256Hash(oldPassword);
				string newPasswordHashed = PasswordHasher.ComputeSHA256Hash(newPassword);
				string confirmPasswordHashed = PasswordHasher.ComputeSHA256Hash(confirmPassword);
				serverClient.PasswordExist(username, oldPasswordHashed);
				if(validateOldPassword == true)
				{
					serverClient.EditPassword(username, newPasswordHashed);
					CleanFields();
				}
				else
				{
					GetParent().GetNode<AcceptDialog>("EditProfile/EditProfileAcceptDialog").SetTitle("ERROR");
					GetParent().GetNode<AcceptDialog>("EditProfile/EditProfileAcceptDialog").SetText("INCORRECT_OLD_PASSWORD");
					GetParent().GetNode<AcceptDialog>("EditProfile/EditProfileAcceptDialog").Visible = true;
				}
			}
		}
		else
		{
			GetParent().GetNode<AcceptDialog>("EditProfile/EditProfileAcceptDialog").SetTitle("WARNING");
			GetParent().GetNode<AcceptDialog>("EditProfile/EditProfileAcceptDialog").SetText("VERIFY_EMPTY_FIELDS");
			GetParent().GetNode<AcceptDialog>("EditProfile/EditProfileAcceptDialog").Visible = true;
		}
	}
	public void _on_SaveProfileInformationTextureButton_pressed()
	{
		newNickname = GetParent().GetNode<LineEdit>("EditProfile/EditProfileNinePatchRect/NicknameLineEdit").Text;
		bool verifyNickname = ValidateNickname();
		if(!String.IsNullOrWhiteSpace(newNickname))
		{
			if(verifyNickname == true)
			{
				serverClient.EditNickname(username, newNickname);
			}
			else
			{
				GetParent().GetNode<AcceptDialog>("RegisterUser/RegisterUserDialog").SetTitle("WARNING");
				GetParent().GetNode<AcceptDialog>("RegisterUser/RegisterUserDialog").SetText("VERIFY_NICKNAME");
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
		if(validator.ValidatePassword(newPassword) == false)
		{
			GetParent().GetNode<AcceptDialog>("EditProfile/EditProfileAcceptDialog").SetTitle("WARNING");
			GetParent().GetNode<AcceptDialog>("EditProfile/EditProfileAcceptDialog").SetText("VERIFY_PASSWORD");
			GetParent().GetNode<AcceptDialog>("EditProfile/EditProfileAcceptDialog").Visible = true;
			isValid = false;
		}
		if(confirmPassword.Equals(newPassword) == false)
		{
			GetParent().GetNode<AcceptDialog>("EditProfile/EditProfileAcceptDialog").SetTitle("WARNING");
			GetParent().GetNode<AcceptDialog>("EditProfile/EditProfileAcceptDialog").SetText("VERIFY_CONFIRM_PASSWORD");
			GetParent().GetNode<AcceptDialog>("EditProfile/EditProfileAcceptDialog").Visible = true;
			isValid = false;
		}
		return isValid;
	}
	public bool ValidateNickname()
	{
		Validation validator = new Validation();
		bool isValid = true;
		if(validator.ValidateUsernameAndNickname(newNickname) == false)
		{
			GetParent().GetNode<AcceptDialog>("EditProfile/EditProfileAcceptDialog").SetTitle("WARNING");
			GetParent().GetNode<AcceptDialog>("EditProfile/EditProfileAcceptDialog").SetText("VERIFY_NICKNAME");
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
		GetParent().GetNode<LineEdit>("EditProfile/EditProfileNinePatchRect/ActualPasswordLineEdit").SetText("");
		GetParent().GetNode<LineEdit>("EditProfile/EditProfileNinePatchRect/NewPasswordLineEdit").SetText("");
		GetParent().GetNode<LineEdit>("EditProfile/EditProfileNinePatchRect/ConfirmPasswordLineEdit").SetText("");
	}
	public void CorrectPassword()
	{
		passwordIsCorrect = Task<bool>.FromResult(true);
		validateOldPassword = true;
		
	}
	public void IncorrectPassword()
	{
		passwordIsCorrect = Task<bool>.FromResult(false);
		validateOldPassword = false;
	}
	public void CorrectEditPassword()
	{
		editPasswordIsCorrect = Task<bool>.FromResult(true);
		GetParent().GetNode<AcceptDialog>("EditProfile/EditProfileAcceptDialog").SetTitle("NOTIFICATION");
		GetParent().GetNode<AcceptDialog>("EditProfile/EditProfileAcceptDialog").SetText("CORRECT_PASSWORD_UPDATE");
		GetParent().GetNode<AcceptDialog>("EditProfile/EditProfileAcceptDialog").Visible = true;
	}
	public void IncorrectEditPassword()
	{
		editPasswordIsCorrect = Task<bool>.FromResult(true);
		GetParent().GetNode<AcceptDialog>("EditProfile/EditProfileAcceptDialog").SetTitle("ERROR");
		GetParent().GetNode<AcceptDialog>("EditProfile/EditProfileAcceptDialog").SetText("ERROR_PASSWORD_UPDATE");
		GetParent().GetNode<AcceptDialog>("EditProfile/EditProfileAcceptDialog").Visible = true;
	}
	public void CorrectEditNickname()
	{
		editNicknameIsCorrect = Task<bool>.FromResult(true);
		GetParent().GetNode<AcceptDialog>("EditProfile/EditProfileAcceptDialog").SetTitle("NOTIFICATION");
		GetParent().GetNode<AcceptDialog>("EditProfile/EditProfileAcceptDialog").SetText("CORRECT_NICKNAME_UPDATE");
		GetParent().GetNode<AcceptDialog>("EditProfile/EditProfileAcceptDialog").Visible = true;
	}
	public void IncorrectEditNickname()
	{
		editNicknameIsCorrect = Task<bool>.FromResult(false);
		GetParent().GetNode<AcceptDialog>("EditProfile/EditProfileAcceptDialog").SetTitle("ERROR");
		GetParent().GetNode<AcceptDialog>("EditProfile/EditProfileAcceptDialog").SetText("ERROR_NICKNAME_UPDATE");
		GetParent().GetNode<AcceptDialog>("EditProfile/EditProfileAcceptDialog").Visible = true;
	}
	
}
