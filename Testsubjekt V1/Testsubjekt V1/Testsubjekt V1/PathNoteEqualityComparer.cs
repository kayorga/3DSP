using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TestsubjektV1
{
    class PathNoteEqualityComparer : IEqualityComparer<PathNode>
    {
        public bool Equals(PathNode node1, PathNode node2)
        {
            // If parameter is null return false:
            if ((object)node2 == null || (object)node1 == null)
            {
                return false;
            }

            // Return true if the fields match:
            return (node1.X == node2.X) && (node1.Y == node2.Y);
        }

        public int GetHashCode(PathNode node)
        {
            int value = node.X*1000;
            value += node.Y;
            return value;
        }
    }
}
