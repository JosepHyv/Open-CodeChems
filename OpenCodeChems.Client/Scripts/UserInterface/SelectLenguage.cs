using Godot;
using System;
using OpenCodeChems.Client.Server;

namespace OpenCodeChems.Client.UserInterface
{
	public class SelectLenguage : Control
	{

		Network serverClient;
		public override void _Ready()
		{
			serverClient = GetNode<Network>("/root/Network") as Network;

		}
		public void _on_EnglishTextureButton_pressed() 
		{
			TranslationServer.SetLocale("en");
			serverClient.UpdateServerLanguage("en");
			GetTree().ChangeScene("res://Scenes/MainMenu.tscn");
		}
		public void _on_SpanishTextureButton_pressed()
		{
			TranslationServer.SetLocale("es");
			serverClient.UpdateServerLanguage("es");
			GetTree().ChangeScene("res://Scenes/MainMenu.tscn");
		}
		public void _on_CancelTextureButton_pressed()
		{
			string lang = TranslationServer.GetLocale();
			serverClient.UpdateServerLanguage(lang);
			GetTree().ChangeScene("res://Scenes/MainMenu.tscn");
		}

	}
}