using Godot;
using System;
using System.Collections.Generic;
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
		public Profile profileObtained = null;
		public List<string> friendsObtained = null;
		public List<string> friendsRequestsObtained = null;
		
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
		delegate void ProfileByUsernameFound();
		[Signal]
		delegate void ProfileByUsernameNotFound();
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
		[Signal]
		delegate void CorrectOldPassword();
		[Signal]
		delegate void IncorrectOldPassword();
		[Signal]
		delegate void CorrectEditPassword();
		[Signal]
		delegate void EditPasswordFail();
		[Signal]
		delegate void CorrectEditNickname();
		[Signal]
		delegate void EditNicknameFail();
		[Signal]
		delegate void CorrectEditImageProfile();
		[Signal]
		delegate void EditImageProfileFail();
		[Signal]
		delegate void CorrectAddFriend();
		[Signal]
		delegate void AddFriendFail();
		[Signal]
		delegate void FriendshipNotRegistered();
		[Signal]
		delegate void FriendshipRegistered();
		[Signal]
		delegate void FriendsFound();
		[Signal]
		delegate void FriendsNotFound();
		[Signal]
		delegate void FriendsRequestsFound();
		[Signal]
		delegate void FriendsRequestsNotFound();
		

		
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

		public void GetProfileByUsername(string username)
		{
			RpcId(PEER_ID,"GetProfileByUsernameRequest", username);
		}
		[Puppet]
		public void ProfileByUsernameObtained(int idProfile, string nickname, int victories, int defeats, byte[] imageProfile, string username)
		{
			profileObtained = new Profile(idProfile, nickname, victories, defeats, imageProfile, username);
			EmitSignal(nameof(ProfileByUsernameFound));
		}
		[Puppet]
		public void ProfileByUsernameNotObtained()
		{
			EmitSignal(nameof(ProfileByUsernameNotFound));
		}
		public void PasswordExist(string username, string hashPassword)
		{
			RpcId(PEER_ID, "PasswordExistRequest", username, hashPassword);
		}
		[Puppet]
		public void PasswordCorrect()
		{
			EmitSignal(nameof(CorrectOldPassword));
		}
		[Puppet]
		public void PasswordIncorrect()
		{
			EmitSignal(nameof(IncorrectOldPassword));
		}
		public void EditPassword(string username, string newHashPassword)
		{
			RpcId(PEER_ID, "EditUserPasswordRequest", username, newHashPassword);
		}
		[Puppet]
		public void EditPasswordSuccessful()
		{
			EmitSignal(nameof(CorrectEditPassword));
		}
		[Puppet]
		public void EditPasswordNotSuccessful()
		{
			EmitSignal(nameof(EditPasswordFail));
		}
		public void EditNickname(string username, string nickname)
		{
			RpcId(PEER_ID, "EditNicknameRequest", username, nickname);
		}
		[Puppet]
		public void EditNicknameSuccessful()
		{
			EmitSignal(nameof(CorrectEditNickname));
		}
		[Puppet]
		public void EditNicknameNotSuccessful()
		{
			EmitSignal(nameof(EditNicknameFail));
		}
		public void EditImageProfile(string username, byte[] imageProfile )
		{
			RpcId(PEER_ID, "EditImageProfileRequest", username, imageProfile);
		}
		[Puppet]
		public void EditImageProfileSuccessful()
		{
			EmitSignal(nameof(CorrectEditImageProfile));
		}
		[Puppet]
		public void EditImageProfileNotSuccessful()
		{
			EmitSignal(nameof(EditImageProfileFail));
		}
		public void AddFriend(int idProfileFrom, int idProfileTo, bool status)
		{
			RpcId(PEER_ID, "AddFriendRequest", idProfileFrom, idProfileTo, status);
		}
		[Puppet]
		public void AddFriendSuccessful()
		{
			EmitSignal(nameof(CorrectAddFriend));
		}
		[Puppet]
		public void AddFriendNotSuccessful()
		{
			EmitSignal(nameof(AddFriendFail));
		}
		public void FriendshipExist(int idProfileFrom, int idProfileTo)
		{
			RpcId(PEER_ID, "FriendshipExistRequest", idProfileFrom, idProfileTo);
		}
		[Puppet]
		public void FriendshipIsNotRegistered()
		{
			EmitSignal(nameof(FriendshipNotRegistered));
		}
		[Puppet]
		public void FriendshipIsRegistered()
		{
			EmitSignal(nameof(FriendshipRegistered));
		}
		public void GetFriends(int idProfile, bool status)
		{
			RpcId(PEER_ID, "GetFriendsRequest", idProfile, status);
		}
		[Puppet]
		public void FriendsObtained(List<string> friends)
		{
			friendsObtained = friends;
			EmitSignal(nameof(FriendsFound));
		}
		[Puppet]
		public void FriendsNotObtained()
		{
			EmitSignal(nameof(FriendsNotFound));
		}
		public void GetFriendsRequests(int idProfile, bool status)
		{
			RpcId(PEER_ID, "GetFriendsRequestsRequest", idProfile, status);
		}
		[Puppet]
		public void FriendsRequestsObtained(List<string> friendsRequests)
		{
			friendsRequestsObtained = friendsRequests;
			EmitSignal(nameof(FriendsRequestsFound));
		}
		[Puppet]
		public void FriendsRequestsNotObtained()
		{
			EmitSignal(nameof(FriendsRequestsNotFound));
		}
	}
}
