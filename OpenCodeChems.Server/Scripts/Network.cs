using Godot;
using System;
using OpenCodeChems.BussinesLogic;

public class Network : Node
{
    private int DEFAULT_PORT = 5500;
    private string ADDRESS = "localhost";
    private int MAX_PLAYERS = 200;
    private int PEERID = 1;
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
        UserManagement userManagement = new UserManagement();
        bool status = false;
        if(userManagement.Login(username, password) == true)
        {
            status = true;
            RpcId(senderId, "LoginResponse", status);
            GD.Print($"Player no. {senderId} logged in successfully.");
        }
        else
        {
            RpcId(senderId, "LoginResponse", status);
            GD.Print($"Player no. {senderId} logged in failed.");
        }
    }
}
