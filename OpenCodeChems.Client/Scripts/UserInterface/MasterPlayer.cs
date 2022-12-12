using Godot;
using System;
using System.Collections.Generic;

public class MasterPlayer : Control
{
	
	AcceptDialog masterDialog = null;

	public override void _Ready()
	{
		

		var  itemNode = GetParent().GetNode<ItemList>("MasterPlayer/BackGroundNinePatchRect/CodeNamesItemList");
		string[] listElements = LoadInternacionalizedCards();
		for(int c = 0 ; c<itemNode.GetItemCount(); c++)
		{
			
				itemNode.SetItemText(c, listElements[c]);
				Random RandomClass = new Random();
				int randomNumber = RandomClass.Next(0,25);
				GD.Print(listElements[randomNumber]);
				
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

	}
	/// <summary>
    /// Loads the card codenames from the right file depending on the internacionalization 
    /// into an array of strings.
    /// </summary>
    /// <returns>An array  with codenames as strings.</returns>
	private string[] LoadInternacionalizedCards()
	{	
		string path= "";
		File cardValues = new File();
		string[] listElements = new string[25];
		if(TranslationServer.GetLocale()== "en")
		{
			path = "res://Scenes/Resources/words.txt";
		}
		else
		{
			path = "res://Scenes/Resources/palabras.txt";
		}

		cardValues.Open(path, File.ModeFlags.Read);
		 while (!cardValues.EofReached())
      {
         listElements.Push(cardValues.GetLine().Trim());
      }
		return listElements;
	}

	//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }

}
