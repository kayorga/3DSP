using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace TestsubjektV1
{
    class ActionScreen : GameScreen
    {
        GameData data;
        Camera camera;
        World world;

        public ActionScreen(GameData gameData, Camera cam, World w)
        {
            Mouse.SetPosition(512, 384);
            data = gameData;
            camera = cam;
            world = w;
        }

        public override int update(GameTime gameTime)
        {
            //TODO
            camera.Update(gameTime, data.player.Position);
            data.player.update(data.bullets, camera);
            data.bullets.update(world);
            data.missions.update(data.player.level);
            data.npcs.update(data.bullets, camera, data.player, data.missions.activeMission);

            if (Keyboard.GetState().IsKeyDown(Keys.P))
                return Constants.CMD_PAUSE;

            if (Keyboard.GetState().IsKeyDown(Keys.J))
                return Constants.CMD_JOURNAL;

            if (Keyboard.GetState().IsKeyDown(Keys.M))
                return Constants.CMD_MOD;

            return Constants.CMD_NONE;
        }

        public override void draw()
        {
            //TODO
            world.draw(camera);
            data.player.draw(camera);
            data.npcs.draw(camera);
            data.bullets.draw(camera);
        }
    }
}
