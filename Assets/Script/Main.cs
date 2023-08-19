using System;
using System.Collections;
using System.Collections.Generic;
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
        }

        public void Start()
        {
            _main = UIPackage.CreateObject(MainUrl, "主页").asCom;
            _main.size = GRoot.inst.size;
            GRoot.inst.AddChild(_main);
        }
    }
}
