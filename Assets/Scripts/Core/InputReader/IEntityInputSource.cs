namespace Core.InputReader
{
    public interface IEntityInputSource
    {
        float HorizontalDirection { get; }
        bool Jump { get; }

        void ResetOneTimeActions();
    }
}