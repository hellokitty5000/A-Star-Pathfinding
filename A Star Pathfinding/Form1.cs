using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace A_Star_Pathfinding
{
    public partial class Form1 : Form
    {
        const int ARRAY_WIDTH = 30;
        const int ARRAY_HEIGHT = 20;

        List<Node> open = new List<Node>();
        List<Node> closed = new List<Node>();
        List<Point> walls = new List<Point>();

        Point start = new Point (0,0);
        Point end = new Point(0,0);
        bool assignFirst = true;

        //public variables
        Button[,] buttonGrid = new Button[ARRAY_WIDTH, ARRAY_HEIGHT];
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
           
            for (int i = 0; i < ARRAY_WIDTH; i++)
            {
                for (int j = 0; j < ARRAY_HEIGHT; j++)
                {
                    buttonGrid[i, j] = new Button();
                    buttonGrid[i, j].Location = new Point(i * ClientSize.Width / ARRAY_WIDTH, j * ClientSize.Height / ARRAY_HEIGHT);
                    buttonGrid[i, j].Size = new Size(ClientSize.Width / ARRAY_WIDTH, ClientSize.Height / ARRAY_HEIGHT);
                    buttonGrid[i, j].Tag = new Point(i,j);
                    buttonGrid[i, j].MouseDown += Form1_MouseDown1;
                    buttonGrid[i, j].KeyDown += keyPress;
                   
                    Controls.Add(buttonGrid[i, j]);
                }
            }
        }
        private void keyPress(object sender, KeyEventArgs e)
        {
            Button temp = sender as Button;
           
            switch (e.KeyCode)
            {
                case Keys.Space:
                    Pathing();
                break;
                
            }

            
        }
        private void Form1_MouseDown1(object sender, MouseEventArgs e)
        {
            Button temp = sender as Button;
            switch (e.Button)
            {
                case MouseButtons.Left:
                    wallRegistration(temp);
                    break;
                case MouseButtons.Right:
                    endPointRegistration(temp);
                    break;               

            }
        }

        void wallRegistration(Button clickedButton)
        {
            Point buttonLocation = (Point)clickedButton.Tag;
            if (walls.Contains(buttonLocation))
            {
                walls.Remove(buttonLocation);
                clickedButton.BackColor = Color.Transparent;
            }
            else
            {
                walls.Add(buttonLocation);
                clickedButton.BackColor = Color.Black;
            }
        }
    
        public void endPointRegistration(Button clickedButton)
        { 
            if (assignFirst)
            {
                buttonGrid[start.X, start.Y].BackColor = Color.Transparent;
                start = (Point)clickedButton.Tag;
            }
            else
            {
                buttonGrid[end.X, end.Y].BackColor = Color.Transparent;
                end = (Point)clickedButton.Tag; 
            }
            walls.Remove((Point)clickedButton.Tag);
            clickedButton.BackColor = Color.Red;
            assignFirst = !assignFirst;
        }

       
        void Pathing()
        {
            if (start == end)
                throw new Exception("Only one end point");

            buttonGrid[start.X, start.Y].BackColor = Color.Red;
            buttonGrid[end.X, end.Y].BackColor = Color.Red;

            open.Add(new Node(start));
            while(true)
            {
                Node current = open.First();
                foreach (var item in open)
                {
                    if (item.F(start,end) < current.F(start,end))
                        current = item;
                }
                open.Remove(current);
                closed.Add(current);

                if (current.Loc == end)
                {
                    do
                    {
                        buttonGrid[current.Loc.X, current.Loc.Y].BackColor = Color.Blue;
                        current = current.parent;
                    } while (current.Loc !=  start);
                    break;
                }
                   
                List<Node> neighbors = current.dummyNeighbors();
                foreach (Node neighbor in neighbors)
                {
                    buttonGrid[neighbor.Loc.X, neighbor.Loc.Y].BackColor = Color.Yellow;
                    if (walls.Contains(neighbor.Loc) || closed.Contains(neighbor) || neighbor.Loc.X < 0 || neighbor.Loc.Y < 0 || neighbor.Loc.X > ARRAY_WIDTH || neighbor.Loc.Y > ARRAY_HEIGHT)
                        continue;

                    if (!open.Contains(neighbor) ||
                        open.Exists(openElement => openElement.Loc == neighbor.Loc && neighbor.F(start, end) < openElement.fCost))
                    {
                        neighbor.fCost = neighbor.F(start, end);
                        neighbor.parent = current;
                        
                        if(!open.Contains(neighbor))
                        {
                            open.Add(neighbor);
                        }
                        else
                        {
                            open[open.IndexOf(open.Find(node => node.Loc == neighbor.Loc))] = neighbor;
                        }
                    }
                    
                    
                };
            }
            
        }

     
    }
}
