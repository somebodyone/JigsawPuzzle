using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using DTT.MiniGame.Jigsaw.Extensions;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DTT.MiniGame.Jigsaw
{
    /// <summary>
    /// Handles the logic of the jigsaw board.
    /// Initializes the jigsaw and checks if it has been completed.
    /// </summary>
    internal class JigsawBoard
    {
        /// <summary>
        /// The config that's currently being used for this board.
        /// </summary>
        internal JigsawConfig Config => _config;

        /// <summary>
        /// A dictionary mapping a grid position to its correct piece.
        /// </summary>
        internal ReadOnlyDictionary<Vector2Int, JigsawPuzzlePiece> CorrectLayout =>
            new ReadOnlyDictionary<Vector2Int, JigsawPuzzlePiece>(_correctLayout);

        /// <summary>
        /// The config that's currently being used for this board.
        /// </summary>
        private JigsawConfig _config;

        /// <summary>
        /// A dictionary mapping a grid position to its correct piece.
        /// </summary>
        private Dictionary<Vector2Int, JigsawPuzzlePiece> _correctLayout;

        /// <summary>
        /// Initializes the board. This creates all the pieces and maps them to their correct positions.
        /// </summary>
        /// <param name="config">Configuration of the puzzle.</param>
        internal void Initialize(JigsawConfig config)
        {
            _config = config;
            // Reserve the dictionary with the total amount of pieces.
            _correctLayout = new Dictionary<Vector2Int, JigsawPuzzlePiece>(config.Size.x * config.Size.y);

            for (int i = 0; i < config.Size.x; i++)
            {
                for (int j = 0; j < config.Size.y; j++)
                {
                    Vector2Int currentPosition = new Vector2Int(i, j);
                    JigsawPuzzlePiece currentPiece = _correctLayout[currentPosition] = new JigsawPuzzlePiece();

                    // Set the connector types of all the edges to EDGE.
                    if (i == 0)
                        currentPiece.SetConnector(Direction.WEST, ConnectorType.EDGE);
                    if (j == 0)
                        currentPiece.SetConnector(Direction.SOUTH, ConnectorType.EDGE);
                    if (i == config.Size.x - 1)
                        currentPiece.SetConnector(Direction.EAST, ConnectorType.EDGE);
                    if (j == config.Size.y - 1)
                        currentPiece.SetConnector(Direction.NORTH, ConnectorType.EDGE);

                    if (i != config.Size.x - 1)
                        currentPiece.SetConnector(Direction.EAST, GetRandomConnector());

                    if (i != 0)
                    {
                        ConnectorType connector = _correctLayout[new Vector2Int(i - 1, j)].GetConnector(Direction.EAST).GetOppositeConnector();
                        currentPiece.SetConnector(Direction.WEST, connector);
                    }

                    if (j != config.Size.y - 1)
                        currentPiece.SetConnector(Direction.NORTH, GetRandomConnector());

                    if (j != 0)
                    {
                        ConnectorType connector = _correctLayout[new Vector2Int(i, j - 1)].GetConnector(Direction.NORTH).GetOppositeConnector();
                        currentPiece.SetConnector(Direction.SOUTH, connector);
                    }
                }
            }
        }

        /// <summary>
        /// Checks all the positions of the pieces and checks whether they match the correct layout.
        /// Will return false if not all pieces are on the board.
        /// </summary>
        /// <returns>Whether the board has been set up correctly.</returns>
        internal bool VerifyBoard()
        {
            Dictionary<Vector2Int, JigsawPuzzlePiece> current = CurrentLayout();

            // Check whether the amount of pieces in the layout are equal.
            // If not it can't be correct.
            if (current.Count != _correctLayout.Count)
                return false;

            // Check the position of every piece and make sure it matches the original.
            foreach (var positionPiecePair in current)
                if (_correctLayout[positionPiecePair.Key] != positionPiecePair.Value)
                    return false;

            return true;
        }

        /// <summary>
        /// Checks whether the given position is out of bounds of the grid.
        /// </summary>
        /// <param name="position">The position to check.</param>
        /// <returns></returns>
        internal bool GridPositionIsOutOfBounds(Vector2Int position) =>
            position.x >= _config.Size.x && position.x < 0 && position.y >= _config.Size.y && position.y < 0;

        /// <summary>
        /// Calculates the current layout. Pieces that are off the board aren't included.
        /// </summary>
        /// <returns>A dictionary with positions mapped to the placed piece.</returns>
        internal Dictionary<Vector2Int, JigsawPuzzlePiece> CurrentLayout()
        {
            Dictionary<Vector2Int, JigsawPuzzlePiece> layout = new Dictionary<Vector2Int, JigsawPuzzlePiece>(_config.Size.x * _config.Size.y);
            foreach (var positionPiecePair in _correctLayout.Where(positionPiecePair => positionPiecePair.Value.Position.HasValue))
                if (positionPiecePair.Value.Position != null)
                    layout[positionPiecePair.Value.Position.Value] = positionPiecePair.Value;

            return layout;
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <returns><inheritdoc/></returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var kvp in _correctLayout)
            {
                sb.Append(kvp.Value);
                sb.AppendLine();
            }

            return sb.ToString();
        }

        /// <summary>
        /// Retrieves a random connector tab or blank.
        /// </summary>
        /// <returns>A random connector tab or blank.</returns>
        private static ConnectorType GetRandomConnector() => (ConnectorType)Mathf.RoundToInt(Random.Range(0, 2) * 2);
    }
}