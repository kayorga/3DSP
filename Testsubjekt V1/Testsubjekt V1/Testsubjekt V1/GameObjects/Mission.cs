﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestsubjektV1
{
    abstract class Mission
    {
        public int target;
        internal int actCount;
        internal int tarCount;
        public byte level;
        public byte countKilledEnemies;
        public int countXPGained;
        public int dmgOut;
        public int dmgIn;
        protected String label;
        protected byte zone;
        public byte Zone { get { return zone; } }
        protected byte area;
        public byte Area { get { return area; } }
        public bool active;
        protected byte[] kinds;
        public byte[] Kinds { get { return kinds; } }
        public bool blocked;


        public TimeSpan timeSpent;

        public abstract string getShortLabel();

        public abstract bool complete();
        
        public abstract bool isType1();

        public abstract bool update(byte kind, int exp);

        public abstract void setup(byte level, byte kind, byte count, byte zone, byte area, string[] nl, string[] zl);

        public abstract void reward(Player player, ModCollection modCollection);

        public abstract void reset();

        public abstract string getLabel();
    }
}
