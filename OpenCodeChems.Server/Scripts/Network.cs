using Godot;
using System;
using System.Collections.Generic;
using OpenCodeChems.BussinesLogic;
using OpenCodeChems.DataAccess;
using System.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using OpenCodeChems.Server.Utils;

public class Network : Node
{
	private int DEFAULT_PORT = 5500;
	private string ADDRESS = "localhost";
	private int MAX_PLAYERS = 200;
	private int PEERID = 1;
	private UserManagement USER_MANAGEMENT = new UserManagement();
	private LineEdit ipLineEdit; 
	private LineEdit portLineEdit; 
	private TextEdit logBlock;
	private RichTextLabel listening;
	private Button connectButton;
	private AcceptDialog dialog;
	private Dictionary<string, List<int>> rooms;
	public List<int> clientsConected;
	public override void _Ready()
	{
		clientsConected = new List<int>();
		dialog = GetParent().GetNode<AcceptDialog>("Network/AcceptDialog");
		rooms = new Dictionary<string, List<int>>();
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
		logBlock.InsertTextAtCursor($"Jugador = {peerId} Conectado\n");
		clientsConected.Add(peerId);
	}

	private void PlayerDisconnected(int peerId)
	{
		logBlock.InsertTextAtCursor($"Jugador = {peerId} Desconectado\n");
		clientsConected.Remove(peerId);
	}

	private void _on_Button_pressed()
	{
		// Replace with function body.
		string ipAddress = ipLineEdit.GetText();
		string port = portLineEdit.GetText();
		Validation validations = new Validation();
		if(validations.ValidateIp(ipAddress) && validations.ValidatePort(port))
		{
			ADDRESS = ipAddress;
			DEFAULT_PORT = Int32.Parse(port);
			connectButton.SetDisabled(true);
			logBlock.InsertTextAtCursor("Entrando al server OpenCodeChems\n");
			var server = new NetworkedMultiplayerENet();
			var result = server.CreateServer(DEFAULT_PORT, MAX_PLAYERS);
			GD.Print(result);
			if(result == 0 )
			{
				GetTree().NetworkPeer = server;
				logBlock.InsertTextAtCursor($"Hosteando server en {ADDRESS}:{DEFAULT_PORT}.\n");
				logBlock.InsertTextAtCursor($"{GetTree().NetworkPeer}\n");
				listening.SetText($"{ADDRESS}:{DEFAULT_PORT}");
			}
			logBlock.InsertTextAtCursor($"Estoy escuchando? {GetTree().IsNetworkServer()}\n");
			logBlock.InsertTextAtCursor($"Mi network ID = {GetTree().GetNetworkUniqueId()}\n");
			GetTree().Connect("network_peer_connected", this, nameof(PlayerConnected));
			GetTree().Connect("network_peer_disconnected", this, nameof(PlayerDisconnected));
		}
		else
		{
			dialog.SetText("INVALID_ADDRESS_OR_PORT");
			dialog.Visible = true;
		}
		
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
			  	logBlock.InsertTextAtCursor($"Player no. {senderId} logged in successfully.\n");
			}
			else
			{
				RpcId(senderId, "LoginFailed");
			  	logBlock.InsertTextAtCursor($"Player no. {senderId} logged in failed.\n");
			}
		}
		catch (DbUpdateException)
	{
	  RpcId(senderId, "LoginFailed");
	}
	}

	[Master]
	private void RegisterUserRequest(string name, string email, string username, string hashPassword, string nickname, byte[] imageProfile, int victories, int defeats)
	{
		int senderId = GetTree().GetRpcSenderId();
		int idProfile = 0;
		try
		{
			User newUser = new User(username, hashPassword, name, email);
			Profile newProfile = new Profile(idProfile, nickname, victories, defeats, imageProfile, username);
			if(USER_MANAGEMENT.RegisterUser(newUser) == true)
			{
				if (USER_MANAGEMENT.RegisterProfile(newProfile) == true)
				{
			  		RpcId(senderId, "RegisterSuccesful");
				  	logBlock.InsertTextAtCursor($"Player no. {senderId} registered in successfully.\n");      
				}       	           
			}
			else
			{
				RpcId(senderId, "RegisterFail");
				logBlock.InsertTextAtCursor($"Player no. {senderId} registered in failed.\n");
			}
		}
		catch (DbUpdateException)
	{
	   RpcId(senderId, "RegisterFail");
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
	
	[Master]
	private void GetProfileRequest(string username)
	{
		int senderId = GetTree().GetRpcSenderId();
		Profile profileObtained = USER_MANAGEMENT.GetProfile(username);
		if (profileObtained != null)
		{
			int idProfile = profileObtained.idProfile;
			string nickname = profileObtained.nickname;
			string usernameObtained = profileObtained.username;
			int victories = profileObtained.victories;
			int defeats = profileObtained.defeats;
			byte[] imageProfile = profileObtained.imageProfile;
			RpcId(senderId, "ProfileObtained", idProfile, nickname, victories, defeats, imageProfile, usernameObtained);
		}
		else
		{
			RpcId(senderId, "ProfileNotObtained");
		}
	}
	
	[Master]
	public void CreateRoom(string code)
	{
		int senderId = GetTree().GetRpcSenderId();
		if(rooms.ContainsKey(code))
		{
			RpcId(senderId, "CreateRoomFail");
			logBlock.InsertTextAtCursor($"user {senderId} cant create {code} room\n");
		}
		else
		{
			List<int>hostRoom = new List<int>();
			hostRoom.Add(senderId);
			rooms.Add(code, hostRoom);
			logBlock.InsertTextAtCursor($"user {senderId} created {code} room\n");
			RpcId(senderId, "CreateRoomAccepted");
			logBlock.InsertTextAtCursor($"Response CreateRoomAccepted to id {senderId}\n");
			RpcId(senderId, "JoinRoomAccepted", senderId);
			logBlock.InsertTextAtCursor($"Response JoinRoomAccepted with room {code} to id {senderId}\n");
		}
	}
	
	[Master]
	public void JoinRoom(string code)
	{
		int senderId = GetTree().GetRpcSenderId();
		if(rooms.ContainsKey(code))
		{
			rooms[code].Add(senderId);
			RpcId(senderId, "JoinRoomAccepted",senderId);
			logBlock.InsertTextAtCursor($"user {senderId} join to {code} room\n");
		}
		else
		{
			RpcId(senderId, "JoinRoomFail");
			logBlock.InsertTextAtCursor($"user {senderId} cant join to {code} room\n");
		}
	}

	[Master]
	public void PasswordExistRequest(string username, string hashPassword)
	{
		int senderId = GetTree().GetRpcSenderId();	
		if (USER_MANAGEMENT.PasswordExist(username, hashPassword) == true)
		{
			RpcId(senderId, "PasswordCorrect");
		}
		else
		{
			RpcId(senderId, "PasswordIncorrect");
		}
	}

	[Master]
	public void EditUserPasswordRequest(string username, string newHashedPassword)
	{
		int senderId = GetTree().GetRpcSenderId();	
		if (USER_MANAGEMENT.EditUserPassword(username, newHashedPassword) == true)
		{
			RpcId(senderId, "EditPasswordSuccessful");
		}
		else
		{
			RpcId(senderId, "EditPasswordNotSuccessful");
		}
	}
	[Master]
	public void EditNicknameRequest(string username, string nickname)
	{
		int senderId = GetTree().GetRpcSenderId();
		if (USER_MANAGEMENT.EditProfileNickname(username, nickname) == true)
		{
			RpcId(senderId, "EditNicknameSuccessful");
		}
		else
		{
			RpcId(senderId, "EditNicknameNotSuccessful");
		}
	}
	[Master]
	public void EditImageProfileRequest(string username, byte[] imageProfile)
	{
		int senderId = GetTree().GetRpcSenderId();
		if (USER_MANAGEMENT.EditProfileImage(username, imageProfile) == true)
		{
			RpcId(senderId, "EditImageProfileSuccessful");
		}
		else
		{
			RpcId(senderId, "EditImageProfileNotSuccessful");
		}
	}
}



