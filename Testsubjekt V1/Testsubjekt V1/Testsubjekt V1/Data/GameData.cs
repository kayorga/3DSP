using System.Collections.Generic;
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
        private AudioManager audio;

        public GameData(ContentManager Content, GraphicsDevice device, AudioManager audio, World world)
        {
            player = new Player(world, Content, audio);
            mods = new ModCollection();
            npcs = new NPCCollection(world, Content, player, device, audio);
            bullets = new BulletCollection(Content, device);
            missions = new MissionCollection(world, npcs);
            this.world = world;
            this.audio = audio;
        }

        public void loadData(int loadedPlayerLevel, int loadedPlayerXP, List<Mod> loadedWeaponMods, List<Mod> loadedMods, byte[] loadedMissionLevels, byte[] loadedMissionTKinds, byte[] loadedMissionTCounts, byte[] loadedMissionZones, byte[] loadedMissionAreas, bool[] loadedMissionStates, byte[][] loadedMissionKinds, int firstModValue)
        {
            this.player.level = loadedPlayerLevel;
            this.player.XP = loadedPlayerXP;
            this.player.myWeapon.mods = loadedWeaponMods;
            this.mods._content = loadedMods;
            this.mods.firstMod = new Mod(Constants.MOD_ELM, firstModValue);
            this.player.myWeapon.setup();

            for (int i = 0; i < 4; i++)
            {
                this.missions[i] = new Type1Mission(loadedMissionLevels[i], loadedMissionTKinds[i], loadedMissionTCounts[i], loadedMissionZones[i], loadedMissionAreas[i], npcs.Labels, world.Labels, loadedMissionStates[i], loadedMissionKinds[i]);
            }
        }

    }
}
