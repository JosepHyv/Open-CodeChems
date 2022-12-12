using Godot;
using System;

public class SelectLenguage : Control
{
	public override void _Ready()
	{
		
	}
	public void _on_EnglishTextureButton_pressed() 
	{
		TranslationServer.SetLocale("en");
		GetTree().ChangeScene("res://Scenes/MainMenu.tscn");
	}
	public void _on_SpanishTextureButton_pressed()
	{
		TranslationServer.SetLocale("es");
		GetTree().ChangeScene("res://Scenes/MainMenu.tscn");
	}
	public void _on_CancelTextureButton_pressed()
	{
		GetTree().ChangeScene("res://Scenes/MainMenu.tscn");
	}

}
