using Godot;
using System;
using OpenCodeChems.Client.Server;
using static OpenCodeChems.Client.Resources.Objects;
using System.Threading.Tasks;

public class SeeFriend : Control
{
    Network serverClient;
	private int PEER_ID = 1;
    private string nicknameFriend = MainMenu.nicknameFriend;
    private string nicknameFriendFound = "";
    private int victories = 0;
    private int defeats = 0;
    private int idProfileFound = 0;
    private int idProfileActualPlayer = MainMenu.idProfile;
    private Profile profileObtained = null;
    private bool STATUS_DELETE_FRIEND = true;
    private Task<bool> deleteFriendIsCorrect = Task<bool>.FromResult(false);

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
        GetParent().GetNode<AcceptDialog>("SeeFriend/DeleteFriendConfirmationDialog").SetText("CONFIRMATION_DELETE_FRIEND");
		GetParent().GetNode<AcceptDialog>("SeeFriend/DeleteFriendConfirmationDialog").SetTitle("WARNING");
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
			byte [] imageProfile = profileObtained.imageProfile;
			string username = profileObtained.username;
		}
        GetParent().GetNode<Label>("SeeFriend/SeeFriendNinePatchRect/NicknameFriendLabel").SetText(nicknameFriendFound);
        GetParent().GetNode<Label>("SeeFriend/SeeFriendNinePatchRect/VictoriesLabel").SetText(victories.ToString());
        GetParent().GetNode<Label>("SeeFriend/SeeFriendNinePatchRect/DefeatsLabel").SetText(defeats.ToString());

	}
	public void GetProfileByNicknameFail()
	{
		GetParent().GetNode<AcceptDialog>("SeeFriend/DeleteFriendAcceptDialog").SetText("ERROR_LOADING_PROFILE");
		GetParent().GetNode<AcceptDialog>("SeeFriend/DeleteFriendAcceptDialog").SetTitle("ERROR");
		GetParent().GetNode<AcceptDialog>("SeeFriend/DeleteFriendAcceptDialog").Visible = true;
	}

    public void DeleteFriendCorrect()
	{
		GetParent().GetNode<AcceptDialog>("SeeFriend/DeleteFriendAcceptDialog").SetText("DELETE_FRIEND_CORRECT");
		GetParent().GetNode<AcceptDialog>("SeeFriend/DeleteFriendAcceptDialog").SetTitle("NOTIFICATION");
		GetParent().GetNode<AcceptDialog>("SeeFriend/DeleteFriendAcceptDialog").Visible = true;
		deleteFriendIsCorrect = Task<bool>.FromResult(true);
	}
    public void DeleteFriendNotCorrect()
	{
        GetParent().GetNode<AcceptDialog>("SeeFriend/DeleteFriendAcceptDialog").SetText("ERROR_DELETE_FRIEND");
		GetParent().GetNode<AcceptDialog>("SeeFriend/DeleteFriendAcceptDialog").SetTitle("ERROR");
		GetParent().GetNode<AcceptDialog>("SeeFriend/DeleteFriendAcceptDialog").Visible = true;
		deleteFriendIsCorrect = Task<bool>.FromResult(false);
	}

}
