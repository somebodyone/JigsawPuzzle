using System;
using DLBASE;
using DTT.MiniGame.Jigsaw;
using DTT.MinigameBase.UI;
using DTT.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace DLAM
{
    public class GameUIManager:MonoBehaviour
    {
        public Button _back;
        public Button _prop;
        public Button _eye;
        public Button test;
        public static GameUIManager Instance;
        [SerializeField]
        // private GamePopupUI _popup;

        private bool isShowBg;
        public void Start()
        {
            Instance = this;

            _eye.onClick.AddListener(() =>
            {
                ToolBgManager.Ins.Show();
            });
            _prop.onClick.AddListener(() =>
            {
                JigsawManager.Instance.FlyToPos();
            });
            _back.onClick.AddListener(() =>
            {
                PauseGame();
            });
            test.onClick.AddListener(() =>
            {
                JigsawManager.Instance.ForceFinish();
            });
        }
        
        private void OnEnable()
        {
            // _popup.ResumeButtonPressed += ResumeGame;
            // _popup.HomeButtonPressed += ToHome;
        }

        /// <summary>
        /// Removes events.
        /// </summary>
        private void OnDisable()
        {

            // _popup.ResumeButtonPressed -= ResumeGame;
            // _popup.HomeButtonPressed -= ToHome;
        }

        
        /// <summary>
        /// Sets the UI in a state for when the game resumes.
        /// </summary>
        private void ResumeGame()
        {
            // _popup.Show(false);
        }

        /// <summary>
        /// Sets the UI in a state for when the game resumes.
        /// </summary>
        private void PauseGame()
        {
            // _popup.Show(true);
            // _popup.SetTitleToPaused();
            // _popup.EnableResumeButton(true);
            PopUpManager.Ins.Show();
        }

        /// <summary>
        /// Sets the UI in a state for when the game goes back to home.
        /// </summary>
        private void ToHome()
        {
            GameManager.Instance.EndGame();
            DLDialogManager.Instance.CloseDialog<GameDialog>();
            //todo ui界面结算奖励
        }
        
    }
}