using System;
using FairyGUI;

namespace DLAM
{
    public class DiffctyCom : GButton
    {
        public static string URL = "ui://Dialog/难度选择子项";
        private Controller _controller;
        private GTextField _reward;
        private Action<DiffctyCom> _click;
        public int ID;

        public void OnInit(int id,int reward)
        {
            ID = id;
            _controller = GetController("select");
            _reward = GetChild("reward").asTextField;
            _reward.text = reward.ToString();
            title = "Difficulty" + id;
            onClick.Add(() =>
            {
                _click?.Invoke(this);
            });
        }

        public void AddClick(Action<DiffctyCom> click)
        {
            _click = click;
        }

        public void Select(bool on)
        {
            _controller.SetSelectedPage(on ? "on" : "off");
        }
    }
}