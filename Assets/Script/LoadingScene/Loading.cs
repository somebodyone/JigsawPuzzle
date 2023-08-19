using System;
using System.Collections;
using System.Collections.Generic;
using DLBASE;
using FairyGUI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LOADING
{
    public class Loading : MonoBehaviour
    {
        public static string LoadingUrl = "Loading";
        private UIPanel _uiPanel;
        private GComponent _ui;
        private GProgressBar _progressBar;
        private float _timesub = 0.5f;
        private float _time = 0;

        public void Awake()
        {
            DLLoadManager.LoadPakege(LoadingUrl);
        }

        public void Start()
        {
            _ui = UIPackage.CreateObject(LoadingUrl, "加载页").asCom;
            _ui.size = GRoot.inst.size;
            GRoot.inst.AddChild(_ui);
            _progressBar = _ui.GetChild("slider").asProgress;
            _progressBar.value = 0;
        }

        public void FixedUpdate()
        {
            _time += _timesub;
            if (_time > 1)
            {
                _progressBar.value += 20f;
                _time = 0;
            }

            if (_progressBar.value >= 100)
            {
                SceneManager.LoadScene("MainScene");
                _ui.Dispose();
            }
        }
    }
}
