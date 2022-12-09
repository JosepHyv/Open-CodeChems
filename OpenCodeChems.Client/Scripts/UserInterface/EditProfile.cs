using Godot;
using System;
using OpenCodeChems.Client.Server;
using static OpenCodeChems.Client.Resources.Objects;
using System.Threading.Tasks;
using OpenCodeChems.Client.Resources;
using System.IO;

public class EditProfile : Control
{
	
	private string username = MainMenu.username;
	Network serverClient;
	int PEER_ID = 1; 
	private Task<bool> passwordIsCorrect = Task<bool>.FromResult(false);
	private Task<bool> editPasswordIsCorrect = Task<bool>.FromResult(false);
	private Task<bool> editNicknameIsCorrect = Task<bool>.FromResult(false);
	private Task<bool> editImageProfileIsCorrect = Task<bool>.FromResult(false);

	
	private bool validateOldPassword = false;
	private string oldPassword = "";
	private string newPassword = "";
	private string confirmPassword = "";
	private string newEmail = "";
	private string newNickname = "";
	private byte[] newImageProfile = null;
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
		serverClient.Connect("CorrectEditImageProfile", this, nameof(CorrectEditImageProfile));
		serverClient.Connect("EditImageProfileFail", this, nameof(IncorrectEditImageProfile));
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
				if(newImageProfile == null)
				{
					serverClient.EditNickname(username, newNickname);
				}
				else
				{
					serverClient.EditNickname(username, newNickname);
					serverClient.EditImageProfile(username, newImageProfile);
				}
			}
			else
			{
				GetParent().GetNode<AcceptDialog>("RegisterUser/RegisterUserDialog").SetTitle("WARNING");
				GetParent().GetNode<AcceptDialog>("RegisterUser/RegisterUserDialog").SetText("VERIFY_NICKNAME");
				GetParent().GetNode<AcceptDialog>("RegisterUser/RegisterUserDialog").Visible = true;
			}
		}
		else
		{
			if(newImageProfile != null)
			{
				serverClient.EditImageProfile(username, newImageProfile);
			}
			else
			{
				GetParent().GetNode<AcceptDialog>("EditProfile/EditProfileAcceptDialog").SetTitle("WARNING");
				GetParent().GetNode<AcceptDialog>("EditProfile/EditProfileAcceptDialog").SetText("VERIFY_EDIT_PROFILE");
				GetParent().GetNode<AcceptDialog>("EditProfile/EditProfileAcceptDialog").Visible = true;
			}
		}
	}
	public void _on_ProfilePhotoTextureButton_pressed()
	{
		GetParent().GetNode<FileDialog>("EditProfile/SelectImageProfileFileDialog").SetTitle("SEARCH_IMAGE_PROFILE");
		GetParent().GetNode<FileDialog>("EditProfile/SelectImageProfileFileDialog").AddFilter("*.png");
		GetParent().GetNode<FileDialog>("EditProfile/SelectImageProfileFileDialog").AddFilter("*.jpg");
		GetParent().GetNode<FileDialog>("EditProfile/SelectImageProfileFileDialog").Access = ((Godot.FileDialog.AccessEnum)2);
		GetParent().GetNode<FileDialog>("EditProfile/SelectImageProfileFileDialog").Visible = true;

	}
	public void _on_SelectImageProfileFileDialog_file_selected(string imagePath)
	{
		imageProfile.Load(imagePath);
		newImageProfile = ImageToByte(imagePath);
		textureImageProfile.CreateFromImage(imageProfile);
		GetParent().GetNode<TextureButton>("EditProfile/EditProfileNinePatchRect/ProfileInformationTransparentFrame/ProfilePhotoTextureButton").SetNormalTexture(textureImageProfile);
	}

	public byte[] ImageToByte(string imageProfilePath)
	{
		FileStream imageProfileFileStream = new FileStream(imageProfilePath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
		Byte[] imageProfile = new Byte[imageProfileFileStream.Length];
		BinaryReader readearToBinary = new BinaryReader(imageProfileFileStream);
		imageProfile = readearToBinary.ReadBytes(Convert.ToInt32(imageProfileFileStream.Length));
		imageProfileFileStream.Close();
		return imageProfile;
	}
	/*public Image ByteToImage(byte[] imageInByte)
	{
		FileStream imageProfileFileStream = new FileStream(imageProfilePath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
		Byte[] imageProfile = new Byte[imageProfileFileStream.Length];
		BinaryReader readearToBinary = new BinaryReader(imageProfileFileStream);
		imageProfile = readearToBinary.ReadBytes(Convert.ToInt32(imageProfileFileStream.Length));
		imageProfileFileStream.Close();
		return imageProfile;
	}*/
	
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
	public void CorrectEditImageProfile()
	{
		editImageProfileIsCorrect = Task<bool>.FromResult(true);
		GetParent().GetNode<AcceptDialog>("EditProfile/EditProfileAcceptDialog").SetTitle("NOTIFICATION");
		GetParent().GetNode<AcceptDialog>("EditProfile/EditProfileAcceptDialog").SetText("CORRECT_IMAGE_PROFILE_UPDATE");
		GetParent().GetNode<AcceptDialog>("EditProfile/EditProfileAcceptDialog").Visible = true;
	}
	public void IncorrectEditImageProfile()
	{
		editImageProfileIsCorrect = Task<bool>.FromResult(false);
		GetParent().GetNode<AcceptDialog>("EditProfile/EditProfileAcceptDialog").SetTitle("ERROR");
		GetParent().GetNode<AcceptDialog>("EditProfile/EditProfileAcceptDialog").SetText("ERROR_IMAGE_PROFILE_UPDATE");
		GetParent().GetNode<AcceptDialog>("EditProfile/EditProfileAcceptDialog").Visible = true;
	}
}
