using DLBASE;
using FairyGUI;

namespace DLAM
{
    public class Card : GButton
    {
        public static string URL = "ui://Main/小卡片";
        private PhotoData _data;

        public void Init(PhotoData data)
        {
            _data = data;
            icon = "ui://Main/people_" + data.id;
            onClick.Add(() =>
            {
                DLDialogManager.Instance.OpenDialog<GameView>(_data);
            });
        }
    }
}