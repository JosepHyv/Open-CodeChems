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
        public int redSpyMaster {get; set;} = -1;

        public int blueSpyMaster {get; set;} = -1;

        public List<int> redPlayers {get; set;} = new List<int>();

        public List<int> bluePlayers {get; set;} = new List<int>();

        public List<int> boardNumbers {get;set;} = new List<int>();
        public int SceneNumber = -1;

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
                        redSpyMaster = -1;
                    }
                }
                else if (rol == Constants.BLUE_SPY_MASTER )   
                {
                    if(blueSpyMaster == uniqueId)
                    {
                        blueSpyMaster = -1;
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
                if(redSpyMaster == -1)
                {
                    DeleteRol(uniqueId);
                    redSpyMaster = uniqueId;
                    status = true;
                }   
            }
            else if(rol == Constants.BLUE_SPY_MASTER)
            {
                if(blueSpyMaster == -1)
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
                if(redSpyMaster == -1)
                {
                    status = true;
                }
                else if(blueSpyMaster == -1)
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
            
            redMasterReady = redSpyMaster != -1;
            blueMasterReady = blueSpyMaster != -1;

            redSpyReady = (redPlayers.Count > 0);
            blueSpyReady = (bluePlayers.Count > 0);

            bool status = redMasterReady && blueMasterReady && redSpyReady && blueSpyReady;
            return status;
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

    }

}