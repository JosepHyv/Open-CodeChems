using Godot;
using System;
using OpenCodeChems.BussinesLogic;
using OpenCodeChems.DataAccess;
using System.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

public class Network : Node
{
	private int DEFAULT_PORT = 5500;
	private string ADDRESS = "localhost";
	private int MAX_PLAYERS = 200;
	private int PEERID = 1;
	private UserManagement USER_MANAGEMENT = new UserManagement();
	public override void _Ready()
	{
		GD.Print("Entrando al server Godot");
		var server = new NetworkedMultiplayerENet();
		var result = server.CreateServer(DEFAULT_PORT, MAX_PLAYERS);
		GD.Print(result);
		if(result == 0 )
		{
			GetTree().NetworkPeer = server;
			GD.Print($"Hosteando server en {ADDRESS}:{DEFAULT_PORT}.");
			GD.Print(GetTree().NetworkPeer);
		}
		GD.Print($"Estoy escuchando? {GetTree().IsNetworkServer()}");
		GD.Print($"Mi network ID = {GetTree().GetNetworkUniqueId()}");
		GetTree().Connect("network_peer_connected", this, nameof(PlayerConnected));
		GetTree().Connect("network_peer_disconnected", this, nameof(PlayerDisconnected));

	}

	private void PlayerConnected(int peerId)
	{
		GD.Print($"Jugador = {peerId} Conectado");
	}

	private void PlayerDisconnected(int peerId)
	{
		GD.Print($"Jugador = {peerId} Desconectado");
	}

	
	
	[Master]
	private void LoginRequest(string username, string password)
	{
		int senderId = GetTree().GetRpcSenderId();
		try
		{
			if(USER_MANAGEMENT.Login(username, password) == true)
			{
				
				RpcId(senderId, "LoginSuccesful");
				GD.Print($"Player no. {senderId} logged in successfully.");
			}
			else
			{
				RpcId(senderId, "LoginFailed");
				GD.Print($"Player no. {senderId} logged in failed.");
			}
		}
		catch (DbUpdateException)
        {
        	
        }
	}
	[Master]
	private void RegisterUserRequest(string name, string email, string username, string hashPassword, string nickname, byte[] imageProfile, int victories, int defeats)
	{
		int senderId = GetTree().GetRpcSenderId();
		bool status = false;
		try
		{
			User newUser = new User(username, hashPassword, name, email);
			Profile newProfile = new Profile(nickname, victories, defeats, imageProfile, username);
			if(USER_MANAGEMENT.RegisterUser(newUser) == true)
			{
				if (USER_MANAGEMENT.RegisterProfile(newProfile) == true)
				{
					status = true;
					RpcId(senderId, "RegisterSuccesful");
					GD.Print($"Player no. {senderId} registered in successfully.");      
				}       
			}
			else
			{
				RpcId(senderId, "RegisterFail");
				GD.Print($"Player no. {senderId} registered in failed.");
			}
		}
		catch (DbUpdateException)
        {
            status = false;
    	}
	}

	[Master]
	private void EmailRegisteredRequest(string email)
	{	
		int senderId = GetTree().GetRpcSenderId();
		bool status = false;	
		if (USER_MANAGEMENT.EmailRegistered(email) == false)
		{
			status = true;
			RpcId(senderId, "EmailIsNotRegistered");
		}
		else
		{
			RpcId(senderId, "EmailIsRegistered");
		}
	}
	[Master]
	private void UsernameRegisteredRequest(string username)
	{	
		int senderId = GetTree().GetRpcSenderId();
		bool status = false;	
		if (USER_MANAGEMENT.UsernameRegistered(username) == false)
		{
			status = true;
			RpcId(senderId, "UsernameIsNotRegistered");
		}
		else
		{
			RpcId(senderId, "UsernameIsRegistered");
		}
	}
	[Master]
	private void NicknameRegisteredRequest(string nickname)
	{	
		int senderId = GetTree().GetRpcSenderId();
		bool status = false;	
		if (USER_MANAGEMENT.NicknameRegistered(nickname) == false)
		{
			status = true;
			RpcId(senderId, "NicknameIsNotRegistered");
		}
		else
		{
			RpcId(senderId, "NicknameIsRegistered");
		}
	}
	[RemoteSync]
	private void GetProfileRequest(string username)
	{
		int senderId = GetTree().GetRpcSenderId();
		Profile profileObtained = USER_MANAGEMENT.GetProfile(username);
		string nickname = profileObtained.nickname;
		string usernameObtained = profileObtained.username;
		int victories = profileObtained.victories;
		int defeats = profileObtained.defeats;
		byte[] imageProfile = profileObtained.imageProfile;
		if (profileObtained != null)
		{
			RpcId(senderId, "ProfileObtained", nickname, victories, defeats, imageProfile, username);
		}
		else
		{
			RpcId(senderId, "ProfileNotObtained");
		}
	}
}
