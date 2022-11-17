using Godot;
using System;

public class Server : Node
{
     
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    private readonly int DEFAULT_PORT = 8000;
    
    private readonly int MAX_PLAYERS = 8;
    public override void _Ready()
    {
        GD.Print("intentando iniciar el servidor");
        GD.Print("Qué esta pasando aqui?");
        GD.Print("que esta pasando, porque no sale nada :(, me quiero morir ya");
        //StartServer();
    }

    public void StartServer()
    {
        var network = new NetworkedMultiplayerENet();
        network.CreateServer(DEFAULT_PORT, MAX_PLAYERS);
        GetTree().SetNetworkPeer(network);
        GD.Print("Server started");
        network.Connect("peerConnected", this, "PeerConnected");
        network.Connect("peerDisconnected", this, "PeerDisconnected");
    }

    public void PeerConnected()
    {
        GD.Print("User connected");
    }
    public void PeerDisconected()
    {
        GD.Print("User disconected");
    }

}
