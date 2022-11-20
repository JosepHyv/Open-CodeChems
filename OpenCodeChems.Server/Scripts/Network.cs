using Godot;
using System;

public class Network : Node
{
    private int DEFAULT_PORT = 5500;
    private string ADDRESS = "localhost";
    private int MAX_PLAYERS = 200;
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

    [Remote]
    public void LoginPlayer(string username, string password)
    {
        int senderId = GetTree().GetRpcSenderId();
       //int senderId = 1;
        GD.Print($"quien me envia el l request = {senderId}");
        GD.Print($"Se conecto {username} con un password de {password}");
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
