using System.Collections.Generic;
using DLBASE;
using UnityEngine;

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
            _opition = DLDataManager.GetOpition<DailyPuzzleData>();
            _data = _opition.data;
            _data.datas = new List<PhotoData>();
        }

        public DailyPuzzleData GetData()
        {
            List<int> list = new List<int>();
            for (int i = 0; i < 28; i++)
            {
                list.Add(i);
            }

            int[] array = new int[4];
            for (int i = 0; i < array.Length; i++)
            {
                int id = Random.Range(0, list.Count);
                array[i] = list[id];
                list.RemoveAt(id);
                PhotoData data = new PhotoData();
                data.type = 1000;
                data.id = i;
                data.photoname = "dailypuzzle";
                data.size = new Vector2Int(4, 6);
                data.reward = new[] { 1000, 2000, 3000 };
                _data.datas.Add(data);
            }
            
            return _data;
        }
    }
}