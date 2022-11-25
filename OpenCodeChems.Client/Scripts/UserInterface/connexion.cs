using Godot;
using System;
using OpenCodeChems.Client.Server;
using OpenCodeChems.Client.Resources;

public class connexion : Control
{

	private LineEdit ipLineEdit;
	private LineEdit portLineEdit;
	private AcceptDialog alerta;
	private Network serverClient;
	private Validation validations = new Validation();
	public override void _Ready()
	{
		serverClient = GetNode<Network>("/root/Network") as Network;
		ipLineEdit = GetParent().GetNode<LineEdit>("connexion/ipLineEdit");
		portLineEdit  = GetParent().GetNode<LineEdit>("connexion/portLineEdit");
		alerta = GetParent().GetNode<AcceptDialog>("connexion/AcceptDialog");
	}
	
	
	private void _on_Button_pressed()
	{
		
		string ipAddress = ipLineEdit.GetText();
		string port = portLineEdit.GetText();	
		if(validations.ValidateIp(ipAddress) && validations.ValidatePort(port))
		{
			serverClient.ADDRESS = ipAddress;
			serverClient.DEFAULT_PORT = Int32.Parse(port);
			serverClient.ConnectToServer();
			GetTree().ChangeScene("res://Scenes/LogIn.tscn");
		}
		else
		{
			alerta.SetText("INVALID_ADDRESS_OR_PORT");
			alerta.Visible = true;
		}
		
	}
	
	private void TryConnect()
	{
		
	}



}

