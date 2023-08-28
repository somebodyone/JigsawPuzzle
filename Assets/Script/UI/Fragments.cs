using FairyGUI;
using UnityEngine;

namespace DLAM
{
    public class Fragments : GButton
    {
        public static string URL = "ui://Main/3x6";
        private GLoader _loader;
        private PhotoData _data;
        private Material _material;
        private Rect _rect;
        private float _scalex;
        private float _scaley;

        public void Init(PhotoData data)
        {
            _data = data;
            size = new Vector2(GRoot.inst.width / 3, GRoot.inst.height / 6);
            _loader = GetChild("icon").asLoader;
            _loader.size = GRoot.inst.size;
            _loader.url = "ui://Main/people_" + _data.id;
            _rect = _loader.image.texture.uvRect;
            _material = new Material(Shader.Find("Unlit/Image-Card"));
            // _material.SetFloat("_U",  _rect.x);
            // _material.SetFloat("_V", -_rect.y);
            // _material.SetFloat("_X", 0.33F - _rect.x);
            // _material.SetFloat("_Y", -0.33F);
            // _material.SetFloat("_CenterX", _rect.x+_rect.width/3);
            // _material.SetFloat("_CenterY", _rect.y+_rect.height/3);
            // _material.SetFloat("_Scale", 3);
            // _scaley = _rect.height / 3f;
            // _scalex = _rect.height / 6f;
            Debug.Log(_scaley +"_scaley+++++++++++++++++++");
            Debug.Log(_scalex +"_scalex+++++++++++++++++++");
            _loader.image.material = _material;
            onTouchBegin.Add(OnTouchBegin);
            onTouchMove.Add(OnTouchMove);
            onTouchEnd.Add(OnTouchEnd);
        }

        private void OnTouchEnd(EventContext context)
        {
            
        }

        private void OnTouchMove(EventContext context)
        {
            xy = Stage.inst.touchPosition;
        }

        private void OnTouchBegin(EventContext context)
        {
            
        }
    }
}