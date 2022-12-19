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
        /// Status of board
        /// </summary>
        public bool [] selectedCards = new bool[25];
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
        /// number of board
        /// </summary>
        public List<int> boardNumbers {get;set;} = new List<int>();
        /// <summary>
        /// Contains the scene to be displayed
        /// </summary>
        public int sceneNumber = Constants.NULL_ROL;

        private int  blueTurn = Constants.NULL_ROL;

        private int redTurn = Constants.NULL_ROL;

        private string rolTurn = Constants.EMPTY_ROL;

        private bool  spyMasterTurn = false;

        private bool blackCard = false;

        private int maxBlueCards = Constants.EMPTY_COUNTER;

        private int maxRedCards = Constants.EMPTY_COUNTER;
        private int blueCards = Constants.EMPTY_COUNTER;

        private int redCards = Constants.EMPTY_COUNTER;

        private string teamWon = Constants.TEAM_WON;
        public void StartTurn()
        {
            blueTurn = redTurn = 0;
            spyMasterTurn = true;
            if(sceneNumber == 1 || sceneNumber == 2)
            {
                rolTurn = Constants.RED_SPY_MASTER;
                maxRedCards = 9;
                maxBlueCards = 8;
            }
            else
            {
                maxRedCards = 8;
                maxBlueCards = 9;
                rolTurn = Constants.BLUE_SPY_MASTER;
            }
        }

        public string GetTurnRol()
        {
            return rolTurn;
        }
        public int GetTurnId()
        {
            int turnId = Constants.NULL_ROL;
            if(rolTurn == Constants.RED_SPY_MASTER)
            {
                turnId = redSpyMaster;
            }
            else if (rolTurn == Constants.RED_PLAYER)
            {
                int position = redTurn % redPlayers.Count;
                turnId = redPlayers[position];
            }
            else if (rolTurn == Constants.BLUE_SPY_MASTER)
            {
                turnId = blueSpyMaster;
            }
            else
            {
                int position = blueTurn % bluePlayers.Count;
                turnId = bluePlayers[position];
            }

            return turnId;
        }

        public void NextTurn()
        {
            if(spyMasterTurn)
            {
                spyMasterTurn = false;
                
                if(rolTurn == Constants.RED_SPY_MASTER)
                {
                    rolTurn = Constants.RED_PLAYER;
                }
                else 
                {
                    rolTurn = Constants.BLUE_PLAYER;
                }
            }
            else if(rolTurn == Constants.RED_PLAYER)
            {
                int mod = redPlayers.Count;
                redTurn = (redTurn + 1 ) % mod;
            }
            else if(rolTurn == Constants.BLUE_PLAYER)
            {
                int mod = bluePlayers.Count;
                blueTurn = (blueTurn + 1 ) % mod;
            }
        }

        public void ChangeTeamTurn()
        {
            spyMasterTurn = true;
            if(rolTurn == Constants.RED_PLAYER)
            {
                rolTurn = Constants.BLUE_SPY_MASTER;
            }
            else
            {
                rolTurn = Constants.RED_SPY_MASTER;
            }
        }


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

        public int GetColor(int index)
        {
            int color = 0;
            if(sceneNumber == 0)
            {
                color = Constants.KeyBlueOne[index];
            }
            else if(sceneNumber == 1)
            {
                color = Constants.KeyRedOne[index];
            }
            else if(sceneNumber == 2)
            {
                color = Constants.KeyRedTwo[index];
            }
            else if(sceneNumber == 3)
            {
                color = Constants.KeyBlueTwo[index];
            }
            return color;
        }

        public bool GameCanContinue()
        {
            bool can = false;
            if(gameStarted && redSpyMaster != Constants.NULL_ROL && blueSpyMaster != Constants.NULL_ROL 
            && redPlayers.Count > 0  && bluePlayers.Count > 0 )
            {
                can = true;
            } 
            return can;
        }
        
        public void CountCard(int color)
        {
            if(color == Constants.BLUE)
            {
                blueCards++;
            }
            else if (color == Constants.RED)
            {
                redCards++;
            }

            if(blueCards >= maxBlueCards)
            {
                teamWon = Constants.BLUE_WON;
            }
            else if(redCards >= maxRedCards)
            {
                teamWon = Constants.RED_WON;
            }
        }

        public bool GameEnd()
        {
            bool status = false;
            status = blackCard || blueCards >= maxBlueCards || redCards >= maxRedCards;
            return status;
        }

        public void SelectedBlack()
        {
            blackCard = true;
            if(GetTurnRol() == Constants.BLUE_PLAYER || GetTurnRol() == Constants.BLUE_SPY_MASTER )
            {
                teamWon = Constants.RED_WON;
            }
            else
            {
                teamWon = Constants.BLUE_WON;
            }
        }

        public string WhoWon()
        {
            return teamWon;
        }

        public List<int> GetListWinners()
        {
            List<int> answare = new List<int>();
            if(teamWon != Constants.TEAM_WON)
            {
                if(teamWon == Constants.BLUE_WON)
                {
                    answare.Add(blueSpyMaster);
                    foreach(int id in bluePlayers)
                    {
                        answare.Add(id);
                    }
                }
                else
                {
                    answare.Add(redSpyMaster);
                    foreach(int id in redPlayers)
                    {
                        answare.Add(id);
                    }
                }
            }
            return answare;
        }

        public List<int> GetListLosers()
        {
            List<int> answare = new List<int>();
            if(teamWon != Constants.TEAM_WON)
            {
                if(teamWon == Constants.BLUE_WON)
                {
                    answare.Add(redSpyMaster);
                    foreach(int id in redPlayers)
                    {
                        answare.Add(id);
                    }
                }
                else
                {
                    answare.Add(blueSpyMaster);
                    foreach(int id in bluePlayers)
                    {
                        answare.Add(id);
                    }
                }
            }
            return answare;
        }




    }

}