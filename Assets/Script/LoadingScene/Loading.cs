using DLAM;
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
        private GTextField _field;
        private float _timesub = 1.5f;
        private float _time = 0;

        public void Awake()
        {
            DLLoadManager.LoadPakege(LoadingUrl);
        }

        public void Start()
        {
            _ui = UIPackage.CreateObject(LoadingUrl, "加载页").asCom;
            _ui.size = GRoot.inst.size;
            _ui.AddRelation(GRoot.inst,RelationType.Size);
            GRoot.inst.AddChild(_ui);
            _progressBar = _ui.GetChild("slider").asProgress;
            _field = _progressBar.GetChild("text").asTextField;
            _progressBar.value = 0;
        }

        public void FixedUpdate()
        {
            _time += _timesub;
            if (_time > 1)
            {
                _progressBar.value += 1f;
                _time = 0;
                _field.text = _progressBar.value + "%";
            }

            if (_progressBar.value >= 100)
            {
                _field.text = "100%";
                GameStart();
                _ui.Dispose();
                Destroy(gameObject);
            }
        }

        private void GameStart()
        {
            GameObject go = new GameObject();
            go.name = "Game";
            go.AddComponent<Main>();
        }
    }
}
