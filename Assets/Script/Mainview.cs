using System;
using FairyGUI;

namespace DLAM
{
    public enum PageEnum
    {
        HomePage,
        DailyPuzzle,
        Category,
        Mypuzzle
    }
    
    public class MainView:GComponent
    {
        public static string URL = "ui://Main/主页";
        private GList _list;
        private BigCard _bigCard;
        private Controller _controller;
        private CateGorayPage _cateGorayPage;

        private GComponent _top;
        private GComponent _down;
        private GButton _homepage;
        private GButton _dailypuzzle;
        private GButton _category;
        private GButton _mypuzzle;
        private GameData _data => GamePresenter.Instance.GameData;
        private RewardData _rewardData => RewardPresenter.Instance.GetData();

        public void Init()
        {
            _list = GetChild("list").asList;
            _bigCard = UIPackage.CreateObjectFromURL(BigCard.URL) as BigCard;
            _bigCard.Init(_data.recommenddata);
            _controller = GetController("type");
            for (int i = 0; i < _data.datas.Count; i++)
            {
                Card card = UIPackage.CreateObjectFromURL(Card.URL) as Card;
                card.Init(_data.datas[i]);
                _list.AddChild(card);
            }
            _cateGorayPage = GetChild("cateGorayPage") as CateGorayPage;
            _cateGorayPage.OnInit();
            _down = GetChild("down").asCom;
            _homepage = _down.GetChild("homepage").asButton;
            _dailypuzzle = _down.GetChild("dailypuzzle").asButton;
            _category = _down.GetChild("category").asButton;
            _mypuzzle = _down.GetChild("mypuzzle").asButton;
            _top = GetChild("top").asCom;
            _homepage.onClick.Add(() => { ChangePage(PageEnum.HomePage);});
            _dailypuzzle.onClick.Add(() => { ChangePage(PageEnum.DailyPuzzle);});
            _category.onClick.Add(() => { ChangePage(PageEnum.Category);});
            _mypuzzle.onClick.Add(() => { ChangePage(PageEnum.Mypuzzle);});
        }

        private void ChangePage(PageEnum type)
        {
            switch (type)
            {
                case PageEnum.HomePage:
                    _controller.SetSelectedPage("HomePage");
                    break;
                case PageEnum.DailyPuzzle:
                    _controller.SetSelectedPage("DailyPuzzle");
                    break;
                case PageEnum.Category:
                    _controller.SetSelectedPage("Category");
                    break;
                case PageEnum.Mypuzzle:
                    _controller.SetSelectedPage("Mypuzzle");
                    break;
            }
        }
    }
}