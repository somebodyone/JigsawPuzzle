using System.Collections.Generic;
using DLBASE;

namespace DLAM
{
    public class MyPuzzleData
    {
        public List<PhotoData> datas;
    }
    
    public class MyPuzzlePresenter : IPresenter<MyPuzzlePresenter>
    {
        private DLOpition<MyPuzzleData> _opition;
        private MyPuzzleData _data;
        
        public override void OnInit()
        {
            _opition = DLDataManager.GetOpition<MyPuzzleData>();
            _data = _opition.data;
            if (_data.datas == null)
            {
                _data.datas = new List<PhotoData>();
            }
        }

        /// <summary>
        /// 完成图片
        /// </summary>
        /// <param name="data"></param>
        public void AchivePhoto(PhotoData data)
        {
            foreach (var item in _data.datas)
            {
                if (item == data)
                {
                    return;
                }
            }
            _data.datas.Add(data);
        }

        public MyPuzzleData GetData()
        {
            return _data;
        }
    }
}