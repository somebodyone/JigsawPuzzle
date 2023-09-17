using System;
using System.Collections.Generic;
using DLBASE;

namespace DLAM
{
    [Serializable]
    public class CateGorayItemData
    {
        public int type;
        public string namekey;
    }

    [Serializable]
    public class CateGorayData
    {
        public List<CateGorayItemData> datas;
    }
    
    public class CateGorayPresenter : IPresenter<CateGorayPresenter>
    {
        private DLOpition<CateGorayData> _opition;
        private CateGorayData _data=>_opition.data;
        private List<string> CateGorayConfig => CoreDataPresenter.Instance.GameCoreConfig.CateGorayConfig;
        public override void OnInit()
        {
            _opition = DLDataManager.GetOpition<CateGorayData>();
            _data.datas = new List<CateGorayItemData>();
            for (int i = 0; i < CateGorayConfig.Count; i++)
            {
                CateGorayItemData data = new CateGorayItemData();
                data.type = i;
                data.namekey = CateGorayConfig[i];
                _data.datas.Add(data);
            }
        }

        public CateGorayData GetData()
        {
            return _data;
        }
    }
}