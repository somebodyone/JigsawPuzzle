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
            for (int i = 0; i < _data.datas.Count; i++)
            {
                DailyPuzzleCompent item =
                    UIPackage.CreateObjectFromURL(DailyPuzzleCompent.URL) as DailyPuzzleCompent;
                item.Init(_data.datas[i]);
                _list.AddChild(item);
            }
        }
    }
}