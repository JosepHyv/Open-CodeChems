using Godot;
using System;

public class SpyPlayer : Control
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        GD.Print("Hola mundo en godot");
    }

    /*private void _on_CodeNamesItemList_item_selected(InputEventMouseButton e)
    {
        
        GD.Print("aqui va la palabra");
        
        
        var  itemNode = GetParent().GetNode<ItemList>("SpyPlayer/BackGroundNinePatchRect/CodeNamesItemList");
        string[] listElements = new string[itemNode.GetItemCount()];

        int elemento = 7;
        string palabra = "Astroaut";

        itemNode.SetItemText(elemento, palabra);


        for(int c = 0 ; c<itemNode.GetItemCount(); c++)
        {
            
                var card = itemNode.GetItemMetadata(c);
                GD.Print(card);
        }

        GD.Print("=================="); 

            
    }*/
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
