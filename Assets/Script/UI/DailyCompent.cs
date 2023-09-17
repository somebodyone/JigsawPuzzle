using DLBASE;
using FairyGUI;

namespace DLAM
{
    public class DailyCompent:GComponent
    {
        public static string URL = "ui://Main/每日任务卡片";
        private Card _card;
        private GLabel _time;
        private PhotoData _data;

        public void OnInit(PhotoData data)
        {
            _data = data;
            _card = GetChild("card") as Card;
            _card.Init(data);
            _time = GetChild("label").asLabel;
            _time.title = GameUtlis.GetMonth(_data.month)+" "+ _data.dayily;
        }
    }
}