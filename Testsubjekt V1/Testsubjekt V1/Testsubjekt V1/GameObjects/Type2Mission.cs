using System;

namespace TestsubjektV1
{
    class Type2Mission : Mission
    {
        public Type2Mission()
        {
            //TODO
        }

        public override bool isType1()
        {
            return false;
        }

        public override bool update(byte kind, int exp)
        {
            //TODO
            return true;
        }

        public override bool complete()
        {
            throw new NotImplementedException();
        }

        public override string getLabel()
        {
            throw new NotImplementedException();
        }

        public override string getShortLabel(NPCCollection npcs)
        {
            throw new NotImplementedException();
        }

        public override void setup(byte level, byte kind, byte count, byte zone, byte area, byte[] kinds, string[] nl, string[] zl)
        {
            throw new NotImplementedException();
        }

        public override void reset(int l)
        {
            throw new NotImplementedException();
        }

        public override void reward(Player player, ModCollection modCollection)
        {
            throw new NotImplementedException();
        }

        public override void reset()
        {
            throw new NotImplementedException();
        }
    }
}
