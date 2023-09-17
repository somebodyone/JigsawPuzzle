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
            DLLoadManager.LoadPakege("Common");
            DLLoadManager.LoadPakege("Main");
            CoreDataPresenter.Instance.OnInit();
            CateGorayPresenter.Instance.OnInit();
            GamePresenter.Instance.OnInit();
            RewardPresenter.Instance.OnInit();
            DailyPuzzlePresenter.Instance.OnInit();
            GameManager.Instance.InitManager();
            DLPlayer.CheckInit();
        }

        public void Start()
        {
            _main = UIPackage.CreateObject(MainUrl, "主页").asCom;
            _main.MakeFullScreen();
            _main.AddRelation(GRoot.inst, RelationType.Size);
            GameObject go = Instantiate(Resources.Load<GameObject>("Puzzle"));
            SpriteRenderer spriteRenderer = go.GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = GameUtlis.GetSpriteByFGUI("Main", "animal0");
            GRoot.inst.AddChild(_main);
            MainView main = _main as MainView;
            main.Init();
        }
    }
}
