using FairyGUI;
using UnityEngine;

namespace DLBASE
{
    public abstract class DLDialog
    {
        public GComponent contentPlane;
        protected GGraph mask;
        protected virtual bool _showMask => false;
        public abstract void OnInit();
        public void SetContentWith(string pakege, string name)
        {
            ShowModelLayer();
            UIPackage.AddPackage("fairygui/"+pakege);
            contentPlane = UIPackage.CreateObject(pakege, name).asCom;
            contentPlane.size = GRoot.inst.size;
            GRoot.inst.AddChild(contentPlane);
            contentPlane.AddRelation(GRoot.inst,RelationType.Size);
            contentPlane.SetPivot(0.5f,0.5f);
        }

        public virtual void Init()
        {
            InitCompent();
            InitAddlistioner();
        }
        
        protected virtual void ShowModelLayer()
        {
            if(!_showMask)return;
            if (mask != null)
            {
                return;
            }

            mask = new GGraph();
            mask.DrawRect(GRoot.inst.width, GRoot.inst.height, 0, Color.white, Color.black);
            mask.AddRelation(GRoot.inst, RelationType.Size);
            mask.name = mask.gameObjectName = "DialogModalLayer";
            GRoot.inst.AddChild(mask);
            mask.alpha = 0;
            mask.TweenFade(0.9f, 0.2f);
        }

        protected virtual void InitCompent()
        {
            
        }

        protected virtual void InitAddlistioner()
        {
            
        }

        public virtual void InitData(params object[] args)
        {
            
        }

        public virtual void Close()
        {
            mask?.Dispose();
            contentPlane?.Dispose();
        }
    }
}