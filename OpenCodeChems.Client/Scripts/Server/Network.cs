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
		public int DEFAULT_PORT {get; set;} = 6007;
		private int MAX_PLAYERS = 200; 
		public string ADDRESS {get; set;} = "localhost";
		private int PEER_ID = 1;
		private bool connected = false;
		private bool regitered = false;
		public Profile profileByUsernameObtained = null;
		public Profile profileByNicknameObtained = null;
		public List<string> friendsObtained = null;
		public List<string> friendsRequestsObtained = null;
		public static string usernamePlayerAsInvitated = "";
		public static string currentRoom = "None";
		public List<string> boardWords = new List<string>();
		
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
		[Signal]
		delegate void ProfileByNicknameFound();
		[Signal]
		delegate void ProfileByNicknameNotFound();
		[Signal]
		delegate void CorrectAcceptFriend();
		[Signal]
		delegate void AcceptFriendFail();
		[Signal]
		delegate void CorrectDenyFriend();
		[Signal]
		delegate void DenyFriendFail();
		[Signal]
		delegate void CorrectDeleteFriend();
		[Signal]
		delegate void DeleteFriendFail();

		[Signal]
		delegate void UpdatePlayersScreen(string redMaster, string blueMaster, List<string> redPlayers, List<string> bluePlayers);
		
		[Signal]
		delegate void CleanRoom();

		[Signal]
		delegate void CantChangeRol();

		[Signal]
		delegate void InvitatedRegistered();
		[Signal]
		delegate void InvitatedRegisteredFail();
		[Signal]
		delegate void CorrectDeleteInvitated();
		[Signal]
		delegate void DeleteInvitatedFail();
		[Signal]
		delegate void CanBan();
		[Signal]
		delegate void BanFail();
		[Signal]
		delegate void CorrectAddVictory();
		[Signal]
		delegate void AddVictoryNotCorrect();
		[Signal]
		delegate void CorrectAddDefeat();
		[Signal]
		delegate void AddDefeatNotCorrect();
		[Signal]
		delegate void YesStartGame();
		[Signal]
		delegate void NoStartGame();
		
		[Signal]
		delegate void UpdateBoardSignal(string rool, int number);
		[Signal]
		delegate void EmailIsSent();
		[Signal]
		delegate void CorrectRestorePassword();
		[Signal]
		delegate void RestorePasswordFail();
		
		private NetworkedMultiplayerENet networkPeer = new NetworkedMultiplayerENet();
		public override void _Ready()
		{
 
			GetTree().Connect("connection_failed", this, nameof(OnConnectionFailed));
			GetTree().Connect("connected_to_server", this, nameof(OnConnectedToServer));
			GetTree().Connect("server_disconnected", this, nameof(LeaveServer));
		}
		
		public void CloseConnection()
		{
			networkPeer.CloseConnection(1);
			return;
		}
		
		public void LeaveServer()
		{
			CloseConnection();
			GetTree().ChangeScene("res://Scenes/connexion.tscn");
			EmitSignal(nameof(ServerDead));
		}

		public void UpdateServerData(string nickname)
		{
			RpcId(PEER_ID, "UpdateData", nickname);
		}

		public void UpdateServerLanguage(string lang)
		{
			RpcId(PEER_ID, "UpdateLanguage", lang);
		}
		public void ConnectToServer()
		{
			networkPeer.CreateClient(ADDRESS, DEFAULT_PORT);
			GetTree().NetworkPeer = networkPeer;			
		}

		public void OnConnectionFailed()
		{
			EmitSignal(nameof(ServerFail));
		}
		public void OnConnectedToServer()
		{
			EmitSignal(nameof(Server));
		}

	   
		public void Login(string username, string password )
		{
			RpcId(PEER_ID,"LoginRequest", username, password);
		}

		public void LogOut()
		{
			profileByUsernameObtained = null;
			profileByNicknameObtained = null;
			friendsObtained = null;
			friendsRequestsObtained = null;

		}

		public void LeftRoom()
		{
			RpcId(PEER_ID, "DeletePlayer", currentRoom);
		}

		[Puppet]
		public void ExitRoom()
		{
			EmitSignal(nameof(CleanRoom));
		}

		[Puppet]
		public void LoginSuccesful()
		{
			EmitSignal(nameof(LoggedIn));
		}

		[Puppet]
		public void LoginFailed()
		{
			EmitSignal(nameof(LoggedFail));
		}

		public void RegisterUser(string name, string email, string username, string hashPassword, string nickname, int imageProfile, int victories, int defeats)
		{
			RpcId(PEER_ID,"RegisterUserRequest", name, email, username, hashPassword, nickname,imageProfile, victories, defeats);		
		}
		
		
		[Puppet]
		public void RegisterSuccesful()
		{
			EmitSignal(nameof(Registered));
		}
		
		[Puppet]
		public void RegisterFail()
		{
			EmitSignal(nameof(RegisteredFail));
		}

		public bool GetRegisteresResponse()
		{
			return this.regitered;
		}

		public void ChangeRolTo(string rol)
		{
			RpcId(PEER_ID, "UpdateRol", rol, currentRoom);
		}

		[Puppet]
		public void NoRolChanged()
		{
			EmitSignal(nameof(CantChangeRol));
		}

		public void EmailRegister(string email)
		{
			RpcId(PEER_ID,"EmailRegisteredRequest", email);
			
		}
		public void UsernameRegister(string username)
		{
			RpcId(PEER_ID,"UsernameRegisteredRequest", username);
			
		}
		public void NicknameRegister(string nickname)
		{
			RpcId(PEER_ID,"NicknameRegisteredRequest", nickname);
			
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
			RpcId(PEER_ID, "CreateRoom", name);
		}
		
		public void ClientJoinRoom(string name)
		{
			RpcId(PEER_ID, "JoinRoom", name);
		}
		public void RoomCreated()
		{
			RpcId(PEER_ID, "UpdateClientsRoom", currentRoom);
		}

		[Puppet]
		public void UpdateRoom(string redMaster, string blueMaster, List<string> redPlayers, List<string> bluePlayers)
		{
			EmitSignal(nameof(UpdatePlayersScreen), redMaster, blueMaster, redPlayers, bluePlayers);
		}
		
		[Puppet]
		public void CreateRoomAccepted(string nameRoom)
		{
			currentRoom = nameRoom;
			EmitSignal(nameof(RoomCreation));
		}
		
		[Puppet]
		public void CreateRoomFail()
		{
			EmitSignal(nameof(RoomCreationFail));

		}
		
		[Puppet]
		public void JoinRoomAccepted(string nameRoom)
		{
			currentRoom = nameRoom;
			EmitSignal(nameof(RoomJoin));			
		}
		
		[Puppet]
		public void JoinRoomFail()
		{
			currentRoom = "None";
			EmitSignal(nameof(RoomJoinFail));
		}

		public void GetProfileByUsername(string username)
		{
			RpcId(PEER_ID,"GetProfileByUsernameRequest", username);
		}
		[Puppet]
		public void ProfileByUsernameObtained(int idProfile, string nickname, int victories, int defeats, int imageProfile, string username)
		{
			profileByUsernameObtained = new Profile(idProfile, nickname, victories, defeats, imageProfile, username);
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
		public void EditImageProfile(string username, int imageProfile )
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
		public void GetProfileByNickname(string nickname)
		{
			RpcId(PEER_ID,"GetProfileByNicknameRequest", nickname);
		}
		[Puppet]
		public void ProfileByNicknameObtained(int idProfile, string nickname, int victories, int defeats, int imageProfile, string username)
		{
			profileByNicknameObtained = new Profile(idProfile, nickname, victories, defeats, imageProfile, username);
			EmitSignal(nameof(ProfileByNicknameFound));
		}
		[Puppet]
		public void ProfileByNicknameNotObtained()
		{
			EmitSignal(nameof(ProfileByNicknameNotFound));
		}
		public void AcceptFriend(int idProfileFrom, int idProfileTo, bool status)
		{
			RpcId(PEER_ID, "AcceptFriendRequest", idProfileFrom, idProfileTo, status);
		}
		[Puppet]
		public void AcceptFriendSuccessful()
		{
			EmitSignal(nameof(CorrectAcceptFriend));
		}
		[Puppet]
		public void AcceptFriendNotSuccessful()
		{
			EmitSignal(nameof(AcceptFriendFail));
		}
		public void DenyFriend(int idProfileFrom, int idProfileTo, bool status)
		{
			RpcId(PEER_ID, "DenyFriendRequest", idProfileFrom, idProfileTo, status);
		}
		[Puppet]
		public void DenyFriendSuccessful()
		{
			EmitSignal(nameof(CorrectDenyFriend));
		}
		[Puppet]
		public void DenyFriendNotSuccessful()
		{
			EmitSignal(nameof(DenyFriendFail));
		}
		public void DeleteFriend(int idProfileActualPlayer, int idProfileFriend, bool status)
		{
			RpcId(PEER_ID, "DeleteFriendRequest", idProfileActualPlayer, idProfileFriend, status);
		}
		[Puppet]
		public void DeleteFriendSuccessful()
		{
			EmitSignal(nameof(CorrectDeleteFriend));
		}
		[Puppet]
		public void DeleteFriendNotSuccessful()
		{
			EmitSignal(nameof(DeleteFriendFail));
		}

		public void RegisterUserInvitated()
		{
			RpcId(PEER_ID, "RegisterUserInvitatedRequest");
		}
		[Puppet]
		public void RegisterInvitatedSuccesful(string username)
		{
			usernamePlayerAsInvitated = username;
			EmitSignal(nameof(InvitatedRegistered));
		}
		[Puppet]
		public void RegisterInvitatedFail()
		{
			EmitSignal(nameof(InvitatedRegisteredFail));
		}
		public void DeleteInvitatedPlayer(string username)
		{
			RpcId(PEER_ID, "DeleteInvitatedPlayerRequest", username);
		}
		[Puppet]
		public void DeleteInvitatedPlayerSuccessful()
		{
			EmitSignal(nameof(CorrectDeleteInvitated));
		}
    	[Puppet]
		public void DeleteInvitatedPlayerFail()
		{
			EmitSignal(nameof(DeleteInvitatedFail));
    	}

		[Puppet]
		public void BanPlayer(string playerName)
		{
			RpcId(PEER_ID, "BanPlayerInRoom", currentRoom, playerName);
		}

		public void BanPermission()
		{
			RpcId(PEER_ID, "BanPermission");
		}

		[Puppet]
		public void BanPermissionAccept()
		{
			EmitSignal(nameof(CanBan));
		}

		[Puppet]
		public void CantBan()
		{
			EmitSignal(nameof(BanFail));
		}

		[Puppet]
		public void IAmBan()
		{
			EmitSignal(nameof(CleanRoom));
		}
		
		public void StartGame()
		{
			RpcId(PEER_ID, "CanStart");
		}


		[Puppet]
		public void Start()
		{
			EmitSignal(nameof(YesStartGame));
		}

		[Puppet]
		public void NoStart()
		{
			EmitSignal(nameof(NoStartGame));
		}


		public void ClientsChangeScene()
		{
			RpcId(PEER_ID, "UpdateGame", currentRoom);
		}
		
		
		
		public void AddVictory(string nickname)
		{
			RpcId(PEER_ID,"AddVictoryRequest", nickname);;
			
		}
		[Puppet]
		public void AddVictorySuccessful()
		{
			EmitSignal(nameof(CorrectAddVictory));
		}
		[Puppet]
		public void AddVictoryFail()
		{
			EmitSignal(nameof(AddVictoryNotCorrect));
		}
		public void AddDefeat(string nickname)
		{
			RpcId(PEER_ID,"AddDefeatRequest", nickname);
			
		}
		[Puppet]
		public void AddDefeatSuccessful()
		{
			EmitSignal(nameof(CorrectAddDefeat));
		}
		[Puppet]
		public void AddDefeatFail()
		{
			EmitSignal(nameof(AddDefeatNotCorrect));
		}
		
		[Puppet]
		public void UpdateScreenClientGame(List<string> words)
		{
			GD.Print($"we got algo");
			boardWords = words;
			GetTree().ChangeScene("res://Scenes/KeyController.tscn");
			GD.Print("Se cambio a la escena Keys Controller");
			RpcId(PEER_ID, "BoardChange", currentRoom);
			
		}

		[Puppet]
		public void UpdateBoard(string rool, int number)
		{
			EmitSignal(nameof(UpdateBoardSignal), rool, number);
		}
		public void SendEmail(string emailTo, string subject, string body)
		{
			RpcId(PEER_ID, "SendEmailRequest", emailTo, subject, body);
		}
		[Puppet]
		public void EmailSent()
		{
			EmitSignal(nameof(EmailIsSent));
		}
		public void RestorePassword(string email, string newHashPassword)
		{
			RpcId(PEER_ID, "RestorePasswordRequest", email, newHashPassword);
		}
		[Puppet]
		public void RestorePasswordSuccessful()
		{
			EmitSignal(nameof(CorrectRestorePassword));
		}
		[Puppet]
		public void RestorePasswordNotSuccessful()
		{
			EmitSignal(nameof(RestorePasswordFail));
		}

	}
}
