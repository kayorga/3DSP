﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace TestsubjektV1
{
    class ModCollection : Collection<Mod>
    {
        private Mod[] specials;
        private int[] count;
        private Mod lastGenerated;
        public Mod firstMod;
        public string lastMod { get { return lastGenerated.getLabel(); } }
        public ModCollection()
            : base(Constants.CAP_MODS)
        {
            for (int i = 0; i < Constants.CAP_MODS; i++)
            {
                _content.Add(new Mod(Constants.MOD_NIL));
            }            

            count = new int[4];
            count.Initialize();

            specials = new Mod[6];

            specials[0] = new Mod(Constants.MOD_TYP, Constants.TYP_WAV);
            specials[2] = new Mod(Constants.MOD_TYP, Constants.TYP_BLA);
            specials[4] = new Mod(Constants.MOD_TYP, Constants.TYP_TRI);

            if (Constants.DEBUG)
            {
                generate((int)Constants.ELM_HEA, Constants.MOD_ELM);
                generate((int)Constants.ELM_ICE, Constants.MOD_ELM);
                generate((int)Constants.ELM_PLA, Constants.MOD_ELM);

                generate((int)Constants.TYP_BLA, Constants.MOD_TYP);
                generate((int)Constants.TYP_WAV, Constants.MOD_TYP);
                generate((int)Constants.TYP_TRI, Constants.MOD_TYP);
            }
        }

        public void setupESpecials()
        {
            switch (firstMod.value)
            {
                case Constants.ELM_HEA:
                    specials[1] = new Mod(Constants.MOD_ELM, Constants.ELM_ICE);
                    specials[3] = new Mod(Constants.MOD_ELM, Constants.ELM_PLA);
                    break;
                case Constants.ELM_ICE:
                    specials[1] = new Mod(Constants.MOD_ELM, Constants.ELM_PLA);
                    specials[3] = new Mod(Constants.MOD_ELM, Constants.ELM_HEA);
                    break;
                case Constants.ELM_PLA:
                    specials[1] = new Mod(Constants.MOD_ELM, Constants.ELM_HEA);
                    specials[3] = new Mod(Constants.MOD_ELM, Constants.ELM_ICE);
                    break;
            }
        }

        public void update(int level)
        {
            //TODO
        }

        public override void clear()
        {
            //TODO
        }

        public void add(byte lv)
        {
            lv -= 2;
            if (lv > 24) return;
            generate(specials[lv / 4 - 1].value, specials[lv / 4 - 1].type);
        }

        public void generate(int level, int t = 0)
        {
            if (t == 0)
            {
                int m = count.Min();
                int ntype = 2;
                List<int> types = new List<int>();

                for (int i = 0; i < 4; i++)
                {
                    if (count[i] == m)
                        types.Add(i);
                }

                ntype = (new Random()).Next(types.Count);

                for (int i = 0; i < Constants.CAP_MODS; i++)
                {
                    if (_content[i].type == Constants.MOD_NIL)
                    {
                        count[types[ntype]]++;
                        _content[i].setup(types[ntype] + 3, level);
                        lastGenerated = _content[i];
                        break;
                    }
                }
            }
            else
            {
                for (int i = 0; i < Constants.CAP_MODS; i++)
                {
                    if (_content[i].type == Constants.MOD_NIL)
                    {
                        if (t > 2) count[t - 3]++;
                        _content[i].setup(t, level);
                        lastGenerated = _content[i];
                        break;
                    }
                }
            }
            _content.OrderBy(x => x.type);
        }
    }
}
