namespace CatchTheFollower
{
    public interface IArtificial
    {
        bool Dead { get; set; }
        void Move();
    }
}
