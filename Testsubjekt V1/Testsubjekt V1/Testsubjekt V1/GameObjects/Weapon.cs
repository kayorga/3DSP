using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace TestsubjektV1
{
    class Weapon
    {
        public int ammo;
        public int maxAmmo;
        public List<Mod> mods;
        int recharge;
        int maxRechrg;
        int cooldown;
        int maxCooldn;

        public Weapon()
        {
            //TODO
        }

        public void update(BulletCollection bullets, Vector3 position, Vector3 direction)
        {
            if (ammo < maxAmmo && recharge <= 0)
            {
                ammo++;
                recharge = maxRechrg;
            }

            if (cooldown <= 0 && (Keyboard.GetState().IsKeyDown(Keys.Space) || Mouse.GetState().LeftButton == ButtonState.Pressed))
            {
                cooldown = maxCooldn;
                bullets.generate(true, position + direction, direction, 0.8f, 80);
            }
        }
    }
}
