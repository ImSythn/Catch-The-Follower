using System.Drawing;
using System.Linq;
using System.Collections.Generic;

namespace CatchTheFollower
{
    public class Player : IObject, IPlayable
    {
        public Image Sprite { get; set; }
        public bool Dead { get; set; }

        private Tile tile;

        public Player(Tile tile)
        {
            Sprite = Image.FromFile(@"..\..\Images\player.png");
            this.tile = tile;
            Dead = false;
        }

        private void CheckNeighbour(MoveEnum moveDirection)
        {
            KeyValuePair<Tile, MoveEnum> neighbourObject = tile.Neighbours.Where(tile => tile.Value == moveDirection).FirstOrDefault();  // Returns first TileNeighbourDto that match condition (or null if condition isn't matched)
            if (neighbourObject.Key != null)
            {
                Tile neighbour = neighbourObject.Key;

                if (!(neighbour.GameObject is IUnmoveable))
                {
                    if (neighbour.GameObject is IMovable box) // Checks if GameObject is of type IMovable. If so, then GameObject is assigned to variable 'box'.
                    {
                        neighbour = box.Move(neighbour, moveDirection) ?? neighbour;  // Overwrite neighbour if returned Tile from box.Move() is not null
                    }

                    IObject playerGameObject = tile.GameObject;
                    IObject neighbourGameObject = neighbour.GameObject;

                    if (!(neighbour.GameObject is IMovable))
                    {
                        tile.GameObject = neighbourGameObject;
                        neighbour.GameObject = playerGameObject;
                        this.tile = neighbour;
                    }
                }
                else if (neighbour.GameObject is Portal) //checks if neighbour is a portal
                {
                    Portal neighbourGameObject = neighbour.GameObject as Portal; //makes neighbour.GameObject a portal, which doesn't conflict since it is a portal
                    KeyValuePair<Tile, MoveEnum> targetNeighbour = neighbourGameObject.Target.Neighbours.Where(tile => tile.Value == moveDirection).FirstOrDefault(); //checks target portal neighbours
                    if (targetNeighbour.Key != null && !(targetNeighbour.Key.GameObject is IUnmoveable)) //checks to see if the target neighbour isn't null or isn't unmoveable
                    {
                        if (targetNeighbour.Key.GameObject is IMovable box)
                        {
                            neighbourGameObject.Target.Neighbours.Remove(targetNeighbour.Key); //temporarily deletes the neighbour
                            neighbourGameObject.Target.Neighbours.Add(box.Move(targetNeighbour.Key, moveDirection) ?? neighbour, moveDirection);  // Overwrite neighbour if returned Tile from box.Move() is not null
                        }
                        IObject playerGameObject = tile.GameObject;

                        if (!(targetNeighbour.Key.GameObject is IMovable)) //checks if targetneighbour isn't movable
                        {
                            tile.GameObject = targetNeighbour.Key.GameObject;
                            targetNeighbour.Key.GameObject = playerGameObject;
                            this.tile = targetNeighbour.Key;
                        }
                    }
                }
            }
        }

        public void Move(MoveEnum moveDirection)
        {
            if (!Dead)
            {
                CheckNeighbour(moveDirection);
            }
            CheckIfDead();
        }

        public void CheckIfDead()
        {
            if (tile.Neighbours.Any(neighbour => neighbour.Key.GameObject is IArtificial))  // Checks if IArtoficial is one of the Player Tile Neigbours 
            {
                Dead = true;
                this.tile.BackgroundImage = Image.FromFile(@"..\..\Images\player_dead.png");
            }
        }
        
    }
}
