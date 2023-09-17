using System.Collections.Generic;
using DLBASE;

namespace DLAM
{
    public class MyPuzzleData
    {
        public List<PhotoData> datas;
    }
    
    public class MyPuzzlePresenter : IPresenter<CoreDataPresenter>
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
    }
}