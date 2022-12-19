using Godot;
using System;
using System.Linq;
using System.Collections.Generic;
using OpenCodeChems.Server.Utils;
using OpenCodeChems.Server.Standar;

namespace OpenCodeChems.Server.Game
{
    /// <summary>
    /// Control all players that connect with the rooms of the game
    /// </summary>
    public class RoomGame
    {
        /// <summary>
        /// Status of a game
        /// </summary>
        public bool gameStarted = false;
        /// <summary>
        /// Number total of players by game
        /// </summary>
        public int numberPlayers {get;set;} = 0;
        /// <summary>
        /// Contains the members of a room, in first place the owner of room
        /// </summary>
        public List<int> members {get;set;} = new List<int>();
        /// <summary>
        /// Contains the banned members of a room
        /// </summary>
        public List<int> blackList {get;set;} = new List<int>();
        /// <summary>
        /// Code identifying the red spy master
        /// </summary>
        public int redSpyMaster {get; set;} = Constants.NULL_ROL;
        /// <summary>
        /// Code indentifying the blue spy master
        /// </summary>
        public int blueSpyMaster {get; set;} = Constants.NULL_ROL;
        /// <summary>
        /// Contains the players who are red spies
        /// </summary>
        public List<int> redPlayers {get; set;} = new List<int>();
        /// <summary>
        /// Contains the players who are blue spies
        /// </summary>
        public List<int> bluePlayers {get; set;} = new List<int>();
        /// <summary>
        /// 
        /// </summary>
        public List<int> boardNumbers {get;set;} = new List<int>();
        /// <summary>
        /// Contains the scene to be displayed
        /// </summary>
        public int SceneNumber = Constants.NULL_ROL;

        /// <summary>
        /// Gets the role to which a player belongs in a room.
        /// </summary>
        /// <param name="uniqueId">id of the player who entered a room</param>
        public string GetRol(int uniqueId)
        {
            string rol = "None";
            if(uniqueId == redSpyMaster)
            {
                rol = Constants.RED_SPY_MASTER;
            }
            else if(uniqueId == blueSpyMaster)
            {
                rol = Constants.BLUE_SPY_MASTER;
            }
            else if(redPlayers.Contains(uniqueId))
            {
                rol = Constants.RED_PLAYER;
            }
            else if(bluePlayers.Contains(uniqueId))
            {
                rol = Constants.BLUE_PLAYER;
            }
            return rol;
        }
        /// <summary>
        /// Add a player to a room
        /// </summary>
        /// <param name="uniqueId">id of the player who entered a room</param>
        public void AddPlayer(int uniqueId)
        {
            AsignRandomRol(uniqueId);
            if(!members.Contains(uniqueId))
            {
                members.Add(uniqueId);
                numberPlayers = members.Count;
            }
        }
        /// <summary>
        /// Assigns a random role to a player who joins a room
        /// </summary>
        /// <param name="uniqueId"> id of the player who entered a room </param>
        public void AsignRandomRol(int uniqueId)
        {
            Random NumberGenerator = new Random();
            int number = NumberGenerator.Next(0,5) % 4;
            string rol = Constants.ROLES[number];
            while(!CanChange(rol, uniqueId))
            {
                number = NumberGenerator.Next(0,5) % 4;
                rol = Constants.ROLES[number];
            }
        }
        /// <summary>
        /// Remove a player of a room
        /// </summary>
        /// <param name="uniqueId">id of the player who entered a room</param>
        public void RemovePlayer(int uniqueId)
        {
            if(members.Contains(uniqueId))
            {
                DeleteRol(uniqueId);
                members.Remove(uniqueId);
            }
        }
        /// <summary>
        /// Remove the rol of the player who are banned
        /// </summary>
        /// <param name="uniqueId">id of the player who entered a room</param>
        public void DeleteRol(int uniqueId)
        {
            if(Exist(uniqueId))
            {
                string rol = GetRol(uniqueId);
                if(rol == Constants.RED_SPY_MASTER)
                {
                    if(redSpyMaster == uniqueId)
                    {
                        redSpyMaster = Constants.NULL_ROL;
                    }
                }
                else if (rol == Constants.BLUE_SPY_MASTER )   
                {
                    if(blueSpyMaster == uniqueId)
                    {
                        blueSpyMaster = Constants.NULL_ROL;
                    }
                }
                else if (rol == Constants.RED_PLAYER)
                {
                    if(redPlayers.Contains(uniqueId))
                    {
                        redPlayers.Remove(uniqueId);
                    }
                }
                else if (rol == Constants.BLUE_PLAYER)
                {
                    if(bluePlayers.Contains(uniqueId))
                    {
                        bluePlayers.Remove(uniqueId);
                    }
                }
                
            }
        }
        /// <summary>
        /// Evaluates the existence of a player in a room
        /// </summary>
        /// <param name="uniqueId">id of the player who entered a room</param>
        /// <returns>bool with true value if exist a player into a room</returns>
        public bool Exist(int uniqueId)
        {
            return members.Contains(uniqueId);
        }
        /// <summary>
        /// Evaluates if a player can change of rol into a room
        /// </summary>
        /// <param name="rol"> rol of a player in the room </param>
        /// <param name="uniqueId">id of the player who entered a room</param>
        /// <returns></returns>
        public bool CanChange(string rol, int uniqueId)
        {
            bool status = false;
            if(rol == Constants.RED_SPY_MASTER)
            {
                if(redSpyMaster == Constants.NULL_ROL)
                {
                    DeleteRol(uniqueId);
                    redSpyMaster = uniqueId;
                    status = true;
                }   
            }
            else if(rol == Constants.BLUE_SPY_MASTER)
            {
                if(blueSpyMaster == Constants.NULL_ROL)
                {
                    DeleteRol(uniqueId);
                    status = true;
                    blueSpyMaster = uniqueId;
                }
            }
            else if(rol == Constants.RED_PLAYER)
            {
                if(redPlayers.Count < Constants.MAX_RED_PLAYERS && !redPlayers.Contains(uniqueId))
                {
                    DeleteRol(uniqueId);
                    redPlayers.Add(uniqueId);
                    status = true;
                }
            }
            else
            {
                if(bluePlayers.Count < Constants.MAX_BLUE_PLAYERS && !bluePlayers.Contains(uniqueId))
                {
                    DeleteRol(uniqueId);
                    bluePlayers.Add(uniqueId);
                    status = true;
                }
            }

            return status;
        }

        /// <summary>
        /// Evaluates if a player can join a room
        /// </summary>
        /// <param name="uniqueId">id of the player who entered a room</param>
        /// <returns>bool with true value if a player can join a room</returns>
        public bool CanJoin(int uniqueId)
        {
            bool status = false;
            if(members.Count < Constants.MAX_MEMBERS)
            {   
                if(redSpyMaster == Constants.NULL_ROL)
                {
                    status = true;
                }
                else if(blueSpyMaster == Constants.NULL_ROL)
                {
                    status = true;
                }
                else if(redPlayers.Count < Constants.MAX_RED_PLAYERS)
                {
                    status = true;
                }
                else if(bluePlayers.Count < Constants.MAX_BLUE_PLAYERS)
                {
                    status = true;
                }
            }

            if(status)
            {

                status &= !blackList.Contains(uniqueId);

                status = status && !blackList.Contains(uniqueId);

            }

            status &= !gameStarted;

            return status;
        }
        /// <summary>
        /// Ban a player of a room
        /// </summary>
        /// <param name="uniqueId">id of the player who entered a room</param>
        public void BanPlayer(int uniqueId)
        {
            RemovePlayer(uniqueId);
            blackList.Add(uniqueId);
        }
        /// <summary>
        /// Evaluates if a game can start
        /// </summary>
        /// <returns>bool with true value if the game can start</returns>
        public bool CanStart()
        {
            bool redMasterReady = false;
            bool blueMasterReady = false;
            bool redSpyReady = false;
            bool blueSpyReady = false;
            
            redMasterReady = redSpyMaster != Constants.NULL_ROL;
            blueMasterReady = blueSpyMaster != Constants.NULL_ROL;

            redSpyReady = (redPlayers.Count > 0);
            blueSpyReady = (bluePlayers.Count > 0);

            bool status = redMasterReady && blueMasterReady && redSpyReady && blueSpyReady;
            return status;
        }
        /// <summary>
        /// Randomly selects a board
        /// </summary>
        public void GenerateBoard()
        {
            Random randomClass = new Random();
            List<int> fullList = new List<int>();
            for(int c = 0 ; c<83; c++)
            {
                fullList.Add(c);
            }

            boardNumbers = fullList.OrderBy(_ => randomClass.Next()).ToList();

        }

    }

}