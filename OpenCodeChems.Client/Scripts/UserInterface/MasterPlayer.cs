using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using OpenCodeChems.Client.Server;

public class MasterPlayer : Control
{

	private bool executed = false;
	Network serverClient;
	AcceptDialog masterDialog = null;
	private int CARD_GAME_MAX_VALUE = 25;
	private Image agentTypeRed = new Image();
	private Image agentTypeBlue = new Image();
	private Image assassinBlack = new Image();
	private string PATH_ASSASSIN_COLOR = "Scenes/Resources/Icons/ssquareBlack.png";
	private ImageTexture textureAssassin = new ImageTexture();
	public override void _Ready()
	{
		GD.Print("Me ejecuto primero");
		serverClient = GetNode<Network>("/root/Network") as Network;
		
		List<string> listElements = serverClient.boardWords;
		while(!executed)
		{
			var  itemNode = GetParent().GetNode<ItemList>("MasterPlayer/BackGroundNinePatchRect/CodeNamesItemList");
			for(int c = 0 ; c<itemNode.GetItemCount(); c++)
			{
				itemNode.SetItemText(c, listElements[c]);					
			}
			executed = !executed;
		}


		serverClient.Connect("CleanRoom", this, nameof(ChangeToMainMenu));
	}

	public void ChangeToMainMenu()
	{
		GetTree().ChangeScene("res://Scenes/MainMenu.tscn");
	}

	private void _on_EnviarTextureButton_pressed()
	{
		var  itemNode = GetParent().GetNode<ItemList>("MasterPlayer/BackGroundNinePatchRect/CodeNamesItemList");
		var  pistaPalabra = GetParent().GetNode<LineEdit>("MasterPlayer/BackGroundNinePatchRect/WordLineEdit");
		for(int c = 0 ; c<itemNode.GetItemCount(); c++)
		{
			var masterDialog = GetParent().GetNode<AcceptDialog>("MasterPlayer/MasterPlayerAcceptDialog");
			if(pistaPalabra.GetText()== itemNode.GetItemText(c))
			{
				masterDialog.SetTitle("WARNING");
				masterDialog.SetText("REPEATED_WORD");
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
		bool exist = true;
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
      		masterDialog.SetTitle("ERROR");
			masterDialog.SetText("FILE_NOT_FOUND");
			masterDialog.Visible = true;
    	}

		Random rand = new Random();
        listGameElements = listAllElements.OrderBy(_ => rand.Next()).ToList();
		
		
		return listGameElements;
	}

}
