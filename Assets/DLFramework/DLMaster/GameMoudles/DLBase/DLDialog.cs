using FairyGUI;
using UnityEngine;

namespace DLBASE
{
    public abstract class DLDialog
    {
        public GComponent contentPlane;
        protected GGraph mask;
        protected virtual bool _showmask => true;
        
        public abstract void OnInit();
        public void SetContentWith(string pakege, string name)
        {
            UIPackage.AddPackage("fairygui/"+pakege);
            contentPlane = UIPackage.CreateObject(pakege, name).asCom;
            contentPlane.size = GRoot.inst.size;
            GRoot.inst.AddChild(contentPlane);
        }

        public virtual void Init()
        {
            if (_showmask)
            {
                mask = new GGraph();
                mask.color = Color.black;
                mask.height = Screen.height;
                mask.width = Screen.width;
                GRoot.inst.AddChild(mask);
            }
            InitData();
            InitCompent();
            InitAddlistioner();
        }

        protected virtual void InitCompent()
        {
            
        }

        protected virtual void InitAddlistioner()
        {
            
        }

        protected virtual void InitData()
        {
            
        }

        public virtual void Close()
        {
            mask?.Dispose();
            contentPlane?.Dispose();
        }
    }
}