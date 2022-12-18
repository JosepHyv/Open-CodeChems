using Godot;
using System;
using System.Collections.Generic;
using OpenCodeChems.Client.Server;


public class SpyPlayer : Control
{
	private LineEdit ChatLineEdit; 
	private TextEdit ChatBlock;
	Network serverClient;
	private Image civilYellow = new Image();
	private Image agentTypeRed = new Image();
	private Image agentTypeBlue = new Image();
	private Image assassinBlack = new Image();
	private string PATH_CIVIL_COLOR = "Scenes/Resources/Icons/square-64.png";
	private ImageTexture textureCivil = new ImageTexture();
	private string PATH_ASSASSIN_COLOR = "Scenes/Resources/Icons/ssquareBlack.png";
	private ImageTexture textureAssassin = new ImageTexture();
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		ChatLineEdit = GetParent().GetNode<LineEdit>("SpyPlayer/ChatLineEdit");
		ChatBlock = GetParent().GetNode<TextEdit>("SpyPlayer/ChatTextEdit");
		serverClient = GetNode<Network>("/root/Network") as Network;
		serverClient.Connect("UpdateChatLog", this, nameof(AadToChat));
		
		List<string> listElements = serverClient.boardWords;
		
		var  itemNode = GetParent().GetNode<ItemList>("SpyPlayer/BackGroundNinePatchRect/CodeNamesItemList");
		for(int c = 0 ; c<itemNode.GetItemCount(); c++)
		{
			itemNode.SetItemText(c, listElements[c]);					
		}
			

	}

	private void _on_ChatTextureButton_pressed()
	{
		string message = ChatLineEdit.GetText();
		if(!String.IsNullOrWhiteSpace(message))
		{
			ChatLineEdit.Clear();
			serverClient.ChatInGame(message);
		}
	}
	public void AadToChat(string message)
	{
		ChatBlock.InsertTextAtCursor($"{message}\n");
	}
	private void _on_CodeNamesItemList_item_selected(int index)
	{
		
		
		var  itemNode = GetParent().GetNode<ItemList>("SpyPlayer/BackGroundNinePatchRect/CodeNamesItemList");
		var  selectedCard = GetParent().GetNode<RichTextLabel>("SpyPlayer/BackGroundNinePatchRect/SelectedCardRichTextLabel");
		selectedCard.Clear();
		selectedCard.AddText(itemNode.GetItemText(index));
		

			
	}


}
