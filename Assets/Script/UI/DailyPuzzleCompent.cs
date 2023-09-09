using DLBASE;
using FairyGUI;

namespace DLAM
{
    public class DailyPuzzleCompent:GComponent
    {
        public static string URL = "ui://Main/每日";
        private GButton _entergame;
        private GTextField _time;
        private GButton _loader;
        private PhotoData _data;

        public void Init(PhotoData data)
        {
            _data = data;
            _entergame = GetChild("enter").asButton;
            _time = GetChild("time").asTextField;
            _loader = GetChild("card").asButton;
            _loader.icon = "ui://Main/"+data.photoname + data.id;
            _entergame.onClick.Add(() =>
            {
                DLDialogManager.Instance.OpenDialog<SelectDifficultyDialog>(_data);
            });
        }
    }
}