using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

public class MasterPlayer : Control
{
	AcceptDialog masterDialog = null;
	private int CARD_GAME_MAX_VALUE = 25;
	private Image agentTypeRed = new Image();
	private Image agentTypeBlue = new Image();
	private Image assassinBlack = new Image();
	private string PATH_ASSASSIN_COLOR = "Scenes/Resources/Icons/ssquareBlack.png";
	private ImageTexture textureAssassin = new ImageTexture();
	public override void _Ready()
	{
		

		var  itemNode = GetParent().GetNode<ItemList>("MasterPlayer/BackGroundNinePatchRect/CodeNamesItemList");
		List<string> listElements = LoadInternacionalizedCards();
		for(int c = 0 ; c<itemNode.GetItemCount(); c++)
		{
			itemNode.SetItemText(c, listElements[c]);					
		}
		
		
	}

	private void _on_EnviarTextureButton_pressed()
	{
		var  itemNode = GetParent().GetNode<ItemList>("MasterPlayer/BackGroundNinePatchRect/CodeNamesItemList");
		var  pistaPalabra = GetParent().GetNode<LineEdit>("MasterPlayer/BackGroundNinePatchRect/WordLineEdit");
		GD.Print(pistaPalabra.GetText());
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
		GD.Print(itemNode.GetItemIcon(0).GetData());
		GD.Print(itemNode.GetItemIcon(4).GetData());
		GD.Print(itemNode.GetItemIcon(14).GetData());
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
		if(TranslationServer.GetLocale()== "en")
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
		catch(FileNotFoundException e)
		{
      		GD.Print(e.ToString());
    	}

		Random rand = new Random();
        listGameElements = listAllElements.OrderBy(_ => rand.Next()).ToList();
 
        GD.Print(String.Join(", ", listGameElements));
		
		
		return listGameElements;
	}

	//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }

}
