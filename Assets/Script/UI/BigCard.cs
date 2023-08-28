using DLBASE;
using FairyGUI;

namespace DLAM
{
    public class BigCard : GComponent
    {
        public static string URL = "ui://Main/大卡片";
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