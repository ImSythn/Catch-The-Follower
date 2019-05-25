using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace CatchTheFollower
{
    public class Tile : Panel  // Makes it so you can use Panel Controls in Tile Class 
    {
        private int tileSize;
        public int GridSize;

        public Dictionary<Tile, MoveEnum> Neighbours;

        private IObject gameObject;
        public IObject GameObject // Set Tiles GameObject so it can be used as an object
        {
            get { return gameObject; }
            set
            {
                this.gameObject = value;  // Sets the GameObject IObject so this Tile knows what's on it 
                if (value != null)  // Sets the background image of this Tile so it shows the sprite that IObject is liked to 
                {
                    BackgroundImage = this.gameObject.Sprite;
                }
                else
                {
                    BackgroundImage = null;
                }
            }
        }

        public Tile(int GridSize, Point tilePos)  
        {
            Neighbours = new Dictionary<Tile, MoveEnum>();  // Gets all the neighbour tiles of this Tile
            tileSize = 50;
            Size = new Size(tileSize, tileSize); 
            Location = new Point(tileSize * tilePos.X, tileSize * tilePos.Y);
            this.GridSize = GridSize;
            base.BackColor = Color.FromArgb(61, 147, 14);
        }

        public void RegisterNeighbours(Tile[,] grid, Point tilePos)  // Adds all Neighbouring Tile and its associated MoveEnum to a list
        {
            Neighbours.Clear();
            if (tilePos.X > 0)  // Check if tile has neighbour in certain direction
                Neighbours.Add(grid[tilePos.X - 1, tilePos.Y], MoveEnum.LEFT);  // Add both a Tile and associated MoveEnum to a Dictonary 
            if (tilePos.X < GridSize - 1)
                Neighbours.Add(grid[tilePos.X + 1, tilePos.Y], MoveEnum.RIGHT);
            if (tilePos.Y > 0)
                Neighbours.Add(grid[tilePos.X, tilePos.Y - 1], MoveEnum.UP);
            if (tilePos.Y < GridSize - 1)
                Neighbours.Add(grid[tilePos.X, tilePos.Y + 1], MoveEnum.DOWN);
        }
    }
}
