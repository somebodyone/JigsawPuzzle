using FairyGUI;

namespace DLAM
{
    public class MyPuzzlePage:GComponent
    {
        public static string URL = "ui://Main/我的拼图";
        private MyPuzzleData _data => MyPuzzlePresenter.Instance.GetData();
        private GList _list;

        public void OnInit()
        {
            _list = GetChild("list").asList;
            for (int i = 0; i < _data.datas.Count; i++)
            {
                PhotoData data = _data.datas[i];
                MyPuzzleCard card = UIPackage.CreateObjectFromURL(MyPuzzleCard.URL) as MyPuzzleCard;
                card.OnInit(data);
                _list.AddChild(card);
            }
        }
    }
}