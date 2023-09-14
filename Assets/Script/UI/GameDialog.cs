using DLBASE;
using DTT.MiniGame.Jigsaw;
using DTT.MinigameBase.UI;
using FairyGUI;

namespace DLAM
{
    public class GameDialog:DLDialogX<GameDialog>
    {
        private PhotoData _data;
        private GButton _back;

        public override void OnInit()
        {
            SetContentWith("Main","游戏主页");
        }

        public override void InitData(params object[] args)
        {
            base.InitData(args);
            _data = (PhotoData)args[0];
        }

        protected override void InitCompent()
        {
            _back = contentPlane.GetChild("back").asButton;
            GameManager.Instance.GameStart();
            JigsawManager.Instance.StartGame(_data);
        }

        protected override void InitAddlistioner()
        {
            _back.onClick.Add(() =>
            {
                DLDialogManager.Instance.CloseDialog<GameDialog>();
            });
        }
    }
}