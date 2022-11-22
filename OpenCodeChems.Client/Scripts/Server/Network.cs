using Godot;
using System;
using System.Resources;
using OpenCodeChems.Objects;


namespace OpenCodeChems.Client.Server
{
   internal  class Network : Node
	{
		private int SERVER_ID = 1;
		private int DEFAULT_PORT = 5500;
		private int MAX_PLAYERS = 200;
		private string ADDRESS = "localhost";
		private int PEER_ID = 1;
		private bool connected = false;
//        private bool logged = false;
		private bool regitered = false;
		
		[Signal]
		delegate void LoggedIn();
		[Signal]
		delegate void Registered();
		[Signal]
		delegate void LoggedFail();
		[Signal]
		delegate void RegisteredFail();
		
		private NetworkedMultiplayerENet networkPeer = new NetworkedMultiplayerENet();
		public override void _Ready()
		{
 
			GetTree().Connect("connection_failed", this, nameof(OnConnectionFailed));
			GetTree().Connect("connected_to_server", this, nameof(ConnectedToServer));
		}

		public void ConnectToServer()
		{
			
			var coso = networkPeer.CreateClient(ADDRESS, DEFAULT_PORT);
			GetTree().NetworkPeer = networkPeer;
			GD.Print("Pase la asignacion");
			GD.Print($"el coso tiene {coso} y es {coso.GetType()} y el ntpeer = {networkPeer.GetType()}");
			connected = (GetTree().NetworkPeer != null);
			GD.Print($"Dentro de NETWORK class = {connected}");
		}

		public void OnConnectionFailed()
		{
			GD.Print("Failed to connect");
		}
		public void ConnectedToServer()
		{
			GD.Print($"Succesfully connected to server {GetTree().GetNetworkUniqueId()}");
			GD.Print($"Soy cliente o server (~/1) {GetTree().IsNetworkServer()}");
		}

		
	   
		public void Login(string username, string password )
		{
			GD.Print("Enviando Request al server");
			RpcId(PEER_ID,"LoginRequest", username, password);
			GD.Print("Request enviado");
			
		}

		[Puppet]
		public void LoginResponse(bool status)
		{
			
			if(status)
			{
				GD.Print("logged In successfully");
				EmitSignal(nameof(LoggedIn));
			}
			else 
			{
				GD.Print("logged failed :( ");
				EmitSignal(nameof(LoggedFail));
			}
		}

		public void RegisterUser(string name, string email, string username, string hashPassword, string nickname, byte [] imageProfile, int victories, int defaults)
		{
			GD.Print("Enviando Request al server");
			RpcId(PEER_ID,"RegisterUserRequest", name, email, username, hashPassword, nickname, imageProfile, victories, defaults);
			GD.Print("Request enviado");
			
		}
		
		[Puppet]
		public void RegisterUserResponse(bool status)
		{
			if(status)
			{
				GD.Print("Register successfully");
				EmitSignal(nameof(Registered));
			}
			else
			{
				GD.Print("Register failed");
				EmitSignal(nameof(RegisteredFail));
			}
		}

		public bool GetRegisteresResponse()
		{
			return this.regitered;
		}

	}
	
}
