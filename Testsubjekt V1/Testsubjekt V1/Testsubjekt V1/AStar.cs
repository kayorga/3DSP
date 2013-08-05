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
        public List<Vector3> list;
        public AStar(World w, Player p, Point origin){
            world = w;
            int xTilePlayer=(int)Math.Round(-1 * p.position.X + Constants.MAP_SIZE - 1) / 2;
            int zTilePlayer=(int)Math.Round(-1 * p.position.Z + Constants.MAP_SIZE - 1) / 2;
            //int xTileNPC=(int)Math.Round(-1 * origin.position.X + Constants.MAP_SIZE - 1) / 2;
            //int zTileNPC=(int)Math.Round(-1 * origin.position.Z + Constants.MAP_SIZE - 1) / 2;
            goal=new PathNode(null ,xTilePlayer, zTilePlayer, new Point(xTilePlayer, zTilePlayer));
            PathNode start=new PathNode(null, origin.X, origin.Y, new Point(goal.X, goal.Y));
            openList = new SortedList<ulong, PathNode>();
            openList.Add(start.key, start);
            closedList = new HashSet<PathNode>();
            list = new List<Vector3>();
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
                    while (node.PreviousNode != null)
                    {
                        list.Add(node.Position);
                        node = node.PreviousNode;
                    }
                    return node.Position;
                }
                closedList.Add(activeNode);
                expandNode(activeNode);

            } while (openList.ElementAt(0).Value!=null);
            return new Vector3(0,0,0);
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
                    if(currentNode.X+i>=0 && currentNode.Y+j>=0 && currentNode.X+i<world.MoveData.Length && currentNode.Y+i<world.MoveData.Length
                        && world.MoveData[currentNode.X+i][currentNode.Y+j]==0)
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
