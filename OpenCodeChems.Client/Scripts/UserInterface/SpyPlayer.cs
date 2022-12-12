using Godot;
using System;

public class SpyPlayer : Control
{
	private LineEdit ChatLineEdit; 
	private TextEdit ChatBlock;


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


//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
