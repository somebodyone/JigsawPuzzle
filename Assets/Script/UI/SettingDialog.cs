using DLBASE;
using FairyGUI;

namespace DLAM
{
    public class SettingDialog:DLDialogX<SettingDialog>
    {
        private GButton _close;
        private Transition _transition;
        protected override bool _showMask => true;

        public override void OnInit()
        {
            SetContentWith("Setting", "SettingDialog");
        }

        protected override void InitCompent()
        {
            _close = contentPlane.GetChild("close").asButton;
            _transition = contentPlane.GetTransition("close");
            _close.onClick.Add(() =>
            {
                contentPlane.TweenMoveY(GRoot.inst.height, 0.3f).OnComplete(() =>
                {
                    DLDialogManager.Instance.CloseDialog<SettingDialog>();
                });
            });
        }
        
    }
}