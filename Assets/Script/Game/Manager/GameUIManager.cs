using System;
using DLBASE;
using UnityEngine;
using UnityEngine.UI;

namespace DLAM
{
    public class GameUIManager:MonoBehaviour
    {
        public Button _back;
        public static GameUIManager Instance;
        public void Start()
        {
            Instance = this;
            _back.onClick.AddListener(() =>
            {
                GameManager.Instance.EndGame();
                DLDialogManager.Instance.CloseDialog<GameDialog>();
            });
        }
    }
}