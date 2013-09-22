using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

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
        private bool isBoss;
        
        public AStar(World w, Player p, Point o, NPCCollection col){
            world = w;
            npcs = col;
            origin = o;            
            list = new List<Vector3>();
            isBoss = false;
            PathNoteEqualityComparer comparer = new PathNoteEqualityComparer();
            openList = new SortedList<ulong, PathNode>();
            closedList = new HashSet<PathNode>(comparer);
            setup(origin, p);
        }

        internal PathNode PathNode
        {
            get
            {
                throw new System.NotImplementedException();
            }
            set
            {
            }
        }

        public void setup(Point o, Player p)
        {
            origin = o;
            //int xTilePlayer = (int)Math.Round((-1 * p.position.X + Constants.MAP_SIZE - 1) * 3.0f / 2.0f);
            //int zTilePlayer = (int)Math.Round((-1 * p.position.Z + Constants.MAP_SIZE - 1) * 3.0f / 2.0f);
            int xTilePlayer = (int)Math.Round((-1 * p.position.X + Constants.MAP_SIZE - 1));
            int zTilePlayer = (int)Math.Round((-1 * p.position.Z + Constants.MAP_SIZE - 1));
            //int xTileNPC=(int)Math.Round(-1 * origin.position.X + Constants.MAP_SIZE - 1) / 2;
            //int zTileNPC=(int)Math.Round(-1 * origin.position.Z + Constants.MAP_SIZE - 1) / 2;
            goal = new PathNode(null, xTilePlayer, zTilePlayer, new Point(xTilePlayer, zTilePlayer),1);
            PathNode start = new PathNode(null, origin.X, origin.Y, new Point(goal.X, goal.Y),1);
            openList.Clear();
            openList.Add(start.key, start);
            closedList.Clear();
        }

        public Vector3 findPath(bool isBoss = false)
        {
            this.isBoss = isBoss;
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

            } while (openList.Count>0 /*&&closedList.Count<world.size*3*/);
            return new Vector3(origin.X * -1.0f + Constants.MAP_SIZE - 1, 0, -1.0f * origin.Y + Constants.MAP_SIZE - 1);//new Vector3(origin.X * -2.0f / 3.0f + Constants.MAP_SIZE - 1, 0, -2.0f * origin.Y / 3.0f + Constants.MAP_SIZE - 1);
        }

        private PathNode getFirstNode(SortedList<ulong, PathNode> list)
        {
            PathNode node = openList.ElementAt(0).Value;
            openList.RemoveAt(0);
            return node;
        }

        private void expandNode(PathNode currentNode)
        {
            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    if (i == 0 && j == 0) continue;
                    /*if(currentNode.X+i>=0 && currentNode.Y+j>=0 && currentNode.X+i<world.MoveData.Length*3 && currentNode.Y+i<world.MoveData.Length*3
                        && world.MoveData[(currentNode.X+i)/3][(currentNode.Y+j)/3]==0 && npcs.npcMoveData[currentNode.X+i][currentNode.Y+j]!=true)*/
                    try
                    {
                        bool condition =
                            currentNode.X + i >= 0 &&
                            currentNode.Y + j >= 0 &&
                            currentNode.X + i < world.MoveData.Length * 2 &&
                            currentNode.Y + i < world.MoveData.Length * 2 &&
                            npcs.npcMoveData[currentNode.X + i][currentNode.Y + j] == 0 &&
                            world.MoveData[(currentNode.X + i) / 2][(currentNode.Y + j) / 2] != 1;

                        if (isBoss)
                        {
                            condition = condition &&
                                (npcs.npcMoveData[currentNode.X + i][currentNode.Y + j] == 0 || npcs.npcMoveData[currentNode.X + i][currentNode.Y + j] == 1);
                        }
                        else
                            condition = condition && npcs.npcMoveData[currentNode.X + i][currentNode.Y + j] == 0;

                        if (condition)
                        {
                            float cost = (Math.Abs(i) + Math.Abs(j) == 2 ? 1.4f : 1);
                            cost = (world.MoveData[(currentNode.X + i) / 2][(currentNode.Y + j) / 2] != 2) ? cost : 100 * cost;
                            PathNode succNode = new PathNode(currentNode, currentNode.X + i, currentNode.Y + j, new Point(goal.X, goal.Y), cost);
                            if (isNodeInClosedList(succNode)) continue;

                            if (!isNodeInOpenList(succNode)) openList.Add(succNode.key, succNode);
                        }
                    }
                    catch (IndexOutOfRangeException) { continue; }
                }
            }
        }

        private bool isNodeInClosedList(PathNode node)
        {
            //foreach (PathNode node2 in closedList) if (node2.Equals(node)) return true;
            //return false;
            //problem: return value immer false - solved
            bool b = closedList.Contains(node);
            return b;
        }

        private bool isNodeInOpenList(PathNode node2)
        {
            foreach (PathNode node in openList.Values)
                if (node.Equals(node2))
                {
                    if (node.G > node2.G)
                    {
                        //problem: muss neu einsortiert werden, key muss geändert werden
                        //node.replaceAndUpdateValues(node2);
                        //openList.ElementAt(openList.IndexOfValue(node)).Key = node.key;
                        openList.RemoveAt(openList.IndexOfValue(node));
                        openList.Add(node2.key, node2);
                    }
                    return true;
                }
            return false;
        }

    }
}
