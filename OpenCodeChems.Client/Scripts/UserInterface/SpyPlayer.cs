using Godot;
using System;
using System.Collections.Generic;
using OpenCodeChems.Client.Server;
using OpenCodeChems.Client.Resources;


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
	private string PATH_RED_COLOR = "Scenes/Resources/Icons/squareRed.png";
	private ImageTexture textureRed = new ImageTexture();
	private string PATH_BLUE_COLOR = "Scenes/Resources/Icons/squareBlue.png";
	private ImageTexture textureBlue = new ImageTexture();
	private int SelectedIndex = Constants.NULL_INDEX;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		ChatLineEdit = GetParent().GetNode<LineEdit>("SpyPlayer/ChatLineEdit");
		ChatBlock = GetParent().GetNode<TextEdit>("SpyPlayer/ChatTextEdit");
		serverClient = GetNode<Network>("/root/Network") as Network;
		serverClient.Connect("UpdateChatLog", this, nameof(AadToChat));
		serverClient.Connect("ChangeClue", this, nameof(ClueChange));
		serverClient.Connect("UpdateCard",this, nameof(ChangeColor));
		
		List<string> listElements = serverClient.boardWords;
		
		var  itemNode = GetParent().GetNode<ItemList>("SpyPlayer/BackGroundNinePatchRect/CodeNamesItemList");
		for(int c = 0 ; c<itemNode.GetItemCount(); c++)
		{
			itemNode.SetItemText(c, listElements[c]);					
		}
			

	}

	private void _on_ChatTextureButton_pressed()
	{
		string message = ChatLineEdit.Text;
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
		
		SelectedIndex = index;
		var  itemNode = GetParent().GetNode<ItemList>("SpyPlayer/BackGroundNinePatchRect/CodeNamesItemList");
		var  selectedCard = GetParent().GetNode<RichTextLabel>("SpyPlayer/BackGroundNinePatchRect/SelectedCardRichTextLabel");
		selectedCard.Clear();
		selectedCard.AddText(itemNode.GetItemText(SelectedIndex));
		

			
	}
	public void ClueChange(string clue)
	{
		var clueContainer = GetParent().GetNode<RichTextLabel>("SpyPlayer/BackGroundNinePatchRect/KeyNumberRichTextLabel");
		clueContainer.Clear();
		clueContainer.AddText(clue);
	}

	public void _on_SendWordTextureButton_pressed()
	{
		if(SelectedIndex != Constants.NULL_INDEX) 
		{
			serverClient.VerifySelectedCard(SelectedIndex);
		}
	}

	public void ChangeColor(int color, int index)
	{
		if(color == 0)
		{
			ChangeBlue(index);
		}
		else if(color == 1)
		{
			ChangeRed(index);
		}
		else if(color == 2)
		{
			changeYellow(index);
		}
		else if(color == 3)
		{
			ChangeBlack(index);
		}
	}
	public void ChangeBlack(int index)
	{	
		var  itemNode = GetParent().GetNode<ItemList>("SpyPlayer/BackGroundNinePatchRect/CodeNamesItemList");
		assassinBlack.Load(PATH_ASSASSIN_COLOR);
		textureAssassin.CreateFromImage(assassinBlack);
		itemNode.SetItemIcon(index, textureAssassin);
	}
	public void ChangeBlue(int index)
	{
		var  itemNode = GetParent().GetNode<ItemList>("SpyPlayer/BackGroundNinePatchRect/CodeNamesItemList");
		agentTypeBlue.Load(PATH_BLUE_COLOR);
		textureBlue.CreateFromImage(agentTypeBlue);
		itemNode.SetItemIcon(index, textureBlue);
	}
	public void ChangeRed(int index)
	{
		var  itemNode = GetParent().GetNode<ItemList>("SpyPlayer/BackGroundNinePatchRect/CodeNamesItemList");
		agentTypeRed.Load(PATH_RED_COLOR);
		textureRed.CreateFromImage(agentTypeRed);
		itemNode.SetItemIcon(index, textureRed);
	}
	public void changeYellow(int index)
	{
		var  itemNode = GetParent().GetNode<ItemList>("SpyPlayer/BackGroundNinePatchRect/CodeNamesItemList");
		civilYellow.Load(PATH_CIVIL_COLOR);
		textureCivil.CreateFromImage(civilYellow);
		itemNode.SetItemIcon(index, textureCivil);
	}


}
