using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace A_Star_Pathfinding
{
    class Node
    {
        public Point Loc;
        
        public Node parent;
        public double fCost;
        
 
        public Node(Point Loc)
        {
            this.Loc = Loc;
        }
        public List<Node> dummyNeighbors()
        {
            List<Node> neighbors = new List<Node>();
            neighbors.Add(new Node(new Point(Loc.X - 1, Loc.Y - 1)));
            neighbors.Add(new Node(new Point(Loc.X - 1, Loc.Y)));
            neighbors.Add(new Node(new Point(Loc.X - 1, Loc.Y + 1)));

            neighbors.Add(new Node(new Point(Loc.X, Loc.Y - 1)));
            neighbors.Add(new Node(new Point(Loc.X, Loc.Y + 1)));

            neighbors.Add(new Node(new Point(Loc.X + 1, Loc.Y - 1)));
            neighbors.Add(new Node(new Point(Loc.X + 1, Loc.Y)));
            neighbors.Add(new Node(new Point(Loc.X + 1, Loc.Y + 1)));

            return neighbors;
        }
        

        //G cost is when input = start 
        //H cost is when output = end
        private double nodeCost(Point input) => Math.Sqrt(Math.Pow(input.X - Loc.X, 2) + Math.Pow(input.X - Loc.Y, 2));
        private double cost(Point input) => Math.Abs(Loc.X - input.X - Loc.Y + input.Y) + Math.Sqrt(2) * Math.Min(Math.Abs(input.X - Loc.X), Math.Abs(input.Y - Loc.Y));

        //    public double Fcost(Point start, Point finish) => nodeCost(start) + nodeCost(finish);
        public double F(Point start, Point end) => cost(start) + cost(end);
        
                

    }   
}

