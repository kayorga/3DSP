using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using Testsubjekt_V1;

namespace TestsubjektV1
{
    class World
    {
        ContentManager content;
        char[][] mapData;
        int[][] moveData;
        ModelObject[][] mapObjects;
        ModelObject[][] ground;
        int _mapID;
        int _theme;
        public int theme { get { return _theme; } }
        public int mapID { get { return _mapID; } }
        public int[] player_start;
        public NPCSpawner[] spawners;

        Model[] groundObs;
        Model[] wallObs;
        Model[] wall2Obs;

        public World(ContentManager Content)
        {
            content = Content;
            int SIZE = Constants.MAP_SIZE;
            mapData = new char[SIZE][];
            mapObjects = new ModelObject[SIZE][];
            moveData = new int[SIZE][];
            ground = new ModelObject[SIZE][];
            player_start = new int[2];

            spawners = new NPCSpawner[4];

            for (int i = 0; i < 4; ++i)
            {
                spawners[i] = new NPCSpawner();
            }

            for (int i = 0; i < SIZE; ++i)
            {
                mapData[i] = new char[SIZE];
                moveData[i] = new int[SIZE];
                mapObjects[i] = new ModelObject[SIZE];
                ground[i] = new ModelObject[SIZE];
            }
            groundObs = new Model[2];
            wallObs = new Model[2];
            wall2Obs = new Model[2];

            #region Objects theme 0
            groundObs[0] = content.Load<Model>("Models\\floor_sim");
            wallObs[0] = content.Load<Model>("Models\\wall_sim");
            wall2Obs[0] = content.Load <Model>("Models\\wall_sim");
            #endregion

            #region Objects theme 1
            groundObs[1] = content.Load<Model>("Models\\grass");
            wallObs[1] = content.Load<Model>("Models\\stone");
            wall2Obs[1] = content.Load<Model>("Models\\tree1");
            #endregion

            _mapID = 0;
            _theme = 0;
            warp(_mapID, _theme);
        }

        public int[][] MoveData
        {
            get { return moveData; }
        }

        public void update(NPCCollection npcs, Player p)
        {
            foreach (NPCSpawner spawner in spawners)
                spawner.update(npcs, p);
        }

        /// <summary>
        /// generates the ground plane: sets ground[i][j] as new ModelObject(groundOb) with according coordinates
        /// </summary>
        private void generateGround(int th)
        {
            for (int i = 0; i < Constants.MAP_SIZE; i++)
            {
                for (int j = 0; j < Constants.MAP_SIZE; j++)
                {
                    ground[i][j] = new ModelObject(groundObs[th]);
                    ground[i][j].Position = new Vector3(i * -2.0f + Constants.MAP_SIZE - 1, -0.5f, -2.0f * j + Constants.MAP_SIZE - 1);
                    ground[i][j].Scaling = new Vector3(2.0f, 1, 2.0f);
                }
            }
        }

        /// <summary>
        /// calls generateGround to set the ground ModelObjects
        /// <para></para>
        /// sets moveData and mapObjects according to mapData
        /// <para> </para>
        /// sets up player and npc spawner start positions and activates npc spawners
        /// </summary>
        public void setup(int th)
        {
            _theme = th;
            generateGround(th);

            for (int i = 0; i < Constants.MAP_SIZE; ++i)
            {
                for (int j = 0; j < Constants.MAP_SIZE; ++j)
                {
                    switch (mapData[i][j])
                    {
                        #region tile cases
                        case '0':
                            mapObjects[i][j] = null;
                            moveData[i][j] = 0; break;
                        case '1': 
                            mapObjects[i][j] = new ModelObject(wallObs[th]);
                            mapObjects[i][j].Scaling = new Vector3(2.0f, 1.5f, 2.0f);
                            mapObjects[i][j].Position = new Vector3(i * -2.0f + Constants.MAP_SIZE - 1, 0.25f, -2.0f * j + Constants.MAP_SIZE - 1);
                            moveData[i][j] = 1;
                            break;
                        case '#':
                            mapObjects[i][j] = new ModelObject(wall2Obs[th]);
                            mapObjects[i][j].Scaling = new Vector3(2.0f, 1.5f, 2.0f);
                            mapObjects[i][j].Position = new Vector3(i * -2.0f + Constants.MAP_SIZE - 1, 0.25f, -2.0f * j + Constants.MAP_SIZE - 1);
                            moveData[i][j] = 1;
                            break;
                        case '-':
                            mapObjects[i][j] = null;
                            moveData[i][j] = 1;
                            break;
                        case 'x':
                            player_start[0] = i;
                            player_start[1] = j;
                            mapObjects[i][j] = null;
                            moveData[i][j] = 0;
                            break;
                        case 'A':
                            spawners[0].setPos(i, j);
                            mapObjects[i][j] = null;
                            moveData[i][j] = 0;
                            break;
                        case 'B':
                            spawners[1].setPos(i, j);
                            mapObjects[i][j] = null;
                            moveData[i][j] = 0;
                            break;
                        case 'C':
                            spawners[1].setPos(i, j);
                            mapObjects[i][j] = null;
                            moveData[i][j] = 0;
                            break;
                        case 'D':
                            spawners[1].setPos(i, j);
                            mapObjects[i][j] = null;
                            moveData[i][j] = 0;
                            break;
                        #endregion
                    }
                }
            }
        }

        /// <summary>
        /// reads map of specified ID from file "map"ID".map" to mapData
        /// <para></para>
        /// calls loadMap(0) on exception
        /// </summary>
        /// <param name="id">map ID to load</param>
        private void loadMap(int id)
        {
            try
            {
                _mapID = id;
                //create a string variable to hold the file contents
                char[][] fileContents = new char[Constants.MAP_SIZE][];
                //create a new TextReader then open the file
                TextReader reader = new StreamReader(content.RootDirectory + "\\map" + id + ".map");
                //loop through the entire file            
                for (int i = 0; i < Constants.MAP_SIZE; ++i)
                {
                    fileContents[i] = new char[Constants.MAP_SIZE];
                }

                for (int a = Constants.MAP_SIZE - 1; a >= 0; --a)
                {
                    for (int b = Constants.MAP_SIZE - 1; b >= 0; --b)
                    {
                        fileContents[b][a] = (char)reader.Read();
                        while (fileContents[b][a] == '\n' || fileContents[b][a] == '\r')
                            fileContents[b][a] = (char)reader.Read();
                    }
                }
                reader.Close();
                //return the results
                mapData = fileContents;
            }
            catch
            {
                _mapID = 0;
                loadMap(0);
            }
        }

        public void warp(int id, int th)
        {
            loadMap(id);
            setup(th);
        }

        public void draw(Camera camera, GraphicsDevice device)
        {
            //device.DepthStencilState = DepthStencilState.None;
            device.BlendState = BlendState.AlphaBlend;

            for (int i = 0; i < Constants.MAP_SIZE; ++i)
            {
                for (int j = 0; j < Constants.MAP_SIZE; ++j)
                {
                    if (mapData[i][j] == '-') continue;
                    ModelObject g = ground[i][j];
                    if (g != null) g.Draw(camera);
                    ModelObject m = mapObjects[i][j];

                    //if (mapData[i][j] == '#' && m != null)
                    //{
                    //    device.DepthStencilState = DepthStencilState.None;
                    //    m.Draw(camera);
                    //    device.DepthStencilState = DepthStencilState.Default;
                    //}
                    if (m != null) m.Draw(camera);
                }
            }

            //device.DepthStencilState = DepthStencilState.Default;
            device.BlendState = BlendState.Opaque;
        }
    }
}
