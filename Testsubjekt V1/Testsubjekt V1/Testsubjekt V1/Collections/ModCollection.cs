using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestsubjektV1
{
    class ModCollection : Collection<Mod>
    {
        private int z;
        private int[] count;
        public ModCollection()
            : base(Constants.CAP_MODS)
        {
            //TODO
            for (int i = 0; i < Constants.CAP_MODS; i++)
            {
                _content.Add(new Mod(Constants.MOD_NIL));
            }            

            count = new int[4];
            count.Initialize();
            z = 0;
            generate(1);
            generate(1);
            generate(1);
            generate(1);
            generate(1);
        }

        public void update(int level)
        {
            //TODO
        }

        public override void clear()
        {
            //TODO
        }

        public void generate(int level, int t = 2)
        {
            //TODO
            if (t == 2)
            {
                int m = count.Min();
                int ntype = 2;
                List<int> types = new List<int>();

                for (int i = 0; i < 4; i++)
                {
                    //if (count[i] == m) ntype = i + 2;
                    if (count[i] == m)
                    types.Add(i);
                }

                ntype = (new Random()).Next(types.Count) + 2;

                for (int i = 0; i < Constants.CAP_MODS; i++)
                {
                    if (_content[i].type == Constants.MOD_NIL)
                    {
                        count[types[ntype - 2]]++;
                        _content[i].setup(types[ntype-2]+2, level);
                        break;
                    }
                }
            }
            _content.OrderBy(x => x.type);
        }
    }
}
