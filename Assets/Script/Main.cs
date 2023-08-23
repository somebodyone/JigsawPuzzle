using System;
using System.Collections;
using System.Collections.Generic;
using DLBASE;
using FairyGUI;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

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
        }

        public void Start()
        {

            _main = UIPackage.CreateObject(MainUrl, "主页").asCom;
            _main.size = GRoot.inst.size;
            GRoot.inst.AddChild(_main);
            _main.GetChild("Btn1").asCom.onClick.Add(() =>
            {
                DataMgr.SetCurLevelData(new Vector2Int(2,3),"test");
                AsyncOperation async = SceneManager.LoadSceneAsync("DemoScene");
            });
            _main.GetChild("Btn2").asCom.onClick.Add(() =>
            {
                DataMgr.SetCurLevelData(new Vector2Int(4,6),"test");
                AsyncOperation async = SceneManager.LoadSceneAsync("DemoScene");
            });
            _main.GetChild("Btn3").asCom.onClick.Add(() =>
            {
                DataMgr.SetCurLevelData(new Vector2Int(2,3),"test2");
                AsyncOperation async = SceneManager.LoadSceneAsync("DemoScene");
            });
            _main.GetChild("Btn4").asCom.onClick.Add(() =>
            {
                DataMgr.SetCurLevelData(new Vector2Int(4,6),"test2");
                AsyncOperation async = SceneManager.LoadSceneAsync("DemoScene");
            });

        }
    }
}
