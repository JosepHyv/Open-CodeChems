using Godot;
using System;

public class VictoryScreen : Control
{
    private TextureButton buton;
    public override void _Ready()
    {
        buton = GetParent().GetNode<TextureButton>("VictoryScreen/TextureButton");
    }

    public void _on_TextureButton_pressed()
    {
        GetTree().ChangeScene("res://Scenes/MainMenu.tscn");
    }


}
