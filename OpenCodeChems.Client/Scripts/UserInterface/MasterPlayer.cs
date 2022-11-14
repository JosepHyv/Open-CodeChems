using Godot;
using System;

public class MasterPlayer : Control
{
    
    

    public override void _Ready()
    {
        var  itemNode = GetParent().GetNode<ItemList>("MasterPlayer/BackGroundNinePatchRect/CodeNamesItemList");
        string[] listElements = new string[] {"aceite", "antorcha", "araña", "artículo", "as", "bala", "barra", "bicho", "bloque", "bosque", "botón", "brazo", "cambio", "carta", "caña", "centauro", "científico", "cocinero", "coco", "concierto", "cubierta", "cubo", "diamante", "diario", "dinosaurio"};
        for(int c = 0 ; c<itemNode.GetItemCount(); c++)
        {
            
                itemNode.SetItemText(c, listElements[c]);
                uint randomNumber = GD.Randi() % 25;
                GD.Print(randomNumber);
                
        }
    }

    private void _on_EnviarTextureButton_pressed()
    {
        var  itemNode = GetParent().GetNode<ItemList>("MasterPlayer/BackGroundNinePatchRect/CodeNamesItemList");
        var  pistaPalabra = GetParent().GetNode<LineEdit>("MasterPlayer/BackGroundNinePatchRect/WordLineEdit");
        GD.Print(pistaPalabra.GetText());
        for(int c = 0 ; c<itemNode.GetItemCount(); c++)
        {
            
            if(pistaPalabra.GetText()== itemNode.GetItemText(c))
            {
                GD.Print("no válida");
            }
                
        }

    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }

}
