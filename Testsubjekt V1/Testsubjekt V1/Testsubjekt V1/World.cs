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
        char[][] mapData;
        int[][] moveData;
        ModelObject[][] mapObjects;
        ModelObject[][] ground;
        int mapID;
        public int[] player_start;
        public NPCSpawner[] spawners;

        Model groundOb;
        Model wallOb;

        public World(ContentManager Content)
        {
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

            groundOb = Content.Load<Model>("Models\\grass");
            wallOb = Content.Load<Model>("Models\\stone");
            mapID = 0;
            loadMap(Content, mapID);
            setup();
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

        private void generateGround()
        {
            for (int i = 0; i < Constants.MAP_SIZE; i++)
            {
                for (int j = 0; j < Constants.MAP_SIZE; j++)
                {
                    ground[i][j] = new ModelObject(groundOb);
                    ground[i][j].Position = new Vector3(i * -2.0f + Constants.MAP_SIZE - 1, -0.5f, -2.0f * j + Constants.MAP_SIZE - 1);
                    ground[i][j].Scaling = new Vector3(2.0f, 1, 2.0f);
                }
            }
        }

        public void setup()
        {
            generateGround();

            for (int i = 0; i < Constants.MAP_SIZE; ++i)
            {
                for (int j = 0; j < Constants.MAP_SIZE; ++j)
                {
                    switch (mapData[i][j])
                    {
                        case '0':
                            mapObjects[i][j] = null;
                            moveData[i][j] = 0; break;
                        case '1': 
                            mapObjects[i][j] = new ModelObject(wallOb);
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
                    }
                }
            }
        }

        private void loadMap(ContentManager Content, int id)
        {
            try
            {
                //create a string variable to hold the file contents
                char[][] fileContents = new char[Constants.MAP_SIZE][];
                //create a new TextReader then open the file
                TextReader reader = new StreamReader(Content.RootDirectory + "\\map" + id + ".map");
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
                mapID = 0;
                loadMap(Content, 0);
            }
        }

        public void draw(Camera camera)
        {
            for (int i = 0; i < Constants.MAP_SIZE; ++i)
            {
                for (int j = 0; j < Constants.MAP_SIZE; ++j)
                {
                    if (mapData[i][j] == '-') continue;
                    ModelObject g = ground[i][j];
                    if (g != null) g.Draw(camera);
                    ModelObject m = mapObjects[i][j];
                    if (m != null) m.Draw(camera);
                }
            }
        }
    }
}
