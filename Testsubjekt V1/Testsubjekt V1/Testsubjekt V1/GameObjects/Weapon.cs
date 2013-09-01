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

        int mod_elm;
        int mod_typ;
        float mod_spd;
        int mod_cdn;
        int mod_rcg;
        int mod_str;
        int mod_acp;

        private AudioManager audio;

        public Weapon(AudioManager audio)
        {
            this.audio = audio;
            maxAmmo = 3;
            ammo = 3;
            maxRechrg = 40;
            recharge = 0;
            maxCooldn = 10;
            cooldown = 0;

            mods = new List<Mod>(4);
            mods.Add(new Mod(Constants.MOD_NIL));
            mods.Add(new Mod(Constants.MOD_NIL));
            mods.Add(new Mod(Constants.MOD_NIL));
            mods.Add(new Mod(Constants.MOD_NIL));
            resetMods();
        }

        public void update(BulletCollection bullets, Vector3 position, Vector3 direction)
        {
            if (ammo < maxAmmo + mod_acp && recharge <= 0)
            {
                ammo++;
                recharge = maxRechrg - mod_rcg;
            }

            if (cooldown <= 0 && (Keyboard.GetState().IsKeyDown(Keys.Space) || Mouse.GetState().LeftButton == ButtonState.Pressed) && ammo > 0)
            {
                cooldown = maxCooldn - mod_cdn;

                bullets.generate(true, position + direction * .5f, direction, 1 + mod_spd, 20, 20 + mod_str, (byte)mod_elm, (byte) mod_typ);
                ammo--;

                audio.playShoot(true);
            }

            cooldown = Math.Max(cooldown - 1, 0);
            recharge = Math.Max(recharge - 1, 0);
        }

        public void reload()
        {
            ammo = maxAmmo + mod_acp;
            cooldown = 0;
            recharge = 0;
        }

        public void setup()
        {
            resetMods();
            for (int i = 0; i < 4; i++)
            {
                applyMod(i);
            }
        }

        private void resetMods()
        {
            mod_typ = 0;
            mod_str = 0;
            mod_spd = 0;
            mod_rcg = 0;
            mod_elm = 0;
            mod_cdn = 0;
            mod_acp = 0;
        }

        private void applyMod(int index)
        {
            Mod m = mods[index];
            if (m == null) return;
            switch (m.type)
            {
                case Constants.MOD_NIL: break;
                case Constants.MOD_ELM: mod_elm  = m.value; break;
                case Constants.MOD_TYP: mod_typ  = m.value; break;
                case Constants.MOD_STR: mod_str += m.value * 2; break;
                case Constants.MOD_SPD: mod_spd += m.value * .1f; break;
                case Constants.MOD_RCG: mod_rcg += m.value * 4; break;
                case Constants.MOD_ACP: mod_acp += m.value * 2; break;
            }
        }
    }
}
