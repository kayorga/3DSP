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
            maxAmmo = 3;
            ammo = 3;
            maxRechrg = 40;
            recharge = 0;
            maxCooldn = 10;
            cooldown = 0;
        }

        public void update(BulletCollection bullets, Vector3 position, Vector3 direction)
        {
            if (ammo < maxAmmo && recharge <= 0)
            {
                ammo++;
                recharge = maxRechrg;
            }

            if (cooldown <= 0 && (Keyboard.GetState().IsKeyDown(Keys.Space) || Mouse.GetState().LeftButton == ButtonState.Pressed) && ammo > 0)
            {
                cooldown = maxCooldn;
                bullets.generate(true, position + direction*.5f, direction, 1, 10);
                ammo--;
            }

            cooldown = Math.Max(cooldown - 1, 0);
            recharge = Math.Max(recharge - 1, 0);
        }
    }
}
