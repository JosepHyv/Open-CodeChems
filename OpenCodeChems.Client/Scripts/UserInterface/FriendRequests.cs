using Godot;
using System;
using OpenCodeChems.Client.Server;
using System.Collections.Generic;
using static OpenCodeChems.Client.Resources.Objects;
using System.Threading.Tasks;

public class FriendRequests : Control
{
    Network serverClient;
	private int PEER_ID = 1;
    private int idProfileActualPlayer = MainMenu.idProfile;
    public static int idProfilePlayerFound = 0;
    private string nicknameFriendRequest = "";
    private List<string> friendsRequestsOfActualPlayer;
    private const bool STATUS_FRIEND_ACCEPT = true;
	private const bool STATUS_FRIEND_DENY = false;
    private bool statusRequest = false;
    public static Profile actualPlayer;
    private Task<bool> addFriendIsCorrect = Task<bool>.FromResult(false);
	private Task<bool> denyFriendIsCorrect = Task<bool>.FromResult(false);
    public override void _Ready()
    {
        serverClient = GetNode<Network>("/root/Network") as Network;
        serverClient.Connect("FriendsRequestsFound", this, nameof(GetFriendsRequestsComplete));
		serverClient.Connect("FriendsRequestsNotFound", this, nameof(GetFriendsRequestsFail));
        serverClient.GetFriendsRequests(idProfileActualPlayer);
        serverClient.Connect("ProfileByNicknameFound", this, nameof(GetProfileByNicknameComplete));
		serverClient.Connect("ProfileByNicknameNotFound", this, nameof(GetProfileByNicknameFail));
        serverClient.Connect("CorrectAcceptFriend", this, nameof(AcceptFriendCorrect));
        serverClient.Connect("AcceptFriendFail", this, nameof(AcceptFriendNotCorrect));
        serverClient.Connect("CorrectDenyFriend", this, nameof(DenyFriendCorrect));
        serverClient.Connect("DenyFriendFail", this, nameof(DenyFriendNotCorrect));
    }


    public void _on_CancelTextureButton_pressed()
    {
        GetTree().ChangeScene("res://Scenes/MainMenu.tscn");
    }
    public void _on_AcceptTextureButton_pressed()
    {
        statusRequest = true;
        serverClient.GetProfileByNickname(nicknameFriendRequest);
    }
    public void _on_ItemList_item_selected(int indexItem)
    {
        nicknameFriendRequest = GetParent().GetNode<ItemList>("FriendRequests/FriendRequestsNinePatchRect/FriendRequestsItemList").GetItemText(indexItem);
    }
    public void _on_DenyTextureButton_pressed()
    {
        serverClient.GetProfileByNickname(nicknameFriendRequest);
        statusRequest = false;
    }
    public void GetFriendsRequestsComplete()
	{
		if(serverClient.friendsRequestsObtained !=null)
		{
			friendsRequestsOfActualPlayer = serverClient.friendsRequestsObtained;
			foreach(var friend in friendsRequestsOfActualPlayer)
			{
				GetParent().GetNode<ItemList>("FriendRequests/FriendRequestsNinePatchRect/FriendRequestsItemList").AddItem(friend.ToString(), null, true);
			}
		}
	}
	public void GetFriendsRequestsFail()
	{
		GetParent().GetNode<AcceptDialog>("FriendRequests/FriendRequestAcceptDialog").DialogText =("ERROR_LOADING_FRIENDS_REQUESTS");
		GetParent().GetNode<AcceptDialog>("FriendRequests/FriendRequestAcceptDialog").WindowTitle = ("ERROR");
		GetParent().GetNode<AcceptDialog>("FriendRequests/FriendRequestAcceptDialog").Visible = true;
	}
    public void GetProfileByNicknameComplete()
	{
		if(serverClient.profileByNicknameObtained != null)
		{
			actualPlayer = serverClient.profileByNicknameObtained;
			idProfilePlayerFound = actualPlayer.idProfile;
			string nickname = actualPlayer.nickname;
			int victories = actualPlayer.victories;
			int defeats = actualPlayer.defeats;
			int imageProfile = actualPlayer.imageProfile;
			string usernameObtained = actualPlayer.username;
		}
        if(statusRequest == true)
        {
            serverClient.AcceptFriend(idProfilePlayerFound, idProfileActualPlayer, STATUS_FRIEND_ACCEPT);
            cleanFriendsRequest();
        }
        else
        {
            serverClient.DenyFriend(idProfilePlayerFound, idProfileActualPlayer, STATUS_FRIEND_DENY);
            cleanFriendsRequest();
        }
		
	}
	public void GetProfileByNicknameFail()
	{
		GetParent().GetNode<AcceptDialog>("FriendRequests/FriendRequestAcceptDialog").DialogText = ("ERROR_LOADING_PROFILE");
		GetParent().GetNode<AcceptDialog>("FriendRequests/FriendRequestAcceptDialog").WindowTitle = ("ERROR");
		GetParent().GetNode<AcceptDialog>("FriendRequests/FriendRequestAcceptDialog").Visible = true;
	}
    public void AcceptFriendCorrect()
	{
		addFriendIsCorrect = Task<bool>.FromResult(true);
	}
	public void AcceptFriendNotCorrect()
	{
		GetParent().GetNode<AcceptDialog>("FriendRequests/FriendRequestAcceptDialog").WindowTitle = ("ERROR");
		GetParent().GetNode<AcceptDialog>("FriendRequests/FriendRequestAcceptDialog").DialogText =
		("FRIEND_REQUEST_CANT_ACCEPTED");
		GetParent().GetNode<AcceptDialog>("AddFriend/AddFriendAcceptDialog").Visible = true;
		addFriendIsCorrect = Task<bool>.FromResult(false);
	}
    public void DenyFriendCorrect()
	{
		denyFriendIsCorrect = Task<bool>.FromResult(true);
	}
	public void DenyFriendNotCorrect()
	{
		GetParent().GetNode<AcceptDialog>("FriendRequests/FriendRequestAcceptDialog").WindowTitle = ("ERROR");
		GetParent().GetNode<AcceptDialog>("FriendRequests/FriendRequestAcceptDialog").DialogText =
		("FRIEND_REQUEST_CANT_DENY");
		GetParent().GetNode<AcceptDialog>("AddFriend/AddFriendAcceptDialog").Visible = true;
		denyFriendIsCorrect = Task<bool>.FromResult(false);
	}
    public void cleanFriendsRequest()
    {
        GetParent().GetNode<ItemList>("FriendRequests/FriendRequestsNinePatchRect/FriendRequestsItemList").Clear();
        serverClient.GetFriendsRequests(idProfileActualPlayer);
    }
}
