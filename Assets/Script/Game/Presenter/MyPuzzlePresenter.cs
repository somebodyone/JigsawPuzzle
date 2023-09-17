using System;
using System.Collections.Generic;
using DLBASE;

namespace DLAM
{
    [Serializable]
    public class MyPuzzleData
    {
        public List<PhotoData> datas;
    }
    
    public class MyPuzzlePresenter : IPresenter<MyPuzzlePresenter>
    {
        private DLOpition<MyPuzzleData> _opition;
        private MyPuzzleData _data=>_opition.data;
        
        public override void OnInit()
        {
            _opition = DLDataManager.GetOpition<MyPuzzleData>();
            if (_data.datas == null)
            {
                _data.datas = new List<PhotoData>();
            }
        }

        /// <summary>
        /// 完成图片
        /// </summary>
        /// <param name="data"></param>
        public void AchivePhoto(PhotoData data,int time)
        {
            foreach (var item in _data.datas)
            {
                if (item == data)
                {
                    return;
                }
            }

            if (data.times[data.selecetId] < time)
            {
                data.times[data.selecetId] = time;
            }
            _data.datas.Add(data);
            _opition.SetDirty(true);
        }

        public MyPuzzleData GetData()
        {
            return _data;
        }
    }
}