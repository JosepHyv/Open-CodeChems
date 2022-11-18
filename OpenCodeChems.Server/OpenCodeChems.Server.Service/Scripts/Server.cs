using Godot;
using System;

public class Server : Node
{
    private int DEFAULT_PORT = 5500;
    private string ADDRESS = "localhost";
    private int MAX_PLAYERS = 8;
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

        GetTree().Connect("network_peer_connected", this, nameof(PlayerConnected));

    }

    private void PlayerConnected(int peerId)
    {
        GD.Print($"Jugador = {peerId} Conectado");
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
