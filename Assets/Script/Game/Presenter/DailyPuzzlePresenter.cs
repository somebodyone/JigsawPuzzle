using System;
using System.Collections.Generic;
using DLBASE;
using UnityEngine;
using Random = UnityEngine.Random;

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
            DLPlayer.lisioner.OnNewDay(this, () =>
            {
                SetData();
            });
        }

        public void SetData()
        {
            List<int> list = new List<int>();
            for (int i = 0; i < 28; i++)
            {
                list.Add(i);
            }
            for (int i = 0; i < _data.datas.Count; i++)
            {
                _data.datas[i].daying = false;
                _data.datas[i].dayily++;
            }
            int id = Random.Range(0, list.Count);
            list.RemoveAt(id);
            PhotoData data = new PhotoData();
            data.type = 1000;
            data.id = id;
            data.photoname = "dailypuzzle";
            data.size = new Vector2Int(4, 6);
            data.reward = new[] { 1000, 2000, 3000 };
            data.daying = true;
            data.dayily = DateTime.Now.Day;
            data.month = DateTime.Now.Month;
            data.year = DateTime.Now.Year;
            _data.datas.Add(data);
            _opition.SetDirty(true);
        }

        public DailyPuzzleData GetData()
        {
            return _data;
        }
    }
}