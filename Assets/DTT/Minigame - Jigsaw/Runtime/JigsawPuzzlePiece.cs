using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace DTT.MiniGame.Jigsaw
{
    /// <summary>
    /// A puzzle piece object in the Jigsaw game. Handles it's connector and position data.
    /// </summary>
    public class JigsawPuzzlePiece
    {
        /// <summary>
        /// The connectors of the object. Can be keyed by the direction for the type.
        /// </summary>
        private Dictionary<Direction, ConnectorType> _connectors = new Dictionary<Direction, ConnectorType>();

        /// <summary>
        /// The position of the piece. If null it's not on the board.
        /// </summary>
        public Vector2Int? Position { get; set; }

        /// <summary>
        /// Creates a new puzzle piece with all the connectors set to the edge.
        /// </summary>
        public JigsawPuzzlePiece()
        {
            Array values = Enum.GetValues(typeof(Direction));
            for (int i = 0; i < values.Length; i++) 
                _connectors[(Direction) values.GetValue(i)] = ConnectorType.EDGE;
        }

        /// <summary>
        /// Sets the connector of a certain direction.
        /// </summary>
        /// <param name="dir">The direction to apply the connector to.</param>
        /// <param name="type">The type of connector to set.</param>
        internal void SetConnector(Direction dir, ConnectorType type) => _connectors[dir] = type;

        /// <summary>
        /// Retrieves the connector of a certain direction.
        /// </summary>
        /// <param name="dir">The direction to get the connector from.</param>
        /// <returns>The type of connector from the given direction.</returns>
        internal ConnectorType GetConnector(Direction dir) => _connectors[dir];

        /// <summary>
        /// Moves the piece of the board.
        /// </summary>
        internal void MovePieceOffBoard() => Position = null;

        /// <summary>
        /// Moves the piece to a position.
        /// </summary>
        /// <param name="position">The position to move it to.</param>
        internal void MovePiece(Vector2Int position) => Position = position;

        /// <summary>
        /// Returns jigsaw piece info in string format for debugging.
        /// </summary>
        /// <returns>Jigsaw piece in string format</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Position: ");
            sb.Append(Position.HasValue ? Position.Value.ToString() : "Off board");
            sb.Append(", ");
            sb.Append("North: ");
            sb.Append(_connectors[Direction.NORTH].ToString());
            sb.Append(", ");
            sb.Append("East: ");
            sb.Append(_connectors[Direction.EAST].ToString());
            sb.Append(", ");
            sb.Append("South: ");
            sb.Append(_connectors[Direction.SOUTH].ToString());
            sb.Append(", ");
            sb.Append("West: ");
            sb.Append(_connectors[Direction.WEST].ToString());
            
            return sb.ToString();
        }
    }
}