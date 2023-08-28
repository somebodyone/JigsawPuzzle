using FairyGUI;

namespace DLAM
{
    public class MainView:GComponent
    {
        public static string URL = "ui://Main/主页";
        private GList _list;
        private BigCard _bigCard;

        private GameData _data => GamePresenter.Instance.GameData;

        public void Init()
        {
            _list = GetChild("list").asList;
            _bigCard = UIPackage.CreateObjectFromURL(BigCard.URL) as BigCard;
            _bigCard.Init(_data.recommenddata);
            for (int i = 0; i < _data.datas.Count; i++)
            {
                Card card = UIPackage.CreateObjectFromURL(Card.URL) as Card;
                card.Init(_data.datas[i]);
                _list.AddChild(card);
            }
        }
    }
}