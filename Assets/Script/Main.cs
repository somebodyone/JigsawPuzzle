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
            
            GamePresenter.Instance.OnInit();
            
            GameManager.Instance.InitManager();
        }

        public void Start()
        {
            _main = UIPackage.CreateObject(MainUrl, "主页").asCom;
            _main.size = GRoot.inst.size;
            GRoot.inst.AddChild(_main);
            MainView main = _main as MainView;
            main.Init();
        }
    }
}
