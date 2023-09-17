using FairyGUI;

namespace DLAM
{
    public class MyPuzzleCard:GComponent
    {
        public static string URL = "ui://Main/我的拼图子项";
        private PhotoData _data;

        public void OnInit(PhotoData data)
        {
            _data = data;
        }
        
    }
}