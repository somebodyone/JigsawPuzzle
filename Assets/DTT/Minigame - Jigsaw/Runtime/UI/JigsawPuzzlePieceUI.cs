using System;
using DTT.MinigameBase.Handles;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DTT.MiniGame.Jigsaw.UI
{
    /// <summary>
    /// Handles the moving of the puzzle piece and UI events.
    /// </summary>
    [RequireComponent(typeof(MoveHandle))]
    public class JigsawPuzzlePieceUI : Image
    {
        /// <summary>
        /// Invoked when the piece is picked up.
        /// </summary>
        public event Action<JigsawPuzzlePieceUI> PickUp;

        /// <summary>
        /// Invoked when the piece is dropped.
        /// </summary>
        public event Action<JigsawPuzzlePieceUI> Drop;

        /// <summary>
        /// Reference to the <see cref="UI.MoveHandle"/> of this object.
        /// </summary>
        public MoveHandle MoveHandle { get; private set; }

        /// <summary>
        /// Reference to the <see cref="JigsawPuzzlePiece"/> of this object.
        /// </summary>
        public JigsawPuzzlePiece Piece { get; private set; }

        /// <summary>
        /// Reference to the rect transform of the object.
        /// </summary>
        public RectTransform RectTransform => (RectTransform)transform;

        /// <summary>
        /// Last world position the piece was picked up from.
        /// </summary>
        private Vector2 _lastPos;

        public bool isInBoard;

        /// <summary>
        /// Gets necessary components and sets initial values.
        /// </summary>
        protected override void Awake()
        {
            base.Awake();
            _lastPos = rectTransform.position;
            MoveHandle = GetComponent<MoveHandle>();
        }

        /// <summary>
        /// Subscribes to necessary events.
        /// </summary>
        protected override void OnEnable()
        {
            base.OnEnable();
            MoveHandle.PointerUp += OnDrop;
            MoveHandle.PointerDown += OnPickUp;
        }

        /// <summary>
        /// Unsubscribes from events.
        /// </summary>
        protected override void OnDisable()
        {
            MoveHandle.PointerUp -= OnDrop;
            MoveHandle.PointerDown -= OnPickUp;
            base.OnDisable();
        }

        /// <summary>
        /// Called when the puzzle piece is picked up.
        /// </summary>
        private void OnPickUp(PointerEventData eventData)
        {
            _lastPos = rectTransform.position;
            PickUp?.Invoke(this);
        }

        /// <summary>
        /// Called when the puzzle piece has been dropped.
        /// </summary>
        private void OnDrop(PointerEventData eventData) => Drop?.Invoke(this);

        /// <summary>
        /// Sets the necessary data to the puzzle piece shader and
        /// initializes the UI piece.
        /// </summary>
        /// <param name="piece">The puzzle piece of this UI element</param>
        /// <param name="sprite">Sprite of the puzzle</param>
        /// <param name="correctPosition">Correct grid position this puzzle piece should be placed on</param>
        /// <param name="width">Width of the board</param>
        /// <param name="height">Height of the board</param>
        public void ApplyData(JigsawPuzzlePiece piece, Sprite sprite, Vector2Int correctPosition, float width, float height)
        {
            Piece = piece;

            Vector4 v4 = new Vector4(
                Mathf.InverseLerp(0, 2, (int)piece.GetConnector(Direction.WEST)),
                Mathf.InverseLerp(0, 2, (int)piece.GetConnector(Direction.EAST)),
                Mathf.InverseLerp(0, 2, (int)piece.GetConnector(Direction.NORTH)),
                Mathf.InverseLerp(0, 2, (int)piece.GetConnector(Direction.SOUTH))
            );

            material = new Material(material);
            material.SetVector("_Connectors", v4);

            this.sprite = sprite;

            material.SetVector("_MainUv",
            new Vector4(
                (correctPosition.x - 1) * width, 
                (correctPosition.y - 1) * height,
                (correctPosition.x + 2) * width,
                (correctPosition.y + 2) * height
                )
            );
        }

        /// <summary>
        /// Checks if the user is clicking on the rect area of this object.
        /// </summary>
        public override bool IsRaycastLocationValid(Vector2 screenPoint, Camera eventCamera)
        {
            Rect hitArea = rectTransform.rect;

            Vector2 aThird = hitArea.size / 3f;
            hitArea.size = aThird;
            hitArea.position = hitArea.position + rectTransform.rect.size / 2 - aThird / 2;

            return hitArea.Contains(rectTransform.InverseTransformPoint(screenPoint));
        }

        /// <summary>
        /// Moves the puzzle piece to a new grid position.
        /// </summary>
        /// <param name="position">Grid position</param>
        public void Move(Vector2Int position)
        {
            Piece.MovePiece(position);
        }

        /// <summary>
        /// Sets the puzzle piece back to the last world position it was picked up from.
        /// </summary>
        public void SnapBack() => rectTransform.position = _lastPos;

        /// <summary>
        /// Sets the grid position to null to indicate it's off the board.
        /// </summary>
        public void MoveOffBoard() => Piece.MovePieceOffBoard();

        /// <summary>
        /// Gets grid position from the given world position.
        /// </summary>
        /// <param name="position">Word position</param>
        /// <returns>Grid position</returns>
        public Vector2Int WorldToGrid(Vector2 position)
        {
            Vector3 halfSize = rectTransform.rect.size / 2;
            return new Vector2Int(Mathf.RoundToInt((position.x - halfSize.x) / rectTransform.rect.width),
                Mathf.RoundToInt((position.y - halfSize.y) / rectTransform.rect.height));
        }

        /// <summary>
        /// Gets local position from the given grid position.
        /// </summary>
        /// <param name="position">Grid position</param>
        /// <returns>Local position</returns>
        public Vector2 GridToLocal(Vector2Int position)
        {
            Vector2 halfSize = rectTransform.rect.size / 2;
            return new Vector2(position.x * rectTransform.rect.width, position.y * rectTransform.rect.height) + halfSize;
        }

        /// <summary>
        /// Updates the position of the piece so it matches on the board. This has to happen if resolution changes.
        /// </summary>
        private void Update()
        {
            if (Piece.Position != null)
            {
                transform.localPosition = GridToLocal(Piece.Position.Value);
            }
        }
    }
}