using Godot;
using System;
using System.Resources;


namespace OpenCodeChems.Client.Server
{
   internal  class Network : Node
    {
        private int SERVER_ID = 1;
        private int DEFAULT_PORT = 5500;
        private int MAX_PLAYERS = 200;
        private string ADDRESS = "localhost";
        private bool connected = false;
        private bool logged = false;
        
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
            RpcId(1,"LoginRequest", username, password);
            GD.Print("Request enviado");
            
        }

        [Puppet]
        public void LoginResponse(bool status)
        {
            this.logged = status;
        }

        public bool GetLoggedResponse()
        {
            return this.logged;
        }
    }
    
}
