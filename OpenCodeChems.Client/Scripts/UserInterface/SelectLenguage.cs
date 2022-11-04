using Godot;
using System;

public class SelectLenguage : Control
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        
    }
    public void _on_EnglishTextureButton_pressed() 
    {
        TranslationServer.SetLocale("en");
    }
    public void _on_SpanishTextureButton_pressed()
    {
        TranslationServer.SetLocale("es");
    }
    public void _on_CancelTextureButton_pressed()
    {
        GetTree().ChangeScene("res://Scenes/MainMenu.tscn");
    }



    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //      
    //  }
}
