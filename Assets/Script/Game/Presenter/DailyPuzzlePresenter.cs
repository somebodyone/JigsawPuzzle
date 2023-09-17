using System;
using System.Collections.Generic;
using DLBASE;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DLAM
{
    [Serializable]
    public class DailyPuzzleData
    {
        public List<PhotoData> datas;
    }
    
    public class DailyPuzzlePresenter: IPresenter<DailyPuzzlePresenter>
    {
        private DLOpition<DailyPuzzleData> _opition;
        private DailyPuzzleData _data=>_opition.data;
        
        public override void OnInit()
        {
            _opition = DLDataManager.GetOpition<DailyPuzzleData>();
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
            if (_data.datas == null)
            {
                _data.datas = new List<PhotoData>();
                for (int i = 1; i < 3; i++)
                {
                    int targetid = Random.Range(0, list.Count);
                    list.RemoveAt(targetid);
                    PhotoData itemdata = new PhotoData();
                    itemdata.type = 1000;
                    itemdata.id = targetid;
                    itemdata.photoname = "dailypuzzle";
                    itemdata.reward = new[] { 1000, 2000, 3000 };
                    itemdata.daying = false;
                    itemdata.dayily = DateTime.Now.Day-i;
                    itemdata.month = DateTime.Now.Month;
                    _data.datas.Add(itemdata);
                }
            }
            else
            {
                for (int i = 0; i < _data.datas.Count; i++)
                {
                    _data.datas[i].daying = false;
                    _data.datas[i].dayily++;
                }
            }
            
            int id = Random.Range(0, list.Count);
            list.RemoveAt(id);
            PhotoData data = new PhotoData();
            data.type = 1000;
            data.id = id;
            data.photoname = "dailypuzzle";
            data.reward = new[] { 1000, 2000, 3000 };
            data.daying = true;
            data.dayily = DateTime.Now.Day;
            data.month = DateTime.Now.Month;
            _data.datas.Add(data);
            _opition.SetDirty(true);
        }

        public DailyPuzzleData GetData()
        {
            return _data;
        }
    }
}