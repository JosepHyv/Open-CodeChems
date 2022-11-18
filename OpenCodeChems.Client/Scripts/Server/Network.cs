using Godot;
using System;
using System.Resources;


namespace OpenCodeChems.Client.Server
{
   public class Network : Node
    {
        // Declare member variables here. Examples:
        // private int a = 2;
        // private string b = "text";
        private int DEFAULT_PORT = 5500;

        private int MAX_PLAYERS = 8;

        private string ADDRESS = "localhost";
        
        private readonly NetworkedMultiplayerENet networkPeer = new NetworkedMultiplayerENet();
        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
            ConnectToServer();
        }

        public void ConnectToServer()
        {
            
            networkPeer.CreateClient(ADDRESS, DEFAULT_PORT);
            GetTree().NetworkPeer = networkPeer;
            GD.Print("Pase la asignacion");
            networkPeer.Connect("Connection failed", this, "OnConnectionFailed");
            networkPeer.Connect("Connection succeded", this, "OnConnectionSucceded");
        }

        public void _OnConnectionFailed()
        {
            GD.Print("Failed to connect");
        }
        public void _OnConnectionSucceded()
        {
            GD.Print("Succesfully connected");
        }
    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    //  public override void _Process(float delta)
    //  {
    //      
    //  }
    }
    
}
