using System.Drawing;
using System.Linq;
using System.Collections.Generic;

namespace CatchTheFollower
{
    public class Box : IObject, IMovable
    {
        public Image Sprite { get; set; }

        private Tile tile;

        public Box(Tile tile)
        {
            Sprite = Image.FromFile(@"..\..\Images\box.png");
            this.tile = tile;
        }

        private Tile CheckNeighbour(Tile neighbour, MoveEnum moveDirection)
        {
            KeyValuePair<Tile, MoveEnum> neighbourObject = neighbour.Neighbours.Where(tile => tile.Value == moveDirection).FirstOrDefault();  // Returns first TileNeighbourDto that match condition (or null if condition isn't matched)
            if (neighbourObject.Key != null)
            {
                Tile boxNeighbour = neighbourObject.Key;

                if (!(boxNeighbour.GameObject is IUnmoveable || boxNeighbour.GameObject is IArtificial))
                {
                    if (boxNeighbour.GameObject is IMovable)
                    {
                        boxNeighbour = CheckNeighbour(boxNeighbour, moveDirection) ?? boxNeighbour; // Overwrites boxNeighbour when CheckNeighbour is not null
                    }

                    IObject playerGameObject = neighbour.GameObject;
                    IObject neighbourGameObject = boxNeighbour.GameObject;

                    if (!(boxNeighbour.GameObject is IMovable))
                    {
                        neighbour.GameObject = neighbourGameObject;
                        boxNeighbour.GameObject = playerGameObject;
                        Tile returnTile = neighbour;
                        neighbour = boxNeighbour;
                        return returnTile;
                    }
                }
                else if (boxNeighbour.GameObject is Portal)
                {
                    Portal neighbourGameObject = boxNeighbour.GameObject as Portal;
                    KeyValuePair<Tile, MoveEnum> targetNeighbour = neighbourGameObject.Target.Neighbours.Where(tile => tile.Value == moveDirection).FirstOrDefault();
                    if (targetNeighbour.Key != null && !(targetNeighbour.Key.GameObject is IUnmoveable))
                    {
                        if (targetNeighbour.Key.GameObject is IMovable box)
                        {
                            neighbourGameObject.Target.Neighbours.Remove(targetNeighbour.Key);
                            neighbourGameObject.Target.Neighbours.Add(box.Move(targetNeighbour.Key, moveDirection) ?? neighbour, moveDirection);  
                        }
                        IObject boxGameObject = neighbour.GameObject;

                        if (!(targetNeighbour.Key.GameObject is IMovable))
                        {
                            neighbour.GameObject = targetNeighbour.Key.GameObject;
                            targetNeighbour.Key.GameObject = boxGameObject;
                            Tile returnTile = neighbour;
                            return returnTile;
                        }
                    }
                }
            }
            return null;
        }

        public Tile Move(Tile neighbour, MoveEnum moveDirection)
        {
            Tile returnTile = CheckNeighbour(neighbour, moveDirection);
            this.tile = returnTile;
            return returnTile;
        }
    }
}
