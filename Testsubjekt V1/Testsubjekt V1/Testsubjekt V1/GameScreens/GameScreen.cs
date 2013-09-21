using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Xml;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Intermediate;
using Microsoft.Xna.Framework.Content;

namespace TestsubjektV1
{
    abstract class GameScreen
    {
        protected ContentManager content;
        protected GraphicsDevice device;
        protected AudioManager audio;
        protected GameData data;
        protected Skybox skybox;

        public byte nextZone;
        public byte nextTheme;
        

        public GameScreen(ContentManager content, GraphicsDevice device, AudioManager audio, GameData data)
        {
            this.content = content;
            this.device = device;
            this.audio = audio;
            this.data = data;
            skybox = new Skybox(device, content, 1);
            nextTheme = 0;
            nextZone = 0;
        }

        public virtual void prepareWarp(byte id, byte th)
        {
            nextZone = id;
            nextTheme = th;
        }

        public virtual int update(GameTime gameTime)
        {
            return 0;
        }

        public abstract void draw();

        public virtual void saveGame(GameData data)
        {
            SaveData saveData;
            XmlWriter writer;
            XmlWriterSettings settings;

            saveData = new SaveData();
            saveData.playerLevel = data.player.level;
            saveData.playerXP = data.player.XP;
            saveData.weaponMods = data.player.myWeapon.mods;
            saveData.mods = data.mods._content;
            saveData.firstModValue = data.mods.firstMod.value;
            saveData.missionLevels = new byte[4];
            saveData.missionTKinds = new byte[4];
            saveData.missionTCounts = new byte[4];
            saveData.missionZones = new byte[4];
            saveData.missionAreas = new byte[4];
            saveData.missionStates = new bool[4];
            saveData.missionKinds = new byte[4][];


            for (int i = 0; i < 4; i++)
            {
                saveData.missionLevels[i] = data.missions[i].level;
                saveData.missionTKinds[i] = data.missions[i].target;
                saveData.missionTCounts[i] = data.missions[i].tarCount;
                //saveData.missionKinds[i] = data.missions[i].Kinds;
                saveData.missionZones[i] = data.missions[i].Zone;
                saveData.missionAreas[i] = data.missions[i].Area;
                saveData.missionStates[i] = data.missions[i].active;
                saveData.missionKinds[i] = data.missions[i].Kinds;
            }

            string filename = "doNotTouchThis.Never";

            settings = new XmlWriterSettings();
            settings.Indent = true;

            writer = XmlWriter.Create(filename, settings);

            using (writer)
            {
                IntermediateSerializer.Serialize(writer, saveData, null);
            }
        }

        public virtual void loadGame(GameData data)
        {
            string filename = "doNotTouchThis.Never";

            XmlReaderSettings settings = new XmlReaderSettings();
            SaveData saveData;
            XmlReader reader;

            reader = XmlReader.Create(filename, settings);
            using (reader)
            {
                saveData = IntermediateSerializer.Deserialize<SaveData>(reader, null);
            }

            byte[][] kinds = saveData.missionKinds;

            data.loadData(saveData.playerLevel, saveData.playerXP, saveData.weaponMods,
                saveData.mods, saveData.missionLevels, saveData.missionTKinds, saveData.missionTCounts,
                saveData.missionZones, saveData.missionAreas, saveData.missionStates, saveData.missionKinds, saveData.firstModValue);
        }

    }
}
