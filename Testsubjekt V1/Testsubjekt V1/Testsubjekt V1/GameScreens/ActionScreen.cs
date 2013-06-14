using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestsubjektV1
{
    class ActionScreen : GameScreen
    {
        GameData data;
        Camera camera;
        World world;

        public ActionScreen(GameData gameData, Camera cam, World w)
        {
            data = gameData;
            camera = cam;
            world = w;
        }

        public override int update()
        {
            //TODO

            data.player.update(data.bullets, camera);
            data.bullets.update(world);
            data.missions.update(data.player.level);
            data.npcs.update();

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
