using FairyGUI;

namespace DLAM
{
    public class DailyPuzzlePage : GComponent
    {
        public static string URL = "ui://Main/每日拼图";
        private GList _list;
        private DailyPuzzleData _data;

        public void Init()
        {
            _list = GetChild("list").asList;
            _data = DailyPuzzlePresenter.Instance.GetData();

            DailyPuzzleCompent item =
                UIPackage.CreateObjectFromURL(DailyPuzzleCompent.URL) as DailyPuzzleCompent;
            item.Init(_data.datas[_data.datas.Count-1]);
            _list.AddChild(item);
            
            for (int i = 0; i <_data.datas.Count-1; i++)
            {
                DailyCompent items =
                    UIPackage.CreateObjectFromURL(DailyCompent.URL) as DailyCompent;
                items.OnInit(_data.datas[i]);
                _list.AddChild(items);
            }
        }
    }
}