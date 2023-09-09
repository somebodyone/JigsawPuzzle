using DLBASE;

namespace DLAM
{
    public class DailyDialog:DLDialogX<DailyDialog>
    {
        public override void OnInit()
        {
            SetContentWith("Dialog", "游戏进入选择界面");
        }
    }
}