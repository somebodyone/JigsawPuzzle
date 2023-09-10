using System;
using System.Collections;
using System.Diagnostics;
using DLAM;
using DTT.MiniGame.Jigsaw.UI;
using DTT.MinigameBase;
using DTT.MinigameBase.UI;
using UnityEngine;

namespace DTT.MiniGame.Jigsaw
{
    /// <summary>
    /// Handles the main logic of the game and acts as a start and finish point.
    /// </summary>
    public class JigsawManager : MonoBehaviour, IElapsed, IRestartable, IFinishedable, IMinigame<JigsawConfig, JigsawResult>
    {
        /// <summary>
        /// The action that will be fired when the game has finished.
        /// </summary>
        public event Action<JigsawResult> Finish;

        public void StartGame(JigsawConfig config)
        {
            
        }

        /// <summary>
        /// The action that will be fired when the game has started.
        /// </summary>
        public event Action Started;

        /// <summary>
        /// Invoked when the game finished.
        /// </summary>
        public event Action Finished;

        /// <summary>
        /// The amount of time in seconds that has elapsed.
        /// </summary>
        public float TimeElapsed => _timer.ElapsedMilliseconds / 1000f;

        /// <summary>
        /// Whether the game is currently paused or not.
        /// </summary>
        public bool IsPaused => _isPaused;

        /// <summary>
        /// Whether the game is currently active.
        /// </summary>
        public bool IsGameActive => _isGameActive;

        /// <summary>
        /// Reference to the UI logic.
        /// </summary>
        [SerializeField]
        [Tooltip("Reference to the UI logic")]
        private JigsawBoardUI _boardUI;

        /// <summary>
        /// Reference to the config of the current game.
        /// </summary>
        [SerializeField]
        [Tooltip("Reference to the config of the current game")]
        private JigsawConfig _config;

        /// <summary>
        /// Whether the game should start on instantiation.
        /// </summary>
        [SerializeField]
        [Tooltip("Whether the game should start on instantiation")]
        private bool _startOnStart = true;

        /// <summary>
        /// A stopwatch to time the duration of the game.
        /// </summary>
        private readonly Stopwatch _timer = new Stopwatch();

        /// <summary>
        /// Current active config.
        /// </summary>
        public JigsawConfig _currentConfig;

        public PhotoData curData;
        /// <summary>
        /// The board logic.
        /// </summary>
        private JigsawBoard _board = new JigsawBoard();

        /// <summary>
        /// The counter to count the amount of misplaced pieces.
        /// </summary>
        private int _misplacedPiecesCounter;

        /// <summary>
        /// Whether the game is currently paused or not.
        /// </summary>
        private bool _isPaused;

        /// <summary>
        /// Whether the game is currently active.
        /// </summary>
        private bool _isGameActive;

        /// <summary>
        /// Uses the components config object to start the game.
        /// </summary>
        // public void StartGame() => StartGame(_config);

        public static JigsawManager Instance;

        public void Awake()
        {
            Instance = this;
        }
        
        /// <summary>
        /// Starts the game with given config.
        /// </summary>
        /// <param name="config">The config the start game with.</param>
        public void StartGame(PhotoData data)
        {
            curData = data;
            _isGameActive = true;
            _isPaused = false;
            _currentConfig = new JigsawConfig();
            Sprite sprite = Resources.Load<Sprite>("animal0");
            _currentConfig.Image = sprite;
            _currentConfig.Image = data.sprite;
            switch (data.selecetId)
            {
                case 0:
                    _currentConfig.Size = new Vector2Int(4,8);
                    break;
                case 1:
                    _currentConfig.Size = new Vector2Int(5,10);
                    break;
                case 2:
                    _currentConfig.Size = new Vector2Int(6,12);
                    break;
                default:
                    _currentConfig.Size = new Vector2Int(4,8);
                    break;
            }
            _misplacedPiecesCounter = 0;
            _boardUI.CleanBoard();

            _board.Initialize(_currentConfig);
            _boardUI.CreateBoard(_board);
        }

        /// <summary>
        /// Forces the game to finish.
        /// </summary>
        public void ForceFinish()
        {
            _isGameActive = false;
            _isPaused = false;
            
            _boardUI.SetBoardInteractable(false);
            JigsawResult result = new JigsawResult(TimeElapsed, _misplacedPiecesCounter);
            Finish?.Invoke(result);
            Finished?.Invoke();
        }

        // /// <summary>
        // /// Starts the game.
        // /// </summary>
        // private IEnumerator Start()
        // {
        //     yield return new WaitForEndOfFrame();
        //     if (_startOnStart)
        //         StartGame();
        // }

        /// <summary>
        /// Subscribes to events.
        /// </summary>
        private void OnEnable()
        {
            _boardUI.PieceDroppedOnBoard += CheckFinish;
            _boardUI.PieceMisplaced += OnPieceMisplaced;
        }

        /// <summary>
        /// Unsubscribes from events.
        /// </summary>
        private void OnDisable()
        {
            _boardUI.PieceDroppedOnBoard -= CheckFinish;
            _boardUI.PieceMisplaced -= OnPieceMisplaced;
            _boardUI.CleanBoard();
        }

        /// <summary>
        /// Whenever a piece is misplaced we increment the counter.
        /// </summary>
        /// <param name="piece">The piece that was misplaced.</param>
        private void OnPieceMisplaced(JigsawPuzzlePieceUI piece) => _misplacedPiecesCounter++;

        /// <summary>
        /// Checks whether the game has finished yet. If it has we finish the game.
        /// </summary>
        /// <param name="piece"></param>
        private void CheckFinish(JigsawPuzzlePieceUI piece)
        {
            if (!_board.VerifyBoard())
                return;
            ForceFinish();
        }
        
        public bool CheckOnePieceCurrent(JigsawPuzzlePieceUI piece)
        {
            return _board.VerifyOnePiece(piece.Piece);
        }

        /// <summary>
        /// Stops the timer of the game.
        /// </summary>
        public void Pause()
        {
            if (!IsGameActive)
                return;

            _isPaused = true;
            _timer.Stop();
            _boardUI.SetBoardInteractable(false);
        }

        /// <summary>
        /// Starts the timer of the game.
        /// </summary>
        public void Continue()
        {
            if (!IsGameActive)
                return;

            _isPaused = false;
            _timer.Start();
            _boardUI.SetBoardInteractable(true);
        }

        /// <summary>
        /// Restarts the game.
        /// </summary>
        public void Restart() => StartGame(_currentConfig);
    }
}