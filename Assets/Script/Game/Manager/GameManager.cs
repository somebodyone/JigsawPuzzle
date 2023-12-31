using DLBASE;
using DTT.MiniGame.Jigsaw;
using FairyGUI;
using UnityEngine;

namespace DLAM
{
    public class GameManager:DLSingleton<GameManager>,IManager
    {
        private GameObject _perfab;
        private GameObject _game;
        
        public void InitManager()
        {
            _perfab = Resources.Load<GameObject>("Game");
            UIObjectFactory.SetPackageItemExtension(MainView.URL, typeof(MainView));
            UIObjectFactory.SetPackageItemExtension(BigCard.URL, typeof(BigCard));
            UIObjectFactory.SetPackageItemExtension(Card.URL, typeof(Card));
            UIObjectFactory.SetPackageItemExtension(CateGorayPage.URL, typeof(CateGorayPage));
            UIObjectFactory.SetPackageItemExtension(DiffctyCom.URL, typeof(DiffctyCom));
            UIObjectFactory.SetPackageItemExtension(DailyPuzzlePage.URL, typeof(DailyPuzzlePage));
            UIObjectFactory.SetPackageItemExtension(DailyPuzzleCompent.URL, typeof(DailyPuzzleCompent));
            UIObjectFactory.SetPackageItemExtension(DailyCompent.URL, typeof(DailyCompent));
            UIObjectFactory.SetPackageItemExtension(MyPuzzlePage.URL, typeof(MyPuzzlePage));
            UIObjectFactory.SetPackageItemExtension(MyPuzzleCard.URL, typeof(MyPuzzleCard));
        }

        public void GameStart()
        {
            _game = Object.Instantiate(_perfab);
        }

        public void EndGame()
        {
            GameObject.Destroy(_game);
        }
    }
}