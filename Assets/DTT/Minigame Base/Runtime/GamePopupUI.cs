using System;
using DTT.MiniGame.Jigsaw;
using DTT.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace DTT.MinigameBase.UI
{
    /// <summary>
    /// Handles the standardized popup in the game.
    /// </summary>
    public class GamePopupUI : MonoBehaviour
    {
        
        [SerializeField]
        private JigsawManager _miniGame;
        
        /// <summary>
        /// Called when the resume button is pressed.
        /// </summary>
        public event Action ResumeButtonPressed;
        
        /// <summary>
        /// Called when the restart button is pressed.
        /// </summary>
        public event Action RestartButtonPressed;
        
        /// <summary>
        /// Called when the home button is pressed.
        /// </summary>
        public event Action HomeButtonPressed;
        
        public event Action OnFinished;
        
        /// <summary>
        /// The text object for the title.
        /// </summary>
        [SerializeField]
        public Text _titleText;
        
        /// <summary>
        /// The text object for the backdrop of the title.
        /// </summary>
        [SerializeField]
        private Text _titleBackdropText;

        /// <summary>
        /// The button for resuming.
        /// </summary>
        [SerializeField]
        private Button _resumeButton;
        
        /// <summary>
        /// The button for restarting.
        /// </summary>
        [SerializeField]
        private Button _restartButton;
        
        /// <summary>
        /// The button for returning to home.
        /// </summary>
        [SerializeField]
        private Button _homeButton;

        /// <summary>
        /// Canvas group of the entire popup.
        /// </summary>
        [SerializeField]
        private CanvasGroup _canvasGroup;

        /// <summary>
        /// The animation of showing the popup.
        /// </summary>
        private Coroutine _showAnimation;
        
        
        [SerializeField]
        private RectTransform buttons;
        
        [SerializeField]
        private RectTransform reward;
        
        [SerializeField]
        private Text rewardValue;
        
        public enum PopType
        {
            normal,
            reward,
        }
        
        /// <summary>
        /// Adds listeners.
        /// </summary>
        private void OnEnable()
        {
            _miniGame.Finish += FinishGame;
            _resumeButton.onClick.AddListener(OnResumeButtonClicked);
            //_restartButton.onClick.AddListener(OnRestartButtonClicked);
            _homeButton.onClick.AddListener(OnHomeButtonClicked);
        }

        /// <summary>
        /// Removes listeners.
        /// </summary>
        private void OnDisable()
        {
            _miniGame.Finish -= FinishGame;
            _resumeButton.onClick.RemoveListener(OnResumeButtonClicked);
            //_restartButton.onClick.RemoveListener(OnRestartButtonClicked);
            _homeButton.onClick.RemoveListener(OnHomeButtonClicked);
        }

        
        private void FinishGame(JigsawResult result)
        {
            Show(true,PopType.reward);
            SetTitleToFinished();
        }
        
        /// <summary>
        /// Called when the resume button is pressed.
        /// </summary>
        private void OnResumeButtonClicked() => ResumeButtonPressed?.Invoke();

        /// <summary>
        /// Called when the restart button is pressed.
        /// </summary>
        private void OnRestartButtonClicked() => RestartButtonPressed?.Invoke();

        /// <summary>
        /// Called when the home button is pressed.
        /// </summary>
        private void OnHomeButtonClicked()=> HomeButtonPressed?.Invoke();
        
        /// <summary>
        /// Sets the title for the paused state.
        /// </summary>
        public void SetTitleToPaused()
        {
            _titleText.text = "GAME PAUSED";
            _titleBackdropText.text = "GAME PAUSED";
        }

        /// <summary>
        /// Sets the title for the finished state.
        /// </summary>
        public void SetTitleToFinished()
        {
            _titleText.text = "GAME FINISHED";
            _titleBackdropText.text = "GAME FINISHED";
        }

        /// <summary>
        /// Enables the resume button, so it's shown to the user.
        /// </summary>
        /// <param name="isEnabled">Whether to enable or disable</param>
        public void EnableResumeButton(bool isEnabled) => _resumeButton.gameObject.SetActive(isEnabled);
        
        /// <summary>
        /// Enables the restart button, so it's shown to the user.
        /// </summary>
        /// <param name="isEnabled">Whether to enable or disable</param>
        public void EnableRestartButton(bool isEnabled) => _restartButton.gameObject.SetActive(isEnabled);
        
        /// <summary>
        /// Enables the home button, so it's shown to the user.
        /// </summary>
        /// <param name="isEnabled">Whether to enable or disable</param>
        public void EnableHomeButton(bool isEnabled) => _homeButton.gameObject.SetActive(isEnabled);

        /// <summary>
        /// Shows the popup based on the state.
        /// </summary>
        /// <param name="state">Whether to show the popup.</param>
        public void Show(bool state,PopType type = PopType.normal)
        {
            if(_showAnimation != null)
                StopCoroutine(_showAnimation);

            _canvasGroup.interactable = state;
            _canvasGroup.blocksRaycasts = state;

            _canvasGroup.alpha = state ? 1f : 0f;

            switch (type)
            {
                case PopType.normal:
                    _resumeButton.transform.gameObject.SetActive(true);
                    reward.gameObject.SetActive(false);
                    buttons.transform.localPosition = new Vector3(0, -80, 0);
                    break;
                case PopType.reward:
                    _resumeButton.transform.gameObject.SetActive(false);
                    reward.gameObject.SetActive(true);
                    buttons.transform.localPosition = new Vector3(0, -160, 0);
                    rewardValue.text =
                        $":{JigsawManager.Instance.curData.reward[JigsawManager.Instance.curData.selecetId]}";
                    break;
            }
            

        }
    }
}