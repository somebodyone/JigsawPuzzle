using DLBASE;

namespace DLAM
{
    public class RewardData
    {
        public int gold;
    }

    public class RewardPresenter : IPresenter<RewardPresenter>
    {
        private DLOpition<RewardData> _opition;
        private RewardData _data;

        public override void OnInit()
        {
            _opition = DLDataManager.GetOpition<RewardData>();
            _data = _opition.data;
        }

        public long GOLD => _data.gold;
        
        public RewardData GetData()
        {
            return _data;
        }
    }
}