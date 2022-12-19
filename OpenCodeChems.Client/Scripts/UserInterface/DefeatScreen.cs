using Godot;
using System;

public class DefeatScreen : Control
{
    
    private TextureButton buton;
    public override void _Ready()
    {
        buton = GetParent().GetNode<TextureButton>("DefeatScreen/TextureButton");
    }

    public void _on_TextureButton_pressed()
    {
        GetTree().ChangeScene("res://Scenes/MainMenu.tscn");
    }


}
