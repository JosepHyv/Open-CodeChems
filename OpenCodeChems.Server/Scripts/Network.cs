using Godot;
using System;
using System.Collections.Generic;
using OpenCodeChems.BussinesLogic;
using OpenCodeChems.DataAccess;
using System.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using OpenCodeChems.Server.Utils;
using OpenCodeChems.Server.Game;

namespace OpenCodeChems.Server.Network
{
	public class Network : Node
	{
		private int DEFAULT_PORT = 6007;
		private string ADDRESS = "localhost";
		private const int MAX_PLAYERS = 200;
		private readonly UserManagement USER_MANAGEMENT = new UserManagement();
		private LineEdit ipLineEdit; 
		private LineEdit portLineEdit; 
		private TextEdit logBlock;
		private RichTextLabel listening;
		private Button connectButton;
		private AcceptDialog dialog;
		private Dictionary<string, RoomGame> rooms;
		private Dictionary<int, string> roomOwners;
		private List<int> clientsConected;
		private Dictionary<int, string> playersData;
		private static Encryption PASSWORD_HASHER = new Encryption();

		public override void _Ready()
		{
			roomOwners = new Dictionary<int, string>();
			clientsConected = new List<int>();
			playersData = new Dictionary<int, string>();
			dialog = GetParent().GetNode<AcceptDialog>("Network/AcceptDialog");
			rooms = new Dictionary<string, RoomGame>();
			ipLineEdit = GetParent().GetNode<LineEdit>("Network/ip");	
			portLineEdit = GetParent().GetNode<LineEdit>("Network/puerto");
			logBlock = GetParent().GetNode<TextEdit>("Network/TextEdit");
			listening = GetParent().GetNode<RichTextLabel>("Network/currentDirText");
			connectButton = GetParent().GetNode<Button>("Network/Button");
			ipLineEdit.Text = ADDRESS;
			portLineEdit.Text = DEFAULT_PORT.ToString();
			
		}

		private void PlayerConnected(int peerId)
		{
			logBlock.InsertTextAtCursor($"Jugador = {peerId} Conectado\n");
			clientsConected.Add(peerId);
			playersData.Add(peerId, "None");
		}

		private void PlayerDisconnected(int peerId)
		{
			logBlock.InsertTextAtCursor($"Jugador = {peerId} Desconectado\n");
			Profile profileForDelete = USER_MANAGEMENT.GetProfileByUsername(peerId.ToString());
			if(profileForDelete != null)
			{
				USER_MANAGEMENT.DeleteInvitatedPlayer(profileForDelete.username);
			}
			clientsConected.Remove(peerId);
			if(roomOwners.ContainsKey(peerId))
			{
				string roomName = roomOwners[peerId];
				logBlock.InsertTextAtCursor($"{peerId} left the room {roomName}\n");
				EraseRoom(roomName);
				roomOwners.Remove(peerId);
				playersData.Remove(peerId);
			}
			DisJoinPlayer(peerId);
		}


		

		private void _on_Button_pressed()
		{
			string ipAddress = ipLineEdit.Text;
			string port = portLineEdit.Text;
			Validation validations = new Validation();
			if(validations.ValidateIp(ipAddress) && validations.ValidatePort(port))
			{
				ADDRESS = ipAddress;
				DEFAULT_PORT = Int32.Parse(port);
				connectButton.Disabled = true;
				logBlock.InsertTextAtCursor("Entrando al server OpenCodeChems\n");
				var server = new NetworkedMultiplayerENet();
				var result = server.CreateServer(DEFAULT_PORT, MAX_PLAYERS);
				if(result == 0 )
				{
					GetTree().NetworkPeer = server;
					logBlock.InsertTextAtCursor($"Hosteando server en {ADDRESS}:{DEFAULT_PORT}.\n");
					logBlock.InsertTextAtCursor($"{GetTree().NetworkPeer}\n");
					listening.Text = $"{ADDRESS}:{DEFAULT_PORT}";
				}
				logBlock.InsertTextAtCursor($"Estoy escuchando? {GetTree().IsNetworkServer()}\n");
				logBlock.InsertTextAtCursor($"Mi network ID = {GetTree().GetNetworkUniqueId()}\n");
				GetTree().Connect("network_peer_connected", this, nameof(PlayerConnected));
				GetTree().Connect("network_peer_disconnected", this, nameof(PlayerDisconnected));
			}
			else
			{
				dialog.DialogText = "INVALID_ADDRESS_OR_PORT";
				dialog.Visible = true;
			}
			
		}

		private int GetPlayerIdInRoom(string roomName, string playerName)
		{
			int idRoom = -1;
			if(rooms.ContainsKey(roomName))
			{
				List<int> playersInRoom = rooms[roomName].members;
				bool status = false;
				for(int c = 0 ; c<playersInRoom.Count && !status; c++)
				{
					int id = playersInRoom[c];
					if(playersData.ContainsKey(id))
					{
						if(playersData[id] == playerName)
						{
							status = true;
							idRoom = id;

						}
					}
				}
			}

			return idRoom;
		}
		
		private void EraseRoom(string code)
		{
			if(rooms.ContainsKey(code))
			{
				List<int> playersInRoom = rooms[code].members;
				for(int c = 0 ; c<playersInRoom.Count; c++)
				{
					int senderId = playersInRoom[c];
					logBlock.InsertTextAtCursor($"player {senderId} exiting room {code}\n");
					RpcId(senderId,"ExitRoom");
				}
				rooms.Remove(code);
			}
		}
		
		[Master]
		private void LoginRequest(string username, string password)
		{
			int senderId = GetTree().GetRpcSenderId();
			if(USER_MANAGEMENT.Login(username, password))
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

		private void DisJoinPlayer(int senderId)
		{
			foreach(KeyValuePair<string, RoomGame> roomName in rooms)
			{
				DeletePlayer(roomName.Key);
			}
		}

		[Master]
		private void DeletePlayer(string nameRoom)
		{
			int senderId = GetTree().GetRpcSenderId();
			if(roomOwners.ContainsKey(senderId))
			{
				EraseRoom(roomOwners[senderId]);
				roomOwners.Remove(senderId);

			}
			if(rooms.ContainsKey(nameRoom))
			{
				if(rooms[nameRoom].Exist(senderId))
				{
					rooms[nameRoom].RemovePlayer(senderId);
					UpdateClientsRoom(nameRoom);
				}
			}
		}

		/// <summary>
		/// call to RegisterUser and RegisterProfile method 
		/// </summary>
		/// <remarks>
		/// recive nine parameters because RPC with Godot Engine can't serialize objects, only it can recive primitive data, call the RegisterUser method if it completed correctly call the RegisterProfile method and send signal to client, reference of https://docs.godotengine.org/en/stable/tutorials/io/binary_serialization_api.html
		/// </remarks>
		/// <param name = "name"> receives a string with the name of the new user </param>
		/// <param name = "email"> receives a string with the email of the new user </param>
		/// <param name = "username"> receives a string with the username of the new user </param>
		/// <param name = "hashPassword"> receives a string with the password of the new user </param>
		/// <param name = "nickname"> receives a string with the nickname of the new user </param>
		/// <param name = "imageProfile"> receives a array of bytes with the image profile of the new user </param>
		/// <param name = "victories"> receives an int with the default victories of the new user </param>
		/// <param name = "defeats"> receives an int with the default defeats of the new user </param>
		/// <returns>Signal to client</returns>
		/// <exception cref="DbUpdateException">throw if lost connection with the database</exception>
		[Master]
		private void RegisterUserRequest(string name, string email, string username, string hashPassword, string nickname, int imageProfile, int victories, int defeats)
		{
			int senderId = GetTree().GetRpcSenderId();
			try
			{
				User newUser = new User(username, hashPassword, name, email);
				Profile newProfile = new Profile(nickname, victories, defeats, imageProfile, username);
				if(USER_MANAGEMENT.RegisterUser(newUser))
				{
					if (USER_MANAGEMENT.RegisterProfile(newProfile))
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
			if (USER_MANAGEMENT.EmailRegistered(email))
			{
				RpcId(senderId, "EmailIsRegistered");
				logBlock.InsertTextAtCursor($"the email send by user {senderId} alredy exists\n");
			}
			else
			{
				RpcId(senderId, "EmailIsNotRegistered");
				logBlock.InsertTextAtCursor($"the email send by user {senderId} not exists\n");
			}
		}
		[Master]
		private void UsernameRegisteredRequest(string username)
		{	
			int senderId = GetTree().GetRpcSenderId();	
			if (USER_MANAGEMENT.UsernameRegistered(username))
			{	
				RpcId(senderId, "UsernameIsRegistered");
				logBlock.InsertTextAtCursor($"the username send by user {senderId} alredy exists\n");
			}
			else
			{
				RpcId(senderId, "UsernameIsNotRegistered");
				logBlock.InsertTextAtCursor($"the username send by user {senderId} not exists\n");
			}
		}
		[Master]
		private void NicknameRegisteredRequest(string nickname)
		{	
			int senderId = GetTree().GetRpcSenderId();	
			if (USER_MANAGEMENT.NicknameRegistered(nickname))
			{
				RpcId(senderId, "NicknameIsRegistered");
				logBlock.InsertTextAtCursor($"the nickname send by user {senderId} alredy exists\n");
			}
			else
			{
				RpcId(senderId, "NicknameIsNotRegistered");
				logBlock.InsertTextAtCursor($"the nickname send by user {senderId} not exists\n");
			}
		}


		
		[Master]
		private void GetProfileByUsernameRequest(string username)
		{
			int senderId = GetTree().GetRpcSenderId();
			Profile profileObtained = USER_MANAGEMENT.GetProfileByUsername(username);
			if (profileObtained != null)
			{
				int idProfile = profileObtained.idProfile;
				string nickname = profileObtained.nickname;
				string usernameObtained = profileObtained.username;
				int victories = profileObtained.victories;
				int defeats = profileObtained.defeats;
				int imageProfile = profileObtained.imageProfile;
				RpcId(senderId, "ProfileByUsernameObtained", idProfile, nickname, victories, defeats, imageProfile, usernameObtained);
				logBlock.InsertTextAtCursor($"user {senderId} obtained {idProfile} profile\n");
			}
			else
			{
				RpcId(senderId, "ProfileByUsernameNotObtained");
				logBlock.InsertTextAtCursor($"user {senderId} can't obtain {username} profile\n");
			}
		}

		[Master]
		public void UpdateData(string nickname)
		{
			int senderId = GetTree().GetRpcSenderId();
			playersData[senderId] = nickname;
		}

		
		[Master]
		public void CreateRoom(string code)
		{
			int senderId = GetTree().GetRpcSenderId();
			if(rooms.ContainsKey(code))
			{
				RpcId(senderId, "CreateRoomFail");
				logBlock.InsertTextAtCursor($"user {senderId} can't create {code} room\n");
			}
			else
			{
				RoomGame hostRoom = new RoomGame();
				hostRoom.AddPlayer(senderId);
				rooms.Add(code, hostRoom);
				roomOwners.Add(senderId, code);
				logBlock.InsertTextAtCursor($"user {senderId} created {code} room\n");
				RpcId(senderId, "CreateRoomAccepted", code);
				logBlock.InsertTextAtCursor($"Response CreateRoomAccepted to id {senderId}\n");
				
			}
		}

		[Master]
		public void UpdateRol(string rol, string nameRoom)
		{
			int senderId = GetTree().GetRpcSenderId();
			if(rooms.ContainsKey(nameRoom))
			{
				if(rooms[nameRoom].CanChange(rol, senderId))
				{
					UpdateClientsRoom(nameRoom);
				}
				else
				{
					RpcId(senderId, "NoRolChanged");
				}
			}
		}

		[Master]
		public void UpdateClientsRoom(string nameRoom)
		{
			if(rooms.ContainsKey(nameRoom))
			{
				List<int> playersInRoom = rooms[nameRoom].members;
				for(int c = 0 ; c<playersInRoom.Count; c++)
				{
					int master = playersInRoom[c];
					logBlock.InsertTextAtCursor($"sending to {master} -> {playersData[master]} the players updated:\n");
					for(int d = 0 ; d<playersInRoom.Count; d++)
					{
						string separator = (d < playersInRoom.Count -1 ) ? "," : ".";
						int slave =  playersInRoom[d];
						logBlock.InsertTextAtCursor($"{playersData[slave]}{separator}");
					}
					string redSpyMaster = null;
					string blueSpyMaster = null;
					if(playersData.ContainsKey(rooms[nameRoom].redSpyMaster))
					{
						redSpyMaster = playersData[rooms[nameRoom].redSpyMaster];
					}

					if(playersData.ContainsKey(rooms[nameRoom].blueSpyMaster))
					{
						blueSpyMaster = playersData[rooms[nameRoom].blueSpyMaster];
					}
					
					List<string> redPlayers = new List<string>(); 
					List<string> bluePlayers = new List<string>();
					foreach(int id in rooms[nameRoom].redPlayers)
					{
						redPlayers.Add(playersData[id]);
					}
					foreach(int id in rooms[nameRoom].bluePlayers)
					{
						bluePlayers.Add(playersData[id]);
					}
					RpcId(master,"UpdateRoom", redSpyMaster, blueSpyMaster, redPlayers, bluePlayers);
				}
				logBlock.InsertTextAtCursor("\n");
			}
		}

		[Master]
		public void BanPlayerInRoom(string roomName, string playerName)
		{
			int senderId = GetTree().GetRpcSenderId();
			if(rooms.ContainsKey(roomName))
			{
				int uniqueId = GetPlayerIdInRoom(roomName, playerName);
				if(uniqueId != -1 && !roomOwners.ContainsKey(uniqueId))
				{
					rooms[roomName].BanPlayer(uniqueId);
					RpcId(uniqueId,"IAmBan");
					UpdateClientsRoom(roomName);
				}
				else
				{
					RpcId(senderId, "CantBan");
				}
			}
			else
			{
				RpcId(senderId, "CantBan");
			}
		}

		[Master]
		public void BanPermission()
		{
			int senderId = GetTree().GetRpcSenderId();
			if(roomOwners.ContainsKey(senderId))
			{
				RpcId(senderId, "BanPermissionAccept");
			}
		}

		[Master]
		public void JoinRoom(string code)
		{
			int senderId = GetTree().GetRpcSenderId();
			if(rooms.ContainsKey(code))
			{
				if(rooms[code].CanJoin(senderId))
				{
					rooms[code].AddPlayer(senderId);
					RpcId(senderId, "JoinRoomAccepted", code);
					logBlock.InsertTextAtCursor($"user {senderId} join to {code} room\n");
				}
				else
				{
					RpcId(senderId, "JoinRoomFail");
					logBlock.InsertTextAtCursor($"user {senderId} can't join to {code} room\n");
				}
				
			}
			else
			{
				RpcId(senderId, "JoinRoomFail");
				logBlock.InsertTextAtCursor($"user {senderId} can't join to {code} room\n");
			}
		}

		[Master]
		public void PasswordExistRequest(string username, string hashPassword)
		{
			int senderId = GetTree().GetRpcSenderId();	
			if (USER_MANAGEMENT.PasswordExist(username, hashPassword))
			{
				RpcId(senderId, "PasswordCorrect");
				logBlock.InsertTextAtCursor($"the password of user {senderId} is correct\n");
			}
			else
			{
				RpcId(senderId, "PasswordIncorrect");
				logBlock.InsertTextAtCursor($"the password of user {senderId} isn't correct\n");
			}
		}

		[Master]
		public void EditUserPasswordRequest(string username, string newHashedPassword)
		{
			int senderId = GetTree().GetRpcSenderId();	
			if (USER_MANAGEMENT.EditUserPassword(username, newHashedPassword))
			{
				RpcId(senderId, "EditPasswordSuccessful");
				logBlock.InsertTextAtCursor($"the password of user {senderId} has been update\n");
			}
			else
			{
				RpcId(senderId, "EditPasswordNotSuccessful");
				logBlock.InsertTextAtCursor($"the password of user {senderId} hasn't been update\n");
			}
		}
		[Master]
		public void EditNicknameRequest(string username, string nickname)
		{
			int senderId = GetTree().GetRpcSenderId();
			if (USER_MANAGEMENT.EditProfileNickname(username, nickname))
			{
				RpcId(senderId, "EditNicknameSuccessful");
				logBlock.InsertTextAtCursor($"the nickname of user {senderId} has been update\n");
			}
			else
			{
				RpcId(senderId, "EditNicknameNotSuccessful");
				logBlock.InsertTextAtCursor($"the password of user {senderId} hasn't been update\n");
			}
		}
		[Master]
		public void EditImageProfileRequest(string username, int imageProfile)
		{
			int senderId = GetTree().GetRpcSenderId();
			if (USER_MANAGEMENT.EditProfileImage(username, imageProfile))
			{
				RpcId(senderId, "EditImageProfileSuccessful");
				logBlock.InsertTextAtCursor($"the image profile of user {senderId} has been update\n");
			}
			else
			{
				RpcId(senderId, "EditImageProfileNotSuccessful");
				logBlock.InsertTextAtCursor($"the image profile of user {senderId} hasn´t been update\n");
			}
		}
		[Master]
		public void AddFriendRequest(int idProfileFrom, int idProfileTo, bool status)
		{
			int senderId = GetTree().GetRpcSenderId();
			
			Friends friendRequest = new Friends(idProfileFrom, idProfileTo, status);
			if (USER_MANAGEMENT.AddFriend(friendRequest))
			{
				RpcId(senderId, "AddFriendSuccessful");
				logBlock.InsertTextAtCursor($"user {senderId} can send friend request\n");
			}
			else
			{
				RpcId(senderId, "AddFriendNotSuccessful");
				logBlock.InsertTextAtCursor($"user {senderId} can't send friend request\n");
			}
		}
		[Master]
		public void FriendshipExistRequest(int idProfileFrom, int idProfileTo)
		{
			int senderId = GetTree().GetRpcSenderId();
			if(USER_MANAGEMENT.FriendshipExist(idProfileFrom, idProfileTo))
			{
				RpcId(senderId, "FriendshipIsNotRegistered");
				logBlock.InsertTextAtCursor($"No exist a friendship between {idProfileFrom} and {idProfileTo}\n");
			}
			else
			{
				RpcId(senderId, "FriendshipIsRegistered");
				logBlock.InsertTextAtCursor($"Exist a friendship between {idProfileFrom} and {idProfileTo}\n");
			}
		}
		[Master]
		public void GetFriendsRequest(int idProfile)
		{
			int senderId = GetTree().GetRpcSenderId();
			List<string> friendsObtainded = USER_MANAGEMENT.GetFriends(idProfile);
			if(friendsObtainded != null)
			{
				RpcId(senderId, "FriendsObtained", friendsObtainded);
				logBlock.InsertTextAtCursor($"Friends of {senderId} obtained\n");
			}
			else
			{
				RpcId(senderId, "FriendsNotObtained");
				logBlock.InsertTextAtCursor($"Friends of {senderId} not obtained\n");
			}
		}
		[Master]
		public void GetFriendsRequestsRequest(int idProfile)
		{
			int senderId = GetTree().GetRpcSenderId();
			List<string> friendsRequestsObtainded = USER_MANAGEMENT.GetFriendsRequests(idProfile);
			if(friendsRequestsObtainded != null)
			{
				RpcId(senderId, "FriendsRequestsObtained", friendsRequestsObtainded);
				logBlock.InsertTextAtCursor($"Friends requests of {senderId} obtained\n");
			}
			else
			{
				RpcId(senderId, "FriendsRequestsNotObtained");
				logBlock.InsertTextAtCursor($"Friends requests of {senderId} not obtained\n");
			}
		}
		[Master]
		private void GetProfileByNicknameRequest(string nickname)
		{
			int senderId = GetTree().GetRpcSenderId();
			Profile profileObtained = USER_MANAGEMENT.GetProfileByNickname(nickname);
			if (profileObtained != null)
			{
				int idProfile = profileObtained.idProfile;
				string nicknameObtained = profileObtained.nickname;
				string username = profileObtained.username;
				int victories = profileObtained.victories;
				int defeats = profileObtained.defeats;
				int imageProfile = profileObtained.imageProfile;
				RpcId(senderId, "ProfileByNicknameObtained", idProfile, nicknameObtained, victories, defeats, imageProfile, username);
				logBlock.InsertTextAtCursor($"user {senderId} obtained {idProfile} profile\n");
			}
			else
			{
				RpcId(senderId, "ProfileByNicknameNotObtained");
				logBlock.InsertTextAtCursor($"user {senderId} can't obtain {nickname} profile\n");
			}
		}
		[Master]
		private void AcceptFriendRequest(int idProfileFrom, int idProfileTo, bool status)
		{
			int senderId = GetTree().GetRpcSenderId();
			Friends friendAccepted = new Friends(idProfileFrom, idProfileTo, status);
			if (USER_MANAGEMENT.AcceptFriend(friendAccepted))
			{
				RpcId(senderId, "AcceptFriendSuccessful");
				logBlock.InsertTextAtCursor($"user {senderId} accept friend request to {idProfileTo}\n");
			}
			else
			{
				RpcId(senderId, "AcceptFriendNotSuccessful");
				logBlock.InsertTextAtCursor($"user {senderId} cant accept friend request to {idProfileTo}\n");
			}
		}
		[Master]
		private void DenyFriendRequest(int idProfileFrom, int idProfileTo, bool status)
		{
			int senderId = GetTree().GetRpcSenderId();
			Friends friendAccepted = new Friends(idProfileFrom, idProfileTo, status);
			if (USER_MANAGEMENT.DenyFriend(friendAccepted))
			{
				RpcId(senderId, "DenyFriendSuccessful");
				logBlock.InsertTextAtCursor($"user {senderId} deny friend request to {idProfileTo}\n");
			}
			else
			{
				RpcId(senderId, "DenyFriendNotSuccessful");
				logBlock.InsertTextAtCursor($"user {senderId} cant deny friend request to {idProfileTo}\n");
			}
		}
		[Master]
		private void DeleteFriendRequest(int idProfileActualPlayer, int idProfileFriend, bool status)
		{
			int senderId = GetTree().GetRpcSenderId();
			Friends friendDelete= new Friends(idProfileActualPlayer, idProfileFriend, status);
			if (USER_MANAGEMENT.SearchFriends(friendDelete))
			{
				if(USER_MANAGEMENT.DeleteFriend(friendDelete))
				{
					RpcId(senderId, "DeleteFriendSuccessful");
					logBlock.InsertTextAtCursor($"user {senderId} delete friend {idProfileFriend}\n");
				}
				else
				{
					RpcId(senderId, "DeleteFriendNotSuccessful");
					logBlock.InsertTextAtCursor($"user {senderId} can't delete friend {idProfileFriend}\n");
				}
			}
			else
			{
				friendDelete.idProfileFrom = idProfileFriend;
				friendDelete.idProfileTo = idProfileActualPlayer;
				friendDelete.status = status;
				if(USER_MANAGEMENT.DeleteFriend(friendDelete))
				{
					RpcId(senderId, "DeleteFriendSuccessful");
					logBlock.InsertTextAtCursor($"user {senderId} delete friend {idProfileFriend}\n");
				}
				else
				{
					RpcId(senderId, "DeleteFriendNotSuccessful");
					logBlock.InsertTextAtCursor($"user {senderId} can't delete friend {idProfileFriend}\n");
				}
			}
		}
		[Master]
		private void RegisterUserInvitatedRequest()
		{
			int senderId = GetTree().GetRpcSenderId();
			try
			{
				string hashPassword = PASSWORD_HASHER.ComputeSHA256Hash(senderId.ToString());
				string username = senderId.ToString();
				string name = "Chemsito" + senderId.ToString();
				string email = senderId.ToString() + "@email.com";
				string nickname = "Chemsito" + senderId.ToString();
				int victories = 0;
				int defeats = 0;
				int imageProfile = 0;
				User newUser = new User(username, hashPassword, name, email);
				Profile newProfile = new Profile(nickname, victories, defeats, imageProfile, username);
				if(USER_MANAGEMENT.RegisterUser(newUser))
				{
					if (USER_MANAGEMENT.RegisterProfile(newProfile))
					{
						RpcId(senderId, "RegisterInvitatedSuccesful", username);
						logBlock.InsertTextAtCursor($"Player no. {senderId} registered as invitated in successfully.\n");      
					}       	           
				}
				else
				{
					RpcId(senderId, "RegisterInvitatedFail");
					logBlock.InsertTextAtCursor($"Player no. {senderId} registered as invitated in failed.\n");
				}
			}
			catch (DbUpdateException)
			{
				RpcId(senderId, "RegisterInvitatedFail");
				logBlock.InsertTextAtCursor($"Player no. {senderId} registered as invitated in failed.\n");
			}

		}
		[Master]
		private void DeleteInvitatedPlayerRequest(string username)
		{
			int senderId = GetTree().GetRpcSenderId();
			if(USER_MANAGEMENT.DeleteInvitatedPlayer(username))
			{
				RpcId(senderId, "DeleteInvitatedPlayerSuccessful");
				logBlock.InsertTextAtCursor($"Player as invitated {senderId} delete in sucessfully.\n");
			}
			else
			{	
				RpcId(senderId, "DeleteInvitatedPlayerFail");
				logBlock.InsertTextAtCursor($"Player as invitated {senderId} delete in failed.\n");
			}
		}
		[Master]
		private void AddVictoryRequest(string nickname)
		{
			int senderId = GetTree().GetRpcSenderId();
			if(USER_MANAGEMENT.AddVictory(nickname))
			{
				RpcId(senderId, "AddVictorySuccessful");
				logBlock.InsertTextAtCursor($"the victories of user {senderId} has been update\n");
			}
			else
			{	
				RpcId(senderId, "AddVictoryFail");
				logBlock.InsertTextAtCursor($"the victories of user {senderId} hasn´t been update\n");
			}
		}
		[Master]
		private void AddDefeatRequest(string nickname)
		{
			int senderId = GetTree().GetRpcSenderId();
			if(USER_MANAGEMENT.AddDefeat(nickname))
			{
				RpcId(senderId, "AddDefeatSuccessful");
				logBlock.InsertTextAtCursor($"the defeats of user {senderId} has been update\n");
			}
			else
			{	
				RpcId(senderId, "AddDefeatFail");
				logBlock.InsertTextAtCursor($"the defeats of user {senderId} hasn´t been update\n");
			}
		}

		[Master]
		private void AddSceneRoom(string nameRom, int number)
		{
			if(rooms.ContainsKey(nameRom))
			{
				rooms[nameRom].SceneNumber = number;
			}
		}

		[Master]
		private void CanStart()
		{
			int senderId = GetTree().GetRpcSenderId();
			if(roomOwners.ContainsKey(senderId))
			{
				string nameRoom = roomOwners[senderId];
				if(rooms[nameRoom].CanStart())
				{
					RpcId(senderId, "Start");
				}
				else
				{
					RpcId(senderId, "NoStart");
				}
			}
		}

		[Master]
		private void IAmOwner( string roomName)
		{
			int senderId = GetTree().GetRpcSenderId();
			if(roomOwners.ContainsKey(senderId))
			{
				if(roomOwners[senderId] == roomName)
				{
					RpcId(senderId, "ICanStart");
				}
			}
		}

		[Master]
		private void UpdateGame(string roomCode)
		{
			if(rooms.ContainsKey(roomCode))
			{
				List<int> playersInRoom = rooms[roomCode].members;
				for(int c = 0; c<playersInRoom.Count; c++)
				{
					int senderId = playersInRoom[c];
					logBlock.InsertTextAtCursor($"sending request  UpdateScreenClientGame  {senderId}\n");
					RpcId(senderId, "UpdateScreenClientGame", rooms[roomCode].GetRol(senderId), rooms[roomCode].SceneNumber);
				}
			}
		}
		[Master]
		private void SendEmailRequest(string emailTo, string subject, string body)
		{
			int senderId = GetTree().GetRpcSenderId();
			var mailSender = GD.Load("res://Scripts/Utils/Email.gd");
			mailSender.Call("SendEmail", emailTo, subject, body);
			RpcId(senderId, "EmailSent");	
			logBlock.InsertTextAtCursor($"sending email to  {senderId}\n");
		}
		[Master]
		public void RestorePasswordRequest(string email, string newHashedPassword)
		{
			int senderId = GetTree().GetRpcSenderId();	
			if (USER_MANAGEMENT.RestorePassword(email, newHashedPassword))
			{
				RpcId(senderId, "RestorePasswordSuccessful");
				logBlock.InsertTextAtCursor($"the password of user {senderId} has been restore\n");
			}
			else
			{
				RpcId(senderId, "RestirePasswordNotSuccessful");
				logBlock.InsertTextAtCursor($"the password of user {senderId} hasn't been restore\n");
			}
		}
	}
}


