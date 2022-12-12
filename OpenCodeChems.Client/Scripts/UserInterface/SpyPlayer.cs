using Godot;
using System;

public class SpyPlayer : Control
{
	private LineEdit ChatLineEdit; 
	private TextEdit ChatBlock;
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

	}

	private void _on_ChatTextureButton_pressed()
	{
		string message = ChatLineEdit.GetText();
		ChatLineEdit.Clear();
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
