using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;

namespace TestsubjektV1
{
    class World
    {
        ContentManager content;
        GraphicsDevice device;
        char[][] mapData;
        byte[][] moveData;
        ModelObject[][] mapObjects;
        ModelObject[][] ground;
        byte _mapID;
        byte _theme;
        private byte SIZE;
        public byte size { get { return SIZE; } }
        public byte theme { get { return _theme; } }
        public byte mapID { get { return _mapID; } }
        public int[] player_start;
        public NPCSpawner[] spawners;
        private byte spawner_count;
        public byte shootDistance { get { return maps[_mapID].attackDistance; } }
        private List<Map> maps;
        public IList<Map> Maps { get { return maps.AsReadOnly(); } }
        public char getMapData(int x, int z) { return mapData[x][z]; }
        private Skybox skybox;

        public class Map
        {
            public readonly byte spawnerCount;
            public readonly byte attackDistance;
            public Map(byte sCount, byte aDistance)
            {
                spawnerCount = sCount;
                attackDistance = aDistance;
            }
        }

        static string[] labels = { "Forest", "Desert", "Arctic"};
        public string[] Labels { get { return labels; } }

        Model[] groundObs;
        Model[] wallObs;
        Model[] wall2Obs;

        public World(ContentManager Content, GraphicsDevice gdev)
        {
            device = gdev;
            content = Content;
            SIZE = Constants.MAP_SIZE;
            mapData = new char[SIZE][];
            mapObjects = new ModelObject[SIZE][];
            moveData = new byte[SIZE][];
            ground = new ModelObject[SIZE][];
            player_start = new int[2];
            skybox = new Skybox(gdev, Content, 2);

            maps = new List<Map>(1);
            scanFiles();
            
            spawners = new NPCSpawner[4];

            for (int i = 0; i < 4; ++i)
            {
                spawners[i] = new NPCSpawner();
            }

            spawner_count = 0;

            for (int i = 0; i < SIZE; ++i)
            {
                mapData[i] = new char[SIZE];
                moveData[i] = new byte[SIZE];
                mapObjects[i] = new ModelObject[SIZE];
                ground[i] = new ModelObject[SIZE];
            }

            groundObs = new Model[4];
            wallObs = new Model[4];
            wall2Obs = new Model[4];

            #region Objects theme 0
            groundObs[0] = content.Load<Model>("Models\\floor_sim");
            wallObs[0] = content.Load<Model>("Models\\wall_sim");
            wall2Obs[0] = content.Load <Model>("Models\\mainframe");
            #endregion

            #region Objects theme 1
            groundObs[1] = content.Load<Model>("Models\\grass");
            wallObs[1] = content.Load<Model>("Models\\stone");
            wall2Obs[1] = content.Load<Model>("Models\\tree1");
            #endregion

            #region Objects theme 2
            groundObs[2] = content.Load<Model>("Models\\wasted");
            wallObs[2] = content.Load<Model>("Models\\rock_w");
            wall2Obs[2] = content.Load<Model>("Models\\cactus");
            #endregion

            #region Objects theme 3
            groundObs[3] = content.Load<Model>("Models\\arctical");
            wallObs[3] = content.Load<Model>("Models\\stone");
            wall2Obs[3] = content.Load<Model>("Models\\stalagmit");
            #endregion

            _mapID = 0;
            _theme = 0;
            warp(_mapID, _theme);
        }

        private byte readNext(TextReader reader, char stop)
        {
            string filecontents = "";
            while (true)
            {
                char c = (char)reader.Read();
                if (c == stop)
                    break;
                else
                    filecontents += c;
            }

            try
            {
                return Byte.Parse(filecontents);
            }
            catch (Exception)
            {
                return 0;
            }
        }

        private void scanFiles()
        {
            byte id = 0;
            string next_file = "\\map0.map";

            while (File.Exists(content.RootDirectory + next_file))
            {
                TextReader reader = new StreamReader(content.RootDirectory + next_file);

                byte sCount = readNext(reader, ',');
                byte aDist = readNext(reader, '\n');

                maps.Add(new Map(sCount, aDist));

                reader.Close();

                id++;
                next_file = "\\map" + id + ".map";
            }
        }

        public byte[][] MoveData
        {
            get { return moveData; }
        }

        public void update(NPCCollection npcs, Player p, Mission m, bool isMainMission = false)
        {
            foreach (NPCSpawner spawner in spawners)
                spawner.update(npcs, p, m, isMainMission);
        }

        /// <summary>
        /// generates the ground plane: sets ground[i][j] as new ModelObject(groundOb) with according coordinates
        /// </summary>
        private void generateGround(int th)
        {
            for (int i = 0; i < SIZE; i++)
            {
                for (int j = 0; j < SIZE; j++)
                {
                    ground[i][j] = new ModelObject(groundObs[th]);
                    ground[i][j].Position = new Vector3(i * -2.0f + SIZE - 1, -0.5f, -2.0f * j + SIZE - 1);
                    ground[i][j].Scaling = new Vector3(2.0f, 1, 2.0f);
                }
            }
        }

        private void setEffect(Model model, Effect effect)
        {
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (ModelMeshPart part in mesh.MeshParts)
                {
                    part.Effect = effect;
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
        public void setup(byte th)
        {
            _theme = th;
            generateGround(th);
            spawner_count = 0;
            foreach (NPCSpawner spawner in spawners)
                spawner.active = false;
            Random ran = new Random();

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
                            if (th != 0)
                                mapObjects[i][j].Rotation = new Vector3(0, (float)(ran.NextDouble() * Math.PI), 0);
                            moveData[i][j] = 1;
                            break;
                        case '-':
                            mapObjects[i][j] = null;
                            moveData[i][j] = 1;
                            break;
                        case '.':
                            mapObjects[i][j] = null;
                            moveData[i][j] = 2;
                            break;
                        case 'x':
                            player_start[0] = i;
                            player_start[1] = j;
                            mapObjects[i][j] = null;
                            moveData[i][j] = 0;
                            break;
                        case 'A':
                            spawners[0].setPos(i, j);
                            spawner_count++;
                            mapObjects[i][j] = null;
                            moveData[i][j] = 0;
                            break;
                        case 'B':
                            spawners[1].setPos(i, j);
                            spawner_count++;
                            mapObjects[i][j] = null;
                            moveData[i][j] = 0;
                            break;
                        case 'C':
                            spawners[2].setPos(i, j);
                            spawner_count++;
                            mapObjects[i][j] = null;
                            moveData[i][j] = 0;
                            break;
                        case 'D':
                            spawners[3].setPos(i, j);
                            spawner_count++;
                            mapObjects[i][j] = null;
                            moveData[i][j] = 0;
                            break;
                        #endregion
                    }
                }
            }
        }

        /// <summary>
        /// configures npc spawners to match the mission data
        /// </summary>
        public void setupSpawners(Mission m)
        {
            if (m.Kinds[0] == Constants.NPC_BOSS)
                spawners[0].setup(m.Kinds[0], Constants.SPAWN_ONCE);
            else
                spawners[0].setup(m.Kinds[0], Constants.SPAWN_INFINITE);

            for (int i = 1; i < m.Kinds.Length; i++)
                spawners[i].setup(m.Kinds[i], Constants.SPAWN_INFINITE);
        }

        /// <summary>
        /// reads map of specified ID from file "map"ID".map" to mapData
        /// <para></para>
        /// calls loadMap(0) on exception
        /// </summary>
        /// <param name="id">map ID to load</param>
        private void loadMap(byte id)
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

                //skip first line
                reader.ReadLine();

                //read map array
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

                //assign contents
                mapData = fileContents;
            }
            catch
            {
                _mapID = 0;
                loadMap(0);
            }
        }

        /// <summary>
        /// loads and generates a new map from file
        /// </summary>
        /// <param name="id">map id to load</param>
        /// <param name="th">zone theme</param>
        public void warp(byte id, byte th)
        {
            loadMap(id);
            setup(th);
            switch (th)
            {
                case 0: skybox.changeTheme(2); break;
                case 1: skybox.changeTheme(1); break;
                case 2: skybox.changeTheme(3); break;
                case 3: skybox.changeTheme(4); break;
                default: break;
            }
        }

        public void draw(Camera camera, GraphicsDevice device)
        {
            skybox.Draw(device, camera);
            //device.DepthStencilState = DepthStencilState.None;
            device.BlendState = BlendState.AlphaBlend;

            for (int i = 0; i < SIZE; ++i)
            {
                for (int j = 0; j < SIZE; ++j)
                {
                    if (mapData[i][j] == '-') continue;
                    ModelObject g = ground[i][j];
                    if (g != null) g.Draw(camera);
                    ModelObject m = mapObjects[i][j];

                    if (m != null) m.Draw(camera);
                }
            }

            //device.DepthStencilState = DepthStencilState.Default;
            device.BlendState = BlendState.Opaque;
        }
    }
}
