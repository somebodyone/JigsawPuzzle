using System;
using DLBASE;

namespace DLAM
{
    [Serializable]
    public class RewardData
    {
        public int gold;
    }

    public class RewardPresenter : IPresenter<RewardPresenter>
    {
        public enum EventType
        {
            UpdateGold
        }
        public class Lisioner : DLLisioner
        {
            public void UpdateGold(object key, Action callback)
            {
                Add(key, EventType.UpdateGold, callback);
            }
        }
        
        private DLOpition<RewardData> _opition;
        private RewardData _data=>_opition.data;
        public  Lisioner lisioner = new Lisioner();
        public override void OnInit()
        {
            _opition = DLDataManager.GetOpition<RewardData>();
        }

        public long GOLD => _data.gold;

        public void AddGold(int gold)
        {
            _data.gold += gold;
            _opition.SetDirty(true);
            lisioner.Emit(EventType.UpdateGold);
        }
        
        public RewardData GetData()
        {
            return _data;
        }
    }
}