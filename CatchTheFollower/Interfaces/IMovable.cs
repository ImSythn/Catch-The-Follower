namespace CatchTheFollower
{
    public interface IMovable
    {
        Tile Move(Tile neighbour, MoveEnum moveDirection);
    }
}
