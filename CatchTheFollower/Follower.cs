using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace CatchTheFollower
{
    public class Follower : IObject, IArtificial
    {
        public Image Sprite { get; set; }
        private Tile tile;
        public bool Dead { get; set; }
        private Random RandomEnum = new Random();

        public Follower(Tile tile)
        {
            Sprite = Image.FromFile(@"..\..\Images\follower.png");
            this.tile = tile;
            Dead = false;
        }


        private void CheckNeighbour(MoveEnum moveDirection)
        {
            KeyValuePair<Tile, MoveEnum> neighbourObject = tile.Neighbours.Where(tile => tile.Value == moveDirection).FirstOrDefault(); // Returns first TileNeighbourDto that match condition (or null if condition isn't matched)
            if (neighbourObject.Key != null)
            {
                Tile neighbour = neighbourObject.Key;

                if (!(neighbour.GameObject is IUnmoveable) || !(neighbour.GameObject is IMovable))
                {
                    IObject playerGameObject = tile.GameObject;
                    IObject neighbourGameObject = neighbour.GameObject;

                    tile.GameObject = neighbourGameObject;
                    neighbour.GameObject = playerGameObject;
                    this.tile = neighbour;
                }
            }
        }

        private MoveEnum CalculateMoveDirection()
        {
            Array values = Enum.GetValues(typeof(MoveEnum));
            MoveEnum randomMoveDirection = (MoveEnum)values.GetValue(RandomEnum.Next(values.Length));


            while (!CanMoveThisDirection(randomMoveDirection))  // If randomMoveDirection is false loop till plausible
            {
                randomMoveDirection = (MoveEnum)values.GetValue(RandomEnum.Next(values.Length));
                if (tile.Neighbours.All(neighbour => CanMoveThisDirection(neighbour.Value) == false))  // Checks if all neighbours are inmovable
                {
                    Dead = true;
                    break;
                }
            }
            
            return randomMoveDirection;
        }

        private bool CanMoveThisDirection(MoveEnum moveDirection)
        {
            KeyValuePair<Tile, MoveEnum> neighbourObject = tile.Neighbours.Where(tile => tile.Value == moveDirection).FirstOrDefault();
            if (neighbourObject.Key != null)
            {
                Tile neighbour = neighbourObject.Key;

                if (!(neighbour.GameObject is IUnmoveable) && !(neighbour.GameObject is IMovable))
                {
                    return true;
                }
            }
            return false;
        }

        public void Move()
        {
            MoveEnum movement = CalculateMoveDirection();
            if (!Dead)
            {
                CheckNeighbour(movement);
            }
            else
            {
                this.tile.BackgroundImage = Image.FromFile(@"..\..\Images\follower_dead.png");
            }
        }
    }
}
