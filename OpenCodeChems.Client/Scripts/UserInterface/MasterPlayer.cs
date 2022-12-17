using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using OpenCodeChems.Client.Server;

namespace OpenCodeChems.Client.UserInterface
{
	public class MasterPlayer : Control
	{

		Network serverClient;
		AcceptDialog masterDialog = null;
		private readonly int CARD_GAME_MAX_VALUE = 25;
		private readonly Image agentTypeRed = new Image();
		private readonly Image agentTypeBlue = new Image();
		private readonly Image assassinBlack = new Image();
		private readonly string PATH_ASSASSIN_COLOR = "Scenes/Resources/Icons/ssquareBlack.png";
		private ImageTexture textureAssassin = new ImageTexture();
		public override void _Ready()
		{
			serverClient = GetNode<Network>("/root/Network") as Network;
			var  itemNode = GetParent().GetNode<ItemList>("MasterPlayer/BackGroundNinePatchRect/CodeNamesItemList");
			List<string> listElements = serverClient.boardWords;
			for(int c = 0 ; c<itemNode.GetItemCount(); c++)
			{
				
				itemNode.SetItemText(c, listElements[c]);					
			}


			serverClient.Connect("CleanRoom", this, nameof(ChangeToMainMenu));
		}

		public void ChangeToMainMenu()
		{
			GetTree().ChangeScene("res://Scenes/MainMenu.tscn");
		}

		public void _on_EnviarTextureButton_pressed()
		{
			var  itemNode = GetParent().GetNode<ItemList>("MasterPlayer/BackGroundNinePatchRect/CodeNamesItemList");
			var  pistaPalabra = GetParent().GetNode<LineEdit>("MasterPlayer/BackGroundNinePatchRect/WordLineEdit");
			for(int c = 0 ; c<itemNode.GetItemCount(); c++)
			{
				var masterDialog = GetParent().GetNode<AcceptDialog>("MasterPlayer/MasterPlayerAcceptDialog");
				if(pistaPalabra.Text == itemNode.GetItemText(c))
				{
					masterDialog.WindowTitle = ("WARNING");
					masterDialog.DialogText = ("REPEATED_WORD");
					masterDialog.Visible = true;
				}
				
			}
			assassinBlack.Load(PATH_ASSASSIN_COLOR);
			textureAssassin.CreateFromImage(assassinBlack);
			itemNode.SetItemIcon(2, textureAssassin);
			itemNode.SetItemIcon(9, itemNode.GetItemIcon(0));
			
		}
		/// <summary>
		/// Loads the card codenames from the right file depending on the internacionalization 
		/// into an array of strings.
		/// </summary>
		/// <returns>An array  with codenames as strings.</returns>
		private List<string> LoadInternacionalizedCards()
		{	
			string path = "";
			Godot.File cardValues = new Godot.File();
			List<string> listGameElements = new List<string>();
			List<string> listAllElements = new List<string>();
			if(TranslationServer.GetLocale() == "en")
			{
				path = "res://Scenes/Resources/words.txt";
			}
			else
			{
				path = "res://Scenes/Resources/palabras.txt";
			}
			try
			{
				cardValues.Open(path, Godot.File.ModeFlags.Read);
				while (!cardValues.EofReached())
				{
					listAllElements.Add(cardValues.GetLine().Trim());
				}
			}
			catch(FileNotFoundException)
			{
				masterDialog.WindowTitle = ("ERROR");
				masterDialog.DialogText = ("FILE_NOT_FOUND");
				masterDialog.Visible = true;
			}

			Random rand = new Random();
			listGameElements = listAllElements.OrderBy(_ => rand.Next()).ToList();
			
			
			return listGameElements;
		}

	}
}