using Godot;
using System;
using System.Resources;
using static OpenCodeChems.Client.Resources.Objects;


namespace OpenCodeChems.Client.Server
{
   internal  class Network : Node
	{

		private int SERVER_ID = 1;
		public int DEFAULT_PORT {get; set;} = 5500;
		private int MAX_PLAYERS = 200; 
		public string ADDRESS {get; set;} = "localhost";
		private int PEER_ID = 1;
		private bool connected = false;
		private bool regitered = false;
		
		[Signal]
		delegate void LoggedIn();
		[Signal]
		delegate void Registered();
		[Signal]
		delegate void LoggedFail();
		[Signal]
		delegate void RegisteredFail();
		[Signal]
		delegate void EmailNotRegistered();
		[Signal]
		delegate void EmailRegistered();
		[Signal]
		delegate void UsernameNotRegistered();
		[Signal]
		delegate void UsernameRegistered();
		[Signal]
		delegate void NicknameNotRegistered();
		[Signal]
		delegate void NicknameRegistered();
		[Signal]
		delegate void ProfileFound(Profile profile);
		[Signal]
		delegate void ProfileNotFound();
    [Signal]
		delegate void RoomCreation();
		[Signal]
		delegate void RoomCreationFail();
		[Signal]
		delegate void RoomJoin();
		[Signal]
		delegate void RoomJoinFail();
		[Signal]
		delegate void ServerDead();
		[Signal]
		delegate void Server();
		[Signal]
		delegate void ServerFail();
		

		
		private NetworkedMultiplayerENet networkPeer = new NetworkedMultiplayerENet();
		public override void _Ready()
		{
 
			GetTree().Connect("connection_failed", this, nameof(OnConnectionFailed));
			GetTree().Connect("connected_to_server", this, nameof(OnConnectedToServer));
			GetTree().Connect("server_disconnected", this, nameof(LeaveServer));
		}
		
		public void CloseConnection()
		{
			GD.Print("Me activaron para cerrar al conexion");
			networkPeer.CloseConnection(1);
			return;
		}
		
		public void LeaveServer()
		{
			GD.Print("Server Dead");
			CloseConnection();
			GetTree().ChangeScene("res://Scenes/connexion.tscn");
			EmitSignal(nameof(ServerDead));
		}

		public void ConnectToServer()
		{
			networkPeer.CreateClient(ADDRESS, DEFAULT_PORT);
			GetTree().NetworkPeer = networkPeer;			
		}

		public void OnConnectionFailed()
		{
			GD.Print("Failed to connect");
			EmitSignal(nameof(ServerFail));
		}
		public void OnConnectedToServer()
		{
			GD.Print($"Succesfully connected to server {GetTree().GetNetworkUniqueId()}");
			GD.Print($"Soy cliente o server (~/1) {GetTree().IsNetworkServer()}");
			EmitSignal(nameof(Server));
		}

		//cliente
	   
		public void Login(string username, string password )
		{
			GD.Print("Enviando Request al server");
			RpcId(PEER_ID,"LoginRequest", username, password);
			GD.Print("Request enviado");
			
		}

		[Puppet]
		public void LoginSuccesful()
		{
			GD.Print("Loggueado Correctamente");
			EmitSignal(nameof(LoggedIn));
		}

		[Puppet]
		public void LoginFailed()
		{
			GD.Print("Loggeo fallido ");
			EmitSignal(nameof(LoggedFail));
		}

		public void RegisterUser(string name, string email, string username, string hashPassword, string nickname, byte[] imageProfile, int victories, int defeats)
		{
			GD.Print("Enviando Request al server");
			RpcId(PEER_ID,"RegisterUserRequest", name, email, username, hashPassword, nickname,imageProfile, victories, defeats);
			GD.Print("Request enviado");
			
		}
		
		
		[Puppet]
		public void RegisterSuccesful()
		{
			GD.Print("Register successfully");
			EmitSignal(nameof(Registered));
		}
		
		[Puppet]
		public void RegisterFail()
		{
			GD.Print("Register failed");
			EmitSignal(nameof(RegisteredFail));
		}

		public bool GetRegisteresResponse()
		{
			return this.regitered;
		}

		public void EmailRegister(string email)
		{
			GD.Print("Enviando Request al server");
			RpcId(PEER_ID,"EmailRegisteredRequest", email);
			GD.Print("Request enviado");
			
		}
		public void UsernameRegister(string username)
		{
			GD.Print("Enviando Request al server");
			RpcId(PEER_ID,"UsernameRegisteredRequest", username);
			GD.Print("Request enviado");
			
		}
		public void NicknameRegister(string nickname)
		{
			GD.Print("Enviando Request al server");
			RpcId(PEER_ID,"NicknameRegisteredRequest", nickname);
			GD.Print("Request enviado");
			
		}

		[Puppet]
		public void EmailIsNotRegistered()
		{
			EmitSignal(nameof(EmailNotRegistered));
		}
		[Puppet]
		public void EmailIsRegistered()
		{
			EmitSignal(nameof(EmailRegistered));
		}
		[Puppet]
		public void UsernameIsNotRegistered()
		{
			EmitSignal(nameof(UsernameNotRegistered));
		}
		[Puppet]
		public void UsernameIsRegistered()
		{
			EmitSignal(nameof(UsernameRegistered));
		}
		[Puppet]
		public void NicknameIsNotRegistered()
		{
			EmitSignal(nameof(NicknameNotRegistered));
		}
		[Puppet]
		public void NicknameIsRegistered()
		{
			EmitSignal(nameof(NicknameRegistered));
    }
		
		public void ClientCreateRoom(string name)
		{
			GD.Print("Create Room Request send");
			RpcId(PEER_ID, "CreateRoom", name);
		}
		
		[Puppet]
		public void CreateRoomAccepted()
		{
			GD.Print("Cosito aceptado OwO");
			EmitSignal(nameof(RoomCreation));
		}
		
		[Puppet]
		public void CreateRoomFail()
		{
			GD.Print("Error creando la sala, ya existe una con la misma clave");
			EmitSignal(nameof(RoomCreationFail));

		}

		public void GetProfile(string username)
		{
			RpcId(PEER_ID,"GetProfileRequest", username);
		}
		[Puppet]
		public void ProfileObtained(string nickname, int victories, int defeats, byte[] imageProfile, string username)
		{
			Profile profileObtained = new Profile(nickname, victories, defeats, imageProfile, username);
			EmitSignal(nameof(ProfileFound), profileObtained);
		}
		[Puppet]
		public void ProfileNotObtained()
		{
			EmitSignal(nameof(ProfileNotFound));
		}
	}
	
}
