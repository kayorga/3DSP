using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace TestsubjektV1
{
    class PathNode : PathNoteEqualityComparer
    {
        private float hCosts;
        private float fCosts;
        private float gCosts;
        private PathNode previousNode;
        private Vector3 worldPosition;
        private int mapX;
        private int mapY;

        public PathNode(PathNode prev, int X, int Y, Point goal, float constantG)
        {
            this.mapX = X; this.mapY = Y;
            this.hCosts = Math.Max(Math.Abs(goal.X - this.mapX), Math.Abs(goal.Y - this.mapY));
            this.previousNode = prev;
            if (this.previousNode == null) this.gCosts = 1;
            else this.gCosts = this.previousNode.G + constantG;
            this.fCosts = this.hCosts + this.gCosts;
            //worldPosition = new Vector3(X * -2.0f/3.0f + Constants.MAP_SIZE - 1, 0, -2.0f * Y / 3.0f + Constants.MAP_SIZE - 1);
            worldPosition = new Vector3(X * -1.0f + Constants.MAP_SIZE - 1, 0, -1.0f * Y + Constants.MAP_SIZE - 1);
        }

        public bool Equals(PathNode node2)
        {
            // If parameter is null return false:
            if ((object)node2 == null)
            {
                return false;
            }

            // Return true if the fields match:
            return (X == node2.X) && (Y == node2.Y);
        }

        public PathNode PreviousNode
        {
            get { return previousNode; }
        }
        public int X{
            get {return mapX;}
        }
        public int Y
        {
            get { return mapY; }
        }
        public float F
        {
            get { return fCosts; }
        }
        public float G
        {
            get { return gCosts; }
        }
        public float H
        {
            get { return hCosts; }
        }
        public ulong key
        {
            //get { return fCosts * (long)(1000000000000 / (Math.Pow(10, Math.Floor(Math.Log10(fCosts))))) + hCosts * (long)(1000000000 / (Math.Pow(10, Math.Floor(Math.Log10(hCosts))))) + X * (long)(1000000 / (Math.Pow(10, Math.Floor(Math.Log10(X))))) + Y * (long)(1000 / (Math.Pow(10, Math.Floor(Math.Log10(Y))))); }
            get
            {
                ulong returnValue = (ulong) Y;
                returnValue += (ulong) X * 1000;
                returnValue += (ulong) (Math.Round(hCosts) * 1000000);
                returnValue += (ulong) Math.Round(fCosts) * 1000000000;
                return returnValue;
            }
        }

        public Vector3 Position {
            get { return worldPosition; }
        }

        public void replaceAndUpdateValues(PathNode betterNode)
        {
            this.previousNode = betterNode.PreviousNode;
            this.gCosts = betterNode.G;
            this.fCosts = betterNode.F;
        }
    }
}
