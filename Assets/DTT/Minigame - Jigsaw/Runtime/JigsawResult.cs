using System.Text;
using UnityEngine;

namespace DTT.MiniGame.Jigsaw
{
    /// <summary>
    /// Class holding the result info of a jigsaw game.
    /// </summary>
    public struct JigsawResult
    {
        /// <summary>
        /// Time it took to finish the game.
        /// </summary>
        public readonly float timeTaken;

        /// <summary>
        /// The final score in the game.
        /// </summary>
        public readonly float score;

        /// <summary>
        /// Amount of pieces that were misplaced on the grid.
        /// </summary>
        public readonly int amountOfMisplacedPieces;

        /// <summary>
        /// Sets the result info.
        /// </summary>
        /// <param name="timeTaken">Time the game took</param>
        /// <param name="amountOfMisplacedPieces">Amount of pieces that were misplaced</param>
        public JigsawResult(float timeTaken, int amountOfMisplacedPieces)
        {
            this.timeTaken = timeTaken;
            this.amountOfMisplacedPieces = amountOfMisplacedPieces;

            float score = 1f - (0.1f * amountOfMisplacedPieces);
            this.score = Mathf.Clamp(score, 0, 1);
        }

        /// <summary>
        /// Returns result info in string format for debugging.
        /// </summary>
        /// <returns>Result in string format</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Time taken (s): ");
            sb.Append(timeTaken);
            sb.Append('\t');
            sb.Append("Amount of misplaced pieces: ");
            sb.Append(amountOfMisplacedPieces);

            return sb.ToString();
        }
    }
}