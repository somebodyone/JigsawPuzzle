using DLBASE;
using FairyGUI;
using UnityEngine;

namespace DLAM
{
    public class UnlockPhotoDialog : DLDialogX<UnlockPhotoDialog>
    {
        private GLabel _iconLabel;
        private GButton _goldbtn;
        private GComponent _adbtn;
        private GButton _backbtn;
        private Transition _closetransition;
        private PhotoData _data;
        private long _gold => RewardPresenter.Instance.GOLD;

        public override void OnInit()
        {
            SetContentWith("Dialog", "解锁卡牌弹窗");
        }

        public override void InitData(params object[] args)
        {
            base.InitData(args);
            _data = (PhotoData)args[0];
        }

        protected override void InitCompent()
        {
            _iconLabel = contentPlane.GetChild("title").asLabel;
            _goldbtn = contentPlane.GetChild("goldbtn").asButton;
            _backbtn = contentPlane.GetChild("back").asButton;
            _adbtn = contentPlane.GetChild("adbtn").asButton;
            _closetransition = contentPlane.GetTransition("close");
            _iconLabel.icon = "ui://Main/" + _data.photoname + _data.id;
            _backbtn.onClick.Add(() =>
            {
                _closetransition.Play(1,0, () =>
                {
                    DLDialogManager.Instance.CloseDialog<UnlockPhotoDialog>();
                });
            });
            _goldbtn.onClick.Add(() =>
            {
                if (_gold < _data.lockgold)
                {
                    Debug.Log("金钱不足");
                }
            });
            _adbtn.onClick.Add(() =>
            {
                AdSdkPresenter.Instance.CheckShowAD(() =>
                {
                    GamePresenter.Instance.UnLockPhoto(_data);
                    DLDialogManager.Instance.CloseDialog<UnlockPhotoDialog>();
                });
            });
        }
    }
}