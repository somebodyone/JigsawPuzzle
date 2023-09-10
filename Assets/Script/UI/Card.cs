using DLBASE;
using FairyGUI;

namespace DLAM
{
    public class Card : GButton
    {
        public static string URL = "ui://Main/小卡片";
        private PhotoData _data;
        private Controller _controller;
        private GButton _collect;
        private GButton _lock;
        private GButton _cardbtn;

        public void Init(PhotoData data)
        {
            _data = data;
            _controller = GetController("type");
            _collect = GetChild("collect").asButton;
            _lock = GetChild("lock").asButton;
            _cardbtn = GetChild("card").asButton;
            _cardbtn.icon = "ui://Main/"+data.photoname + data.id;
            _collect.onClick.Add(() =>
            {
                GamePresenter.Instance.CollectPhoto(_data);
                UpdateCompent();
            });
            _lock.onClick.Add(() =>
            {
                DLDialogManager.Instance.OpenDialog<UnlockPhotoDialog>(_data);
            });
            _cardbtn.onClick.Add(() =>
            {
                 if (_data.islock)
                {
                    DLDialogManager.Instance.OpenDialog<UnlockPhotoDialog>(_data);
                    // TipsManager.ShowTips("PLEASE UNLOCK PICTRUE");
                }
                else
                {
                    DLDialogManager.Instance.OpenDialog<SelectDifficultyDialog>(_data);
                }
            });
            UpdateCompent();
        }

        private void UpdateCompent()
        {
            string key = "all";
            if (_data.islock)
            {
                key = "all";
            }else 
            {
                key = "collect";
            }
            _controller.SetSelectedPage(key);
            _collect.GetController("select").SetSelectedPage(_data.collect?"off":"on");
        }
    }
}