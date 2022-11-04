using Godot;
using System;
using System.Resources;
using  OpenCodeChems;

public class Network : Node
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    private readonly int DEFAULT_PORT = 8000;

    private readonly int MAX_PLAYERS = 8;

    private readonly string ADDRESS = "localhost";
    
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        
    }

    public void ConnectToServer()
    {
        var peer = new NetworkedMultiplayerENet();
        peer.CreateClient(ADDRESS, DEFAULT_PORT);
        GetTree().SetNetworkPeer(peer);
        peer.Connect("Connection failed", this, "OnConnectionFailed");
        peer.Connect("Connection succeded", this, "OnConnectionSucceded");
    }

    public void OnConnectionFailed()
    {
        GD.Print("Failed to connect");
    }
    public void OnConnectionSucceded()
    {
        GD.Print("Succesfully connected");
    }
//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
