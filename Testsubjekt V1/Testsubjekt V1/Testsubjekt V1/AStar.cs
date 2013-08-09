using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TestsubjektV1
{
    class AStar
    {
        private SortedList<ulong, PathNode> openList;
        private HashSet<PathNode> closedList;
        private World world;
        private PathNode goal;
        private NPCCollection npcs;
        public List<Vector3> list;
        private Point origin;
        
        public AStar(World w, Player p, Point o, NPCCollection col){
            world = w;
            npcs = col;
            origin = o;
            setup(origin, p);
            list = new List<Vector3>();
        }

        public void setup(Point o, Player p)
        {
            origin = o;
            int xTilePlayer = (int)Math.Round((-1 * p.position.X + Constants.MAP_SIZE - 1) * 3.0f / 2.0f);
            int zTilePlayer = (int)Math.Round((-1 * p.position.Z + Constants.MAP_SIZE - 1) * 3.0f / 2.0f);
            //int xTileNPC=(int)Math.Round(-1 * origin.position.X + Constants.MAP_SIZE - 1) / 2;
            //int zTileNPC=(int)Math.Round(-1 * origin.position.Z + Constants.MAP_SIZE - 1) / 2;
            goal = new PathNode(null, xTilePlayer, zTilePlayer, new Point(xTilePlayer, zTilePlayer));
            PathNode start = new PathNode(null, origin.X, origin.Y, new Point(goal.X, goal.Y));
            openList = new SortedList<ulong, PathNode>();
            openList.Add(start.key, start);
            closedList = new HashSet<PathNode>();
        }

        public Vector3 findPath()
        {
            PathNode activeNode;
            do
            {
                activeNode = getFirstNode(openList);
                if (activeNode.Equals(goal))
                {
                    PathNode node = activeNode;

                    if (node.PreviousNode == null) return node.Position;
                    while (node.PreviousNode.PreviousNode != null)
                    {
                        //list.Add(node.Position);
                        node = node.PreviousNode;
                    }
                    return node.Position;
                }
                closedList.Add(activeNode);
                expandNode(activeNode);

            } while (openList.Count>0 && openList.Count<world.size*6 && closedList.Count<world.size*3);
            return new Vector3(origin.X * -2.0f / 3.0f + Constants.MAP_SIZE - 1, 0, -2.0f * origin.Y / 3.0f + Constants.MAP_SIZE - 1);
        }

        private PathNode getFirstNode(SortedList<ulong, PathNode> list)
        {
            PathNode node = openList.ElementAt(0).Value;
            openList.RemoveAt(0);
            return node;
        }

        private void expandNode(PathNode currentNode)
        {
             for(int i=-1; i<2; i++)
                for (int j = -1; j < 2; j++)
                {
                    if(currentNode.X+i>=0 && currentNode.Y+j>=0 && currentNode.X+i<world.MoveData.Length*3 && currentNode.Y+i<world.MoveData.Length*3
                        && world.MoveData[(currentNode.X+i)/3][(currentNode.Y+j)/3]==0 && npcs.npcMoveData[currentNode.X+i][currentNode.Y+j]!=true)
                    {
                        PathNode succNode = new PathNode(currentNode, currentNode.X+i, currentNode.Y+j, new Point(goal.X, goal.Y));
                        if (isNodeInClosedList(succNode)) break;
                        
                        if (!isNodeInOpenList(succNode)) openList.Add(succNode.key, succNode);
                    }
                }
        }

        private bool isNodeInClosedList(PathNode node)
        {
            foreach (PathNode node2 in closedList) if (node2.Equals(node)) return true;
            return false;
        }

        private bool isNodeInOpenList(PathNode node2)
        {
            foreach (PathNode node in openList.Values)
                if (node.Equals(node2))
                {
                    if (node.G >= node2.G)
                    {
                        openList.RemoveAt(openList.IndexOfValue(node));
                        openList.Add(node2.key, node2);
                    }
                    return true;
                }
            return false;
        }

    }
}
