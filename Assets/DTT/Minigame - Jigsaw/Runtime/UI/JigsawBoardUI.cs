using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;
using DTT.Utils.Extensions;
using UnityEngine.UI;

namespace DTT.MiniGame.Jigsaw.UI
{
    /// <summary>
    /// Manages the UI of the jigsaw board.
    /// </summary>
    public class JigsawBoardUI : MonoBehaviour
    {
        /// <summary>
        /// Gets called when a piece is placed on the board.
        /// </summary>
        public event Action<JigsawPuzzlePieceUI> PieceDroppedOnBoard;

        /// <summary>
        /// Gets called when a piece is placed on the board, but isn't in it's correct position.
        /// </summary>
        public event Action<JigsawPuzzlePieceUI> PieceMisplaced;

        /// <summary>
        /// Gets called when a piece is placed on the board and is in its correct position.
        /// </summary>
        public event Action<JigsawPuzzlePieceUI> PieceCorrectlyPlaced;

        /// <summary>
        /// The prefab for the jigsaw pieces.
        /// </summary>
        [SerializeField]
        [Tooltip("The prefab for the jigsaw pieces.")]
        private JigsawPuzzlePieceUI _prefabPiece;

        /// <summary>
        /// The container parent for all the pieces that are on the board.
        /// </summary>
        [SerializeField]
        [Tooltip("The container parent for all the pieces that are on the board.")]
        public RectTransform _piecesContainer;

        /// <summary>
        /// The container parent for the piece that currently is being dragged.
        /// </summary>
        [SerializeField]
        [Tooltip("The container parent for the piece that currently is being dragged.")]
        private RectTransform _draggingPieceContainer;

        /// <summary>
        /// The container parent for the pieces that are currently off the board.
        /// </summary>
        [SerializeField]
        public RectTransform _offBoardPiecesContainer;

        [SerializeField]
        private RectTransform list;

        /// <summary>
        /// All the pieces that have been instantiated.
        /// </summary>
        public JigsawPuzzlePieceUI[] _pieces = Array.Empty<JigsawPuzzlePieceUI>();

        /// <summary>
        /// Reference to the board of the game used for displaying the UI.
        /// </summary>
        private JigsawBoard _board;

        /// <summary>
        /// Since the image of the puzzle piece takes up one third of the UI object,
        /// the size of the transform needs to be multiplied by 3.
        /// </summary>
        private const int REQUIRED_IMAGE_SIZE = 3;

        /// <summary>
        /// The canvas this object is in.
        /// </summary>
        private Canvas _canvas;

        /// <summary>
        /// The size of the canvas on the previous frame.
        /// </summary>
        private Vector2 _previousCanvasSize;

        /// <summary>
        /// Creates the board based on the board.
        /// </summary>
        internal void CreateBoard(JigsawBoard board)
        {
            _board = board;
            var layout = board.CorrectLayout;
            _pieces = new JigsawPuzzlePieceUI[board.CorrectLayout.Count];

            _piecesContainer.GetComponent<Image>().sprite = board.Config.Image;
            // Define counter for accessing UI elements index.
            int counter = 0;
            foreach (var kvp in layout)
            {
                // Create new piece from prefab.
                JigsawPuzzlePieceUI piece = Instantiate(_prefabPiece, _offBoardPiecesContainer);//_piecesContainer);
                _pieces[counter] = piece;
                piece.RectTransform.sizeDelta = _piecesContainer.rect.size / board.Config.Size;
                piece.ApplyData(kvp.Value, board.Config.Image, kvp.Key, 1.0f / board.Config.Size.x, 1.0f / board.Config.Size.y);
                piece.MoveOffBoard();
                piece.raycastTarget = true;
                piece.rectTransform.localScale = Vector3.one * REQUIRED_IMAGE_SIZE;


                piece.transform.localPosition = piece.GridToLocal(Vector2Int.zero);
                //显示乱序
                int r = Random.Range(0, counter);
                piece.transform.SetSiblingIndex(r);
                piece.PickUp += HandlePickedUpPiece;
                piece.Drop += HandleDroppedPiece;
                ++counter;
            }
            //最下面的拼图 设置为正确位置
            //_pieces[0].transform.SetAsFirstSibling();
            //HandleDroppedPiece(_pieces[0]);
        }
        
        

        /// <summary>
        /// Cleans up all the event subscriptions and destroys the UI elements.
        /// </summary>
        public void CleanBoard()
        {
            for (int i = 0; i < _pieces.Length; i++)
            {
                if (_pieces[i] == null)
                    continue;

                _pieces[i].PickUp -= HandlePickedUpPiece;
                _pieces[i].Drop -= HandleDroppedPiece;
                Destroy(_pieces[i].gameObject);
            }
        }

        /// <summary>
        /// Sets the interactability of the puzzle pieces using the raycast target field on the image.
        /// </summary>
        /// <param name="interactable">Whether the pieces should be interactable or not</param>
        public void SetBoardInteractable(bool interactable)
        {
            for (int i = 0; i < _pieces.Length; i++)
                _pieces[i].raycastTarget = interactable;
        }

        /// <summary>
        /// Get initial values and references.
        /// </summary>
        private void Start()
        {
            _canvas = GetComponentInParent<Canvas>();
            _previousCanvasSize = _canvas.pixelRect.size;
        }

        /// <summary>
        /// Checks if the resolution changed and if so update the puzzle piece sizes.
        /// </summary>
        private void Update()
        {
            if (_previousCanvasSize != _canvas.pixelRect.size)
            {
                StartCoroutine(UpdateSizeCoroutine());
            }


            _previousCanvasSize = _canvas.pixelRect.size;
        }

        /// <summary>
        /// Updates the size of the pieces a frame later so the UI is time to process the new size.
        /// </summary>
        private IEnumerator UpdateSizeCoroutine()
        {
            yield return new WaitForEndOfFrame();
            for (int i = 0; i < _pieces.Length; i++)
            {
                _pieces[i].RectTransform.sizeDelta = _piecesContainer.rect.size / _board.Config.Size;
            }
        }

        /// <summary>
        /// Sets the piece to the pickup state.
        /// </summary>
        /// <param name="piece">The piece that's been picked up.</param>
        private void HandlePickedUpPiece(JigsawPuzzlePieceUI piece)
        {
            list.gameObject.SetActive(false);
            //判断拼图是否在正确的位置 无法移动正确位置的拼图
            if (JigsawManager.Instance.CheckOnePieceCurrent(piece)) return;

            piece.rectTransform.SetParent(_draggingPieceContainer);
            piece.MoveOffBoard();
        }

        /// <summary>
        /// Handles the state for when a piece is dropped.
        /// </summary>
        /// <param name="piece">The piece that is being dropped.</param>
        public void HandleDroppedPiece(JigsawPuzzlePieceUI piece)
        {
            list.gameObject.SetActive(true);
            // Check if the piece is placed on the board.
            if (piece.rectTransform.GetWorldRect().Overlaps(_piecesContainer.GetWorldRect()))
            {
                // 拼图数据位置
                Vector2Int pos = piece.WorldToGrid(_piecesContainer.InverseTransformPoint(piece.rectTransform.position));

                // Retrieve the current layout from the grid.
                Dictionary<Vector2Int, JigsawPuzzlePiece> layout = _board.CurrentLayout();

                if (!layout.ContainsKey(pos) && !_board.GridPositionIsOutOfBounds(pos) && _board.CorrectLayout.ContainsKey(pos))
                {
                    PlacePieceOnBoard(piece, pos);
                    return;
                }
                else
                {
                    // If it overlaps with another piece we place it off the board but keep the position.
                    //piece.rectTransform.SetParent(_offBoardPiecesContainer);
                }
            }

            PlacePieceOutsideOfBoard(piece);
        }

        /// <summary>
        /// Places the piece on the board with the given location.
        /// </summary>
        /// <param name="piece">The piece to place.</param>
        /// <param name="pos">The position to place it on.</param>
        private void PlacePieceOnBoard(JigsawPuzzlePieceUI piece, Vector2Int pos)
        {
            piece.rectTransform.SetParent(_piecesContainer);
            piece.Move(pos);
            //音频 和checkFinish
            PieceDroppedOnBoard?.Invoke(piece);

            // Check if the piece is placed on the correct slot in the layout.
            //是否归入正确位置
            if (_board.CorrectLayout[pos] != piece.Piece)
                //加了一次错误步骤
                PieceMisplaced?.Invoke(piece);
            else
            //没加方法
                PieceCorrectlyPlaced?.Invoke(piece);
        }

        /// <summary>
        /// The piece to place outside of the board.
        /// </summary>
        /// <param name="piece">The piece to place.</param>
        private void PlacePieceOutsideOfBoard(JigsawPuzzlePieceUI piece)
        {
            //piece.rectTransform.SetParent(_offBoardPiecesContainer);

            // Check if the piece is placed in an area off the board.
            //if (_offBoardAreas.Any(t => t.GetWorldRect().Contains(piece.rectTransform.position)))
             //   return;
             if (piece.rectTransform.parent == _draggingPieceContainer)
             {
                 piece.rectTransform.SetParent(_offBoardPiecesContainer);
                 piece.rectTransform.SetAsFirstSibling();
             }
             piece.SnapBack();
        }

    }
}