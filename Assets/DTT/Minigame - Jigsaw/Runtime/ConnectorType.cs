using UnityEngine;

namespace DTT.MiniGame.Jigsaw
{
    /// <summary>
    /// The different types of a side of jigsaw puzzle piece.
    /// </summary>
    public enum ConnectorType
    {
        /// <summary>
        /// Can be seen as a female piece.
        /// </summary>
        [InspectorName("Blank")]
        BLANK = 0,
        
        /// <summary>
        /// A side piece of the puzzle. Is seen as straight.
        /// </summary>
        [InspectorName("Edge")]
        EDGE = 1,
        
        /// <summary>
        /// Can be seen as a male piece.
        /// </summary>
        [InspectorName("Tab")]
        TAB = 2
    }
}