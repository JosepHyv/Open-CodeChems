using Godot;
using System;
using OpenCodeChems.Client.Server;
using OpenCodeChems.Client.Resources;

public class connexion : Control
{

	private LineEdit ipLineEdit;
	private LineEdit portLineEdit;
	private AcceptDialog alerta;
	private Label statusLabel;
	private Network serverClient;
	private Validation validations = new Validation();
	public override void _Ready()
	{
		serverClient = GetNode<Network>("/root/Network") as Network;
		statusLabel = GetParent().GetNode<Label>("connexion/statusLabel");
		ipLineEdit = GetParent().GetNode<LineEdit>("connexion/ipLineEdit");
		portLineEdit  = GetParent().GetNode<LineEdit>("connexion/portLineEdit");
		alerta = GetParent().GetNode<AcceptDialog>("connexion/AcceptDialog");
		ipLineEdit.SetText(serverClient.ADDRESS);
		portLineEdit.SetText($"{serverClient.DEFAULT_PORT}");
		serverClient.Connect("ServerDead", this, nameof(CloseClient));
		serverClient.Connect("Server", this, nameof(ServerConnected));
		serverClient.Connect("ServerFail", this, nameof(ServerConnectedFail));
	}
	
	private void CloseClient()
	{
		GD.Print("Me ejecute bien? :0");
		alerta.SetText("Conexion Con El Servidor Perdida");
		alerta.Visible = true;
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
			statusLabel.SetText("TRYING_CONNECT_TO_SERVER");
			
		}
		else
		{
			alerta.SetText("INVALID_ADDRESS_OR_PORT");
			alerta.Visible = true;
		}
		
	}
	
	private void ServerConnected()
	{
		GetTree().ChangeScene("res://Scenes/LogIn.tscn");
	}
	
	private void ServerConnectedFail()
	{
		alerta.SetText("SERVER_NOT_FOUND");
		alerta.Visible = true;
		serverClient.CloseConnection();
		statusLabel.SetText("");
		
	}



}

