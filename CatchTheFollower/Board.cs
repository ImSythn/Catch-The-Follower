using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace CatchTheFollower
{
    public partial class Board : Form
    {
        private IPlayable player;
        private IArtificial follower;
        private int formSize;
        private Tile[,] grid;
        private List<Point> blockPositions;
        private List<Point> boxPositions;
        private List<DoublePoint> portalPositionList; 
        private Point followerPosition;
        private Point playerPosition;
        private Timer followTimer;
        private int gridSize;
        private int blockCount;
        private int boxCount;
        private int portalCount;
        public int Difficulty { get; set; }
        public bool Paused { get; set; }

        Random RandomNumber = new Random();

        public Board()
        {
            Text = "Catch The Follower";
            Difficulty = 1;
            gridSize = 15;
            formSize = 50;
            portalCount = 2;
            followTimer = new Timer();
            followTimer.Interval = 1000;
            followTimer.Tick += FollowTimer_Tick;
            Paused = false;
            InitializeComponent();
        }
        
        private void Board_Load(object sender, EventArgs e)
        {
            grid = new Tile[gridSize, gridSize];  // Creates Grid out of Class Tile
            this.Size = new Size(formSize * gridSize + 300, formSize * gridSize + 39); // 300 and 39 to compensate the window frame
            this.BackColor = Color.FromArgb(32, 33, 31);  // nice colour

            for (int x = 0; x < gridSize; x++)
            {
                for (int y = 0; y < gridSize; y++)
                {
                    grid[x, y] = new Tile(gridSize, new Point(x, y));  // Create a new Tile on x, y Positoin 
                }
            }
            LoadItems();
            UI UserInterface = new UI(this);  // sends itself to UI
            UserInterface.MakeButtons();
        }

        public void LoadItems()  // Loads in all the objects in their correct positions
        {
            BlockBoxCount();
            ObjectLocations();

            for (int x = 0; x < gridSize; x++)
            {
                for (int y = 0; y < gridSize; y++)
                {
                    grid[x, y].GameObject = null;  // Makes sure that when the board resets there is not more objects from previous board
                    grid[x, y].RegisterNeighbours(grid, new Point(x,y));  // Registers Neighbours with associated MoveEnums of all Tile
                    Controls.Add(grid[x, y]); 
                    CreateObject(x, y);  
                }
            }
            followTimer.Start();
        }

        private void BlockBoxCount()
        {
            if (Difficulty == 0)
            {
                blockCount = gridSize * gridSize / 100 * 15;
                boxCount = gridSize * gridSize / 100 * 15; ;
            }
            if (Difficulty == 1)
            {
                blockCount = gridSize * gridSize / 100 * 10;
                boxCount = gridSize * gridSize / 100 * 10;
            }
            if (Difficulty == 2)
            {
                blockCount = gridSize * gridSize / 100 * 5;
                boxCount = gridSize * gridSize / 100 * 5;
            }
            if (Difficulty == 3)
            {
                blockCount = 1;
                boxCount = 4;
            }
        }

        private void ObjectLocations() // Pre-generate object positions
        {
            List<Point> allPositions = new List<Point>();
            blockPositions = new List<Point>();
            boxPositions = new List<Point>();
            portalPositionList = new List<DoublePoint>();
            List<Point> temporaryPortalPosition = new List<Point>();
            playerPosition = new Point(gridSize - gridSize, gridSize - gridSize);
            followerPosition = new Point(gridSize - 1, gridSize - 1);

            int[] Count = { blockCount, boxCount, portalCount, 0 }; // Added 0 for breaking out of while loop
            int countNumber = 0;

            while (Count[countNumber] > 0)
            {
                Point RandomPoint = NewRandomPosition();
                if (!allPositions.Any(item => item == RandomPoint) && RandomPoint != playerPosition && RandomPoint != followerPosition) // Stops object positions from overlapping
                {
                    if (countNumber == 0)
                        blockPositions.Add(RandomPoint);
                    if (countNumber == 1)
                        boxPositions.Add(RandomPoint);
                    if (countNumber == 2)
                        temporaryPortalPosition.Add(RandomPoint);
                    Count[countNumber]--;
                }
                allPositions.Add(RandomPoint);
                if (Count[countNumber] < 1)  // When BlockCount runs out switch to BoxCount
                {
                    countNumber++;
                }
            }
            int index = 0;
            for (int i = 0; i < temporaryPortalPosition.Count/2; i++) //Links portal coordinates as source(for own position) and target(for linked portal position) for loop for expandability (incase you want more portals)
            {
                portalPositionList.Add(new DoublePoint(temporaryPortalPosition[index], temporaryPortalPosition[index + 1]));
                portalPositionList.Add(new DoublePoint(temporaryPortalPosition[index + 1], temporaryPortalPosition[index]));
                index += 2;
            }
        }

        private Point NewRandomPosition()  // Create a random point
        {
            int x = RandomNumber.Next(gridSize);
            int y = RandomNumber.Next(gridSize);
            Point RandomPoint = new Point(x, y);
            return RandomPoint;
        }

        private void CreateObject(int x, int y) // If an ObjectLocation (in a List) is the same as new Point Create a new instance of this GameObject on this point
        {
            if (blockPositions.Any(BlockLocation => BlockLocation == new Point(x, y)))  // Cycles through list untill it finds a BlockLocation that equals new Point
            {
                grid[x, y].GameObject = new Block(grid[x, y]);  // Creates GameObject new Block on this position and sends this Positoin to this Block 
            }
            if (boxPositions.Any(BoxLocation => BoxLocation == new Point(x, y)))  
            {
                grid[x, y].GameObject = new Box(grid[x, y]);  
            }
            if (portalPositionList.Any(portalPosition => portalPosition.Source == new Point(x,y)))
            {
                DoublePoint doublePoint = portalPositionList.FirstOrDefault(portalPosition => portalPosition.Source == new Point(x, y));
                grid[x, y].GameObject = new Portal(grid[x,y]/*source portal*/, grid[doublePoint.Target.X, doublePoint.Target.Y]/*target portal*/);
            }
            if (playerPosition == new Point(x, y))
            {
                grid[x, y].GameObject = new Player(grid[x, y]);  
                player = (IPlayable)grid[x, y].GameObject;  // Cast GameObject as IPlayable
            }
            if (followerPosition == new Point(x, y))
            {
                grid[x, y].GameObject = new Follower(grid[x, y]);  
                follower = (IArtificial)grid[x, y].GameObject;
            }
        }

        private void GameInput_KeyDown(object sender, KeyEventArgs e)  
        {
            if (!Paused && !follower.Dead)
            {
                switch (e.KeyCode) 
                {
                    case Keys.Up:
                        player.Move(MoveEnum.UP);
                        break;
                    case Keys.Right:
                        player.Move(MoveEnum.RIGHT);
                        break;
                    case Keys.Down:
                        player.Move(MoveEnum.DOWN);
                        break;
                    case Keys.Left:
                        player.Move(MoveEnum.LEFT);
                        break;
                }
            }
        }

        private void FollowTimer_Tick(object sender, EventArgs e)  // Checks if Player or Follower is dead if true stop Follower move timer
        {
            if (!follower.Dead && !player.Dead)
            {
                if (!Paused)
                    follower.Move();
                player.CheckIfDead();
            }
            else
                ((Timer)sender).Stop();
        }
    }
}
