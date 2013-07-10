using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;

namespace TestsubjektV1
{
    class GameData
    {
        public Player player;

        public BulletCollection bullets;
        public MissionCollection missions;
        public ModCollection mods;
        public NPCCollection npcs;

        public GameData(ContentManager Content, World world)
        {
            player = new Player(world, Content);
            missions = new MissionCollection();
            mods = new ModCollection();
            npcs = new NPCCollection(world);
            bullets = new BulletCollection(Content);
        }
    }
}
