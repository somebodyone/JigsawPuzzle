using UnityEngine;
using UnityEngine.UI;

namespace DTT.MiniGame.Jigsaw.Demo
{
    /// <summary>
    /// Handles a button appearing after finishing the game, giving the user the ability to restart the game.
    /// </summary>
    [RequireComponent(typeof(Button), typeof(CanvasGroup))]
    public class RestartButton : MonoBehaviour
    {
        /// <summary>
        /// Mini game to restart.
        /// </summary>
        [SerializeField]
        [Tooltip("Mini game to restart")]
        private JigsawManager _miniGame;

        /// <summary>
        /// Button reference.
        /// </summary>
        private Button _button;

        /// <summary>
        /// Canvas group reference.
        /// </summary>
        private CanvasGroup _canvasGroup;

        /// <summary>
        /// Gets necessary components.
        /// </summary>
        private void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            _button = GetComponent<Button>();
            SetVisible(false);
        }

        /// <summary>
        /// Adds click listeners.
        /// </summary>
        private void OnEnable()
        {
            // _button.onClick.AddListener(OnClick);
            _miniGame.Finish += OnFinish;
            _miniGame.Started += OnStart;
        }

        /// <summary>
        /// Removes click listeners.
        /// </summary>
        private void OnDisable()
        {
            // _button.onClick.RemoveListener(OnClick);
            _miniGame.Finish -= OnFinish;
            _miniGame.Started -= OnStart;
        }

        /// <summary>
        /// Called when the button is clicked.
        /// </summary>
        // private void OnClick() => _miniGame.StartGame();

        /// <summary>
        /// Called when the game has started.
        /// </summary>
        private void OnStart() => SetVisible(false);

        /// <summary>
        /// Called when the game has finished.
        /// </summary>
        /// <param name="result">Result of the jigsaw game</param>
        private void OnFinish(JigsawResult result) => SetVisible(false);

        /// <summary>
        /// Sets the button visible.
        /// </summary>
        /// <param name="visible">Whether the button should be visible or not</param>
        private void SetVisible(bool visible)
        {
            _canvasGroup.alpha = visible ? 1 : 0;
            _canvasGroup.interactable = visible;
            _canvasGroup.blocksRaycasts = visible;
        }
    }
}