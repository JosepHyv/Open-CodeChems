using Godot;
using System;
using System.Linq;
using System.Collections.Generic;
using OpenCodeChems.Server.Utils;
using OpenCodeChems.Server.Standar;

namespace OpenCodeChems.Server.Game
{
    public class RoomGame
    {
      
        public bool gameStarted = false;
        public int numberPlayers {get;set;} = 0;
        public List<int> members {get;set;} = new List<int>();
        public List<int> blackList {get;set;} = new List<int>();
        public int redSpyMaster {get; set;} = Constants.NULL_ROL;

        public int blueSpyMaster {get; set;} = Constants.NULL_ROL;

        public List<int> redPlayers {get; set;} = new List<int>();

        public List<int> bluePlayers {get; set;} = new List<int>();

        public List<int> boardNumbers {get;set;} = new List<int>();
        
        public int sceneNumber = Constants.NULL_ROL;

        private int  blueTurn = Constants.NULL_ROL;

        private int redTurn = Constants.NULL_ROL;

        private string rolTurn = Constants.EMPTY_ROL;

        private bool  spyMasterTurn = false;

        public void StartTurn()
        {
            blueTurn = redTurn = 0;
            spyMasterTurn = true;
            if(sceneNumber == 1 || sceneNumber == 2)
            {
                rolTurn = Constants.RED_SPY_MASTER;
            }
            else
            {
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

        public void AddPlayer(int uniqueId)
        {
            AsignRandomRol(uniqueId);
            if(!members.Contains(uniqueId))
            {
                members.Add(uniqueId);
                numberPlayers = members.Count;
            }
        }

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

        public void RemovePlayer(int uniqueId)
        {
            if(members.Contains(uniqueId))
            {
                DeleteRol(uniqueId);
                members.Remove(uniqueId);
            }
        }

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

        public bool Exist(int uniqueId)
        {
            return members.Contains(uniqueId);
        }

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

        public void BanPlayer(int uniqueId)
        {
            RemovePlayer(uniqueId);
            blackList.Add(uniqueId);
        }

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
            //return status;
            return true;
        }

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
        

    }

}