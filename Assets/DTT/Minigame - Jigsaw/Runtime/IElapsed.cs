
namespace DTT.MiniGame.Jigsaw
{
    /// <summary>
    /// Interface for getting the amount of time that has elapsed since starting the minigame.
    /// </summary>
    public interface IElapsed
    {
        /// <summary>
        /// Returns the amount of time that has passed since starting the minigame.
        /// </summary>
        float TimeElapsed { get; }
    }
}