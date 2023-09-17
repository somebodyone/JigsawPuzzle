using System;
using DLBASE;
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
        private DailyPuzzlePage _dailyPuzzlePage;
        private MyPuzzlePage _myPuzzlePage;

        private GComponent _top;
        private GComponent _down;
        private GButton _homepage;
        private GButton _dailypuzzle;
        private GButton _category;
        private GButton _mypuzzle;
        private GButton _setbtn;
        private GButton _gold;

        private GButton _personal;
        
        private GameData _data => GamePresenter.Instance.GameData;
        private RewardData _rewardData => RewardPresenter.Instance.GetData();

        public void Init()
        {
            _list = GetChild("list").asList;
            _bigCard = UIPackage.CreateObjectFromURL(BigCard.URL) as BigCard;
            _bigCard.Init(_data.recommenddata);
            _controller = GetController("type");
            _cateGorayPage = GetChild("cateGorayPage") as CateGorayPage;
            _cateGorayPage.OnInit();
            _dailyPuzzlePage = GetChild("dailyPuzzle") as DailyPuzzlePage;
            _dailyPuzzlePage.Init();
            _myPuzzlePage = GetChild("mypuzzle") as MyPuzzlePage;
            _myPuzzlePage.OnInit();
            _down = GetChild("down").asCom;
            _homepage = _down.GetChild("homepage").asButton;
            _dailypuzzle = _down.GetChild("dailypuzzle").asButton;
            _category = _down.GetChild("category").asButton;
            _mypuzzle = _down.GetChild("mypuzzle").asButton;
            _top = GetChild("top").asCom;
            _personal = _top.GetChild("personal").asButton;
            _gold = _top.GetChild("reward").asButton;
            _setbtn = _top.GetChild("set").asButton;
            _homepage.onClick.Add(() => { ChangePage(PageEnum.HomePage);});
            _setbtn.onClick.Add(() =>
            {
                DLDialogManager.Instance.OpenDialog<SettingDialog>();
            });
            _dailypuzzle.onClick.Add(() => { ChangePage(PageEnum.DailyPuzzle);});
            _category.onClick.Add(() => { ChangePage(PageEnum.Category);});
            _mypuzzle.onClick.Add(() => { ChangePage(PageEnum.Mypuzzle);});
            RewardPresenter.Instance.lisioner.UpdateGold(this, () =>
            {
                _gold.title = _rewardData.gold.ToString();
            });
            GamePresenter.Instance.lisioner.UpdatePhoto(this, UpdateCompent);
            _personal.onClick.Add(() =>
            {
                DLDialogManager.Instance.OpenDialog<PersonalDialog>();
            });
            UpdateCompent();
        }

        private void UpdateCompent()
        {
            _list.RemoveChildrenToPool();
            for (int i = 0; i < _data.datas.Count; i++)
            {
                Card card = UIPackage.CreateObjectFromURL(Card.URL) as Card;
                card.Init(_data.datas[i]);
                _list.AddChild(card);
            }
        }
        private void ChangePage(PageEnum type)
        {
            _homepage.GetController("select").SetSelectedPage("on");
            _dailypuzzle.GetController("select").SetSelectedPage("on");
            _category.GetController("select").SetSelectedPage("on");
            _mypuzzle.GetController("select").SetSelectedPage("on");
            switch (type)
            {
                case PageEnum.HomePage:
                    _controller.SetSelectedPage("HomePage");
                    _homepage.GetController("select").SetSelectedPage("off");
                    break;
                case PageEnum.DailyPuzzle:
                    _controller.SetSelectedPage("DailyPuzzle");
                    _dailypuzzle.GetController("select").SetSelectedPage("off");
                    break;
                case PageEnum.Category:
                    _controller.SetSelectedPage("Category");
                    _category.GetController("select").SetSelectedPage("off");
                    break;
                case PageEnum.Mypuzzle:
                    _controller.SetSelectedPage("Mypuzzle");
                    _mypuzzle.GetController("select").SetSelectedPage("off");
                    _myPuzzlePage.UpdateCompent();
                    break;
            }
        }
    }
}