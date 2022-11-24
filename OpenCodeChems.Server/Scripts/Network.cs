using Godot;
using System;
using OpenCodeChems.BussinesLogic;
using OpenCodeChems.DataAccess;
using System.Data.SqlClient;

public class Network : Node
{
	private int DEFAULT_PORT = 5500;
	private string ADDRESS = "localhost";
	private int MAX_PLAYERS = 200;
	private int PEERID = 1;
	private LineEdit ipLineEdit; 
	private LineEdit portLineEdit; 
	private TextEdit logBlock;
	private RichTextLabel listening;
	private Button connectButton;
	public override void _Ready()
	{
		ipLineEdit = GetParent().GetNode<LineEdit>("Network/ip");	
		portLineEdit = GetParent().GetNode<LineEdit>("Network/puerto");
		logBlock = GetParent().GetNode<TextEdit>("Network/TextEdit");
		listening = GetParent().GetNode<RichTextLabel>("Network/currentDirText");
		connectButton = GetParent().GetNode<Button>("Network/Button");
		ipLineEdit.SetText(ADDRESS);
		portLineEdit.SetText(DEFAULT_PORT.ToString());
		
	}

	private void PlayerConnected(int peerId)
	{
		GD.Print($"Jugador = {peerId} Conectado");
	}

	private void PlayerDisconnected(int peerId)
	{
		GD.Print($"Jugador = {peerId} Desconectado");
	}

	private void _on_Button_pressed()
	{
		// Replace with function body.
		ADDRESS = ipLineEdit.GetText();
		DEFAULT_PORT = Int32.Parse(portLineEdit.GetText());
		connectButton.SetDisabled(true);
		logBlock.InsertTextAtCursor("Entrando al server Godot\n");
		var server = new NetworkedMultiplayerENet();
		var result = server.CreateServer(DEFAULT_PORT, MAX_PLAYERS);
		GD.Print(result);
		if(result == 0 )
		{
			GetTree().NetworkPeer = server;
			logBlock.InsertTextAtCursor($"Hosteando server en {ADDRESS}:{DEFAULT_PORT}.\n");
			GD.Print(GetTree().NetworkPeer);
			listening.SetText($"{ADDRESS}:{DEFAULT_PORT}");
		}
		logBlock.InsertTextAtCursor($"Estoy escuchando? {GetTree().IsNetworkServer()}\n");
		logBlock.InsertTextAtCursor($"Mi network ID = {GetTree().GetNetworkUniqueId()}\n");
		GetTree().Connect("network_peer_connected", this, nameof(PlayerConnected));
		GetTree().Connect("network_peer_disconnected", this, nameof(PlayerDisconnected));
	}
	
	[Master]
	private void LoginRequest(string username, string password)
	{
		int senderId = GetTree().GetRpcSenderId();
		UserManagement userManagement = new UserManagement();
		if(/*userManagement.Login(username, password) == */true)
		{
			
			RpcId(senderId, "LoginSuccesful");
			logBlock.InsertTextAtCursor($"Player no. {senderId} logged in successfully.\n");
		}
		else
		{
			RpcId(senderId, "LoginFailed");
			logBlock.InsertTextAtCursor($"Player no. {senderId} logged in failed.\n");
		}
	}
	[Master]
	private void RegisterUserRequest(string name, string email, string username, string hashPassword,byte[] imageProfile, int victories, int defeats)
	{
		int senderId = GetTree().GetRpcSenderId();
		UserManagement userManagement = new UserManagement();
		bool status = false;
		try
		{
			User newUser = new User(username, hashPassword, name, email, victories, defeats, imageProfile);
			if(/*userManagement.RegisterUser(newUser) == */true)
			{
				status = true;
				RpcId(senderId, "RegisterSuccesful");
				logBlock.InsertTextAtCursor($"Player no. {senderId} registered in successfully.\n");             
			}
			else
			{
				RpcId(senderId, "RegisterFail");
				logBlock.InsertTextAtCursor($"Player no. {senderId} registered in failed.\n");
			}
		}
		catch 
		{
			
		}
	}
}



