using DLBASE;
using FairyGUI;
using UnityEngine;

namespace DLAM
{
    public class TipsManager:DLSingleton<TipsManager>,IManager
    {
        public static string URL = "ui://Common/提示框";
        
        public void InitManager()
        {
            
        }

        public static void ShowTips(string url)
        {
            GLabel tips = UIPackage.CreateObjectFromURL(URL).asLabel;
            tips.title = url;
            GRoot.inst.AddChild(tips);
            float y = GRoot.inst.height / 2.0f;
            tips.xy = new Vector2(0,y );
            tips.TweenMoveY(y - 100, 1).OnComplete(() =>
            {
                tips.Dispose();
            });
        }
    }
}