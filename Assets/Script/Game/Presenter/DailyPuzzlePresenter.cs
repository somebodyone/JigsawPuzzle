using System.Collections.Generic;
using DLBASE;

namespace DLAM
{
    public class DailyPuzzleData
    {
        public List<PhotoData> datas;
    }
    
    public class DailyPuzzlePresenter: IPresenter<DailyPuzzlePresenter>
    {
        private DLOpition<DailyPuzzleData> _opition;
        private DailyPuzzleData _data;
        
        public override void OnInit()
        {
            
        }
    }
}