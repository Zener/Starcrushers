///////////////////////////////////////////////////////////////////////////////////////////////
// Starcrushers
// Programmed by Carlos Peris
// Barcelona, March-June 2007
///////////////////////////////////////////////////////////////////////////////////////////////



using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace ShipsWar
{
    class Player
    {
        public static bool showComputerShips = true;
        public int homePlanetID;
        public int planetFocusedID;
        public Planet homePlanet;
        public int side;
        public static float[][] sideColors;// = { { 0.0f, 1.0f, 0.0f, 1.0f }, { 1.0f, 0.0f, 0.0f, 1.0f } };
        public const int HUMAN_PLAYER = 0;
        public const int COMP_PLAYER = 1;
        public int playerType;
        public static int NUM_PLAYERS;
        public DateTime lastHitTime;
        public static Universe universe;
        public float buildTechnology;
        public float weaponTechnology;
        public float engineTechnology;
        public static Player[] players;
        public int playerCompAI;

        public static void Init(Universe _universe, int _np)
        {
            universe = _universe;
            NUM_PLAYERS = _np;
            players = new Player[NUM_PLAYERS];
        }

        
        public static Player GetPlayerBySide(int _side)
        {
            return players[_side - 1];
        }



        public Player(int _side, int _type, int _cpuType)
        {
            homePlanetID = -1;
            side = _side;
            playerType = _type;
            playerCompAI = _cpuType;

            sideColors = new float[8][];
            float[] c1 = { 0.0f, 1.0f, 0.0f, 1.0f };
            sideColors[0] = c1;
            float[] c2 = { 1.0f, 0.0f, 0.0f, 1.0f };
            sideColors[1] = c2;
            float[] c3 = { 0.0f, 0.0f, 1.0f, 1.0f };
            sideColors[2] = c3;
            float[] c4 = { 1.0f, 1.0f, 0.0f, 1.0f };
            sideColors[3] = c4;

            buildTechnology = 0.0f;
            weaponTechnology = 0.0f;
            engineTechnology = 0.0f;
        }



        public void SetHomePlanet(Planet _p, int _pid)
        {
            homePlanet = _p;
            homePlanetID = _pid;
            homePlanet.side = side;
            planetFocusedID = homePlanetID;
        }


        
        public float[] GetSideColor()
        {
            float[] c1 = { 0.5f, 0.5f, 0.5f, 0.5f };
            
            if (this.IsAlive())
            {
                return sideColors[side - 1];
            }
            else
            {
                return c1;
            }
        }

        public static float[] GetSideColor(int _side)
        {
            float[] c1 = { 0.5f, 0.5f, 0.5f, 0.5f };

            if (players != null && players[_side-1] != null && players[_side-1].IsAlive())
            {
                return sideColors[_side - 1];
            }
            else
            {
                return c1;
            }
        }


        public static Color GetSideColorClass(int _side)
        {
            float[] playerColorF = Player.GetSideColor(_side);
            Vector4 playerColorV = new Vector4(playerColorF[0], playerColorF[1], playerColorF[2], playerColorF[3]);            
            return new Color(playerColorV);
        }



        public static int PlayerWon()
        {
            int res = 0;
            int alivePlayers = 0;
            int alivePlayer = 0;

            for (int i = 1; i <= NUM_PLAYERS; i++)
            {
                if (players[i-1].IsAlive())
                {
                        alivePlayers++;
                        alivePlayer = i;
                    
                }
                /*
                for (int j = 0; j < universe.planets.Length; j++)
                {
                    if (universe.planets[j].side == i)
                    {
                        alivePlayers++;
                        alivePlayer = i;
                        break;
                    }
                }*/
            }

            if (alivePlayers == 1)
            {
                res = alivePlayer;
            }
            return res;
        }

        public bool IsAlive()
        {
            for (int j = 0; j < universe.planets.Length; j++)
            {
                if (universe.planets[j].side == side)
                {
                    return true;
                }
                // Starbase captured
                else if (universe.planets[j].homePlanet && 
                    universe.planets[j].homePlanetSide == side && 
                    universe.planets[j].homePlanetShields > 0)
                {
                    return true;
                }
            }
            return false;
        }

        public float GetOwnedPercentage()
        {
            int count = 0;
            for (int j = 0; j < universe.planets.Length; j++)
            {
                if (universe.planets[j].side == side)
                {
                    count++;
                }
            }

            return ((float)count / (float)universe.planets.Length);
        }

        public int GetOwnedPlanetsCount()
        {
            int count = 0;
            for (int j = 0; j < universe.planets.Length; j++)
            {
                if (universe.planets[j].side == side)
                {
                    count++;
                }
            }

            return count;
        }


        public float GetBuildPercentage()
        {
            return buildTechnology / 10000.0f;
        }

        public float GetWeaponPercentage()
        {
            return weaponTechnology / 10000.0f;
        }

        public float GetWarpPercentage()
        {
            return engineTechnology / 10000.0f;
        }

        public void ActionSendShip(Planet p)
        {
            if (homePlanet.side == side)
            {
                universe.SendShip(homePlanet, p);
            }
        }


        public void ActionSendShips(Planet p)
        {
            for (int i = 0; i < universe.planets.Length; i++)
            {
                if (universe.planets[i].side == side)
                {
                    universe.SendShip(universe.planets[i], p);
                }
            }
        }


        public static int GetControllerBySide(int _side)
        {
            int control = 0;
            for (int i = 0; i < NUM_PLAYERS; i++)
            {
                if (players[i].playerType == HUMAN_PLAYER)
                {
                    if (_side - 1 == control)
                    {
                        return i;
                    }
                    else
                    {
                        control++;
                    }
                }
            }
            return 0;
        }


        public static int GetHumanPlayerCount()
        {
            int sum = 0;
            for (int i = 0; i < NUM_PLAYERS; i++)
            {
                if (players[i].playerType == HUMAN_PLAYER)
                {
                    sum++;
                }
            }
            return sum;
        }
    }
}
