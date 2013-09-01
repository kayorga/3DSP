using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace TestsubjektV1
{
    class Map
    {
        private SpriteBatch spriteBatch;
        private GameData data;
        private World world;
        private Texture2D enemy;
        private Texture2D player;
        private Texture2D ground;
        private Texture2D obstacle;
        private Texture2D none;
        private Rectangle[][] mapRectangle;

        private int size;

        public Map(GameData data, World world, GraphicsDevice graphicsDevice, ContentManager Content, Point position)
        {
            spriteBatch = new SpriteBatch(graphicsDevice);
            this.data = data;
            this.world = world;

            enemy = Content.Load<Texture2D>("Map\\enemy");
            player = Content.Load<Texture2D>("Map\\player");
            ground = Content.Load<Texture2D>("Map\\ground");
            obstacle = Content.Load<Texture2D>("Map\\obstacle");
            none = Content.Load<Texture2D>("Map\\none");

            size = 5;

            mapRectangle = new Rectangle[Constants.MAP_SIZE*2][];

            for (int i = 0; i < Constants.MAP_SIZE*2; i++)
            {
                mapRectangle[i] = new Rectangle[Constants.MAP_SIZE*2];
                for (int j = 0; j < Constants.MAP_SIZE*2; j++)
                {
                    mapRectangle[i][j] = new Rectangle(position.X + i*size, position.Y + j*size, size, size);
                }
            }

        }

        public void Draw()
        {
            spriteBatch.Begin();

            for (int i = 0; i < Constants.MAP_SIZE*2; i++)
            {
                for (int j = 0; j < Constants.MAP_SIZE*2; j++)
                {
                    if (world.getMapData(Constants.MAP_SIZE - i / 2 - 1, Constants.MAP_SIZE - j / 2 - 1) != '-') spriteBatch.Draw(ground, mapRectangle[i][j], Color.White);

                    if (world.getMapData(Constants.MAP_SIZE - i / 2 - 1, Constants.MAP_SIZE - j / 2 - 1) == '1' || world.getMapData(Constants.MAP_SIZE - i / 2 - 1, Constants.MAP_SIZE - j / 2 - 1) == '#') spriteBatch.Draw(obstacle, mapRectangle[i][j], Color.White);

                    if (data.npcs.npcMoveData[Constants.MAP_SIZE * 2 - i - 1][Constants.MAP_SIZE * 2 - j - 1] != 0 && data.npcs.npcMoveData[Constants.MAP_SIZE * 2 - i - 1][Constants.MAP_SIZE * 2 - j - 1] != 255) spriteBatch.Draw(enemy, mapRectangle[i][j], Color.White);
                }
            }

            spriteBatch.Draw(player, mapRectangle[Constants.MAP_SIZE * 2 - data.player.xTile - 1][Constants.MAP_SIZE * 2 - data.player.zTile - 1], Color.White);

            spriteBatch.End();
        }
        
    }
}
