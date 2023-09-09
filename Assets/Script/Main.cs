using DLBASE;
using FairyGUI;
using UnityEngine;

namespace DLAM
{
    public class Main : MonoBehaviour
    {
        public static string MainUrl = "Main";
        private GComponent _main;

        public void Awake() 
        {
            DLPlayer.CheckInit();
            DLLoadManager.LoadPakege("Common");
            DLLoadManager.LoadPakege("Main");
            CoreDataPresenter.Instance.OnInit();
            CateGorayPresenter.Instance.OnInit();
            GamePresenter.Instance.OnInit();
            RewardPresenter.Instance.OnInit();
            DailyPuzzlePresenter.Instance.OnInit();
            GameManager.Instance.InitManager();
        }

        public void Start()
        {
            _main = UIPackage.CreateObject(MainUrl, "主页").asCom;
            _main.MakeFullScreen();
            // _main.AddRelation(GRoot.inst, RelationType.Size);
            GRoot.inst.AddChild(_main);
            MainView main = _main as MainView;
            main.Init();
        }
    }
}
