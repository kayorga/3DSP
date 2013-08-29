using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace TestsubjektV1
{
    class GameData
    {
        public Player player;

        public BulletCollection bullets;
        public MissionCollection missions;
        public ModCollection mods;
        public NPCCollection npcs;
        private World world;

        public GameData(GraphicsDevice graphicsDevice, ContentManager Content, World world)
        {
            player = new Player(world, Content);
            mods = new ModCollection();
            npcs = new NPCCollection(world, Content, player, graphicsDevice);
            bullets = new BulletCollection(Content, graphicsDevice);
            missions = new MissionCollection(world, npcs);
            this.world = world;
        }

        public void loadData(int loadedPlayerLevel, int loadedPlayerXP, Weapon loadedWeapon, List<Mod> loadedMods, byte[] loadedMissionLevels, byte[] loadedMissionTKinds, byte[] loadedMissionTCounts, byte[] loadedMissionZones, byte[] loadedMissionAreas, bool[] loadedMissionStates)
        {
            this.player.level = loadedPlayerLevel;
            this.player.XP = loadedPlayerXP;
            this.player.myWeapon = loadedWeapon;
            this.mods._content = loadedMods;

            for (int i = 0; i < 4; i++)
            {
                this.missions[i] = new Type1Mission(loadedMissionLevels[i], loadedMissionTKinds[i], loadedMissionTCounts[i], loadedMissionZones[i], loadedMissionAreas[i], npcs.Labels, world.Labels, loadedMissionStates[i]);
            }
        }

    }
}
