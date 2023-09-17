using DLBASE;
using FairyGUI;

namespace DLAM
{
    public class PersonalDialog:DLDialogX<PersonalDialog>
    {
        private GButton _close;
        protected override bool _showMask => true;

        public override void OnInit()
        {
            SetContentWith("Dialog","个人中心弹窗");
        }

        protected override void InitCompent()
        {
            _close = contentPlane.GetChild("close").asButton;
        }

        protected override void InitAddlistioner()
        {
            _close.onClick.Add(() =>
            {
                mask.TweenFade(0, 0.25f);
                contentPlane.TweenMoveY(GRoot.inst.height, 0.3f).OnComplete(() =>
                {
                    DLDialogManager.Instance.CloseDialog<PersonalDialog>();
                });
            });
        }
    }
}