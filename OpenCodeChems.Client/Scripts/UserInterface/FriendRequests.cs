using Godot;
using System;
using OpenCodeChems.Client.Server;
using System.Collections.Generic;

public class FriendRequests : Control
{
    Network serverClient;
	private int PEER_ID = 1;
    private int idProfileActualPlayer = MainMenu.idProfile;
    private List<string> friendsRequestsOfActualPlayer;
    private bool STATUS_FRIENDS_REQUESTS = false;
    public override void _Ready()
    {
        serverClient = GetNode<Network>("/root/Network") as Network;
        serverClient.Connect("FriendsRequestsFound", this, nameof(GetFriendsRequestsComplete));
		serverClient.Connect("FriendsRequestsNotObtained", this, nameof(GetFriendsRequestsFail));
        serverClient.GetFriendsRequests(idProfileActualPlayer, STATUS_FRIENDS_REQUESTS);
    }


    public void _on_CancelTextureButton_pressed()
    {
        GetTree().ChangeScene("res://Scenes/MainMenu.tscn");
    }
    public void _on_AcceptTextureButton_pressed()
    {

    }
    public void _on_ItemList_item_selected(int indexItem)
    {
        string nickname = GetParent().GetNode<ItemList>("FriendRequests/FriendRequestsNinePatchRect/FriendRequestsItemList").GetItemText(indexItem);
    }
    public void _on_DenyTextureButton_pressed()
    {

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
		GetParent().GetNode<AcceptDialog>("FriendRequests/FriendRequestAcceptDialog").SetText("ERROR_LOADING_FRIENDS_REQUESTS");
		GetParent().GetNode<AcceptDialog>("FriendRequests/FriendRequestAcceptDialog").SetTitle("WARNING");
		GetParent().GetNode<AcceptDialog>("FriendRequests/FriendRequestAcceptDialog").Visible = true;
	}
}
