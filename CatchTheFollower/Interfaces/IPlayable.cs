namespace CatchTheFollower
{
    public interface IPlayable
    {
        bool Dead { get; set; }
        void Move(MoveEnum moveDirection);
        void CheckIfDead();
    }
}
