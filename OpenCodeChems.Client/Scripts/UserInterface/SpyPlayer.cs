using Godot;
using System;

namespace OpenCodeChems.Client.UserInterface
{
	public class SpyPlayer : Control
	{
		private LineEdit ChatLineEdit; 
		private TextEdit ChatBlock;
		private readonly Image civilYellow = new Image();
		private readonly Image agentTypeRed = new Image();
		private readonly Image agentTypeBlue = new Image();
		private readonly Image assassinBlack = new Image();
		private readonly string PATH_CIVIL_COLOR = "Scenes/Resources/Icons/square-64.png";
		private readonly ImageTexture textureCivil = new ImageTexture();
		private readonly string PATH_ASSASSIN_COLOR = "Scenes/Resources/Icons/ssquareBlack.png";
		private readonly ImageTexture textureAssassin = new ImageTexture();
		
		public override void _Ready()
		{
			ChatLineEdit = GetParent().GetNode<LineEdit>("SpyPlayer/ChatLineEdit");
			ChatBlock = GetParent().GetNode<TextEdit>("SpyPlayer/ChatTextEdit");

		}

		private void _on_ChatTextureButton_pressed()
		{
			string message = ChatLineEdit.Text;
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
}