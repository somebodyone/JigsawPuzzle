using System;
using System.Collections.Generic;
using DLBASE;
using Random = UnityEngine.Random;

namespace DLAM
{
    [Serializable]
    public class PhotoData
    {
        public bool islock = true;
        public bool collect;
        public int type;
        public int id;
        public string photoname;
        public int[] reward;
        public int lockgold;//解锁所需金币
        public int month;//开始时间
        public int dayily;//日期
        public int selecetId;
        public bool daying = false;
        public int[] times = { -1, -1, -1 };
    }
    
    public class GameData
    {
        public PhotoData recommenddata;
        public List<PhotoData> datas;
    }
    
    public class GamePresenter : IPresenter<GamePresenter>
    {
        public enum EventType
        {
            UpdatePhoto
        }
        public class Lisioner : DLLisioner
        {
            public void UpdatePhoto(object key, Action callback)
            {
                Add(key, EventType.UpdatePhoto, callback);
            }
        }
        private DLOpition<GameData> _opition;
        private GameData _data=>_opition.data;
        public Lisioner lisioner = new Lisioner();
        public override void OnInit()
        {
            _opition = DLDataManager.GetOpition<GameData>();
            _data.recommenddata = new PhotoData();
            _data.datas = new List<PhotoData>();
            for (int i = 0; i < 16; i++)
            {
                PhotoData data = new PhotoData();
                data.type = 7;
                data.id = i;
                if (i < 3)
                {
                    data.islock = false;
                }
                data.photoname = "gril";
                data.reward = new[] { 100, 200, 300 };
                _data.datas.Add(data);
            }
            for (int i = 0; i < 10; i++)
            {
                PhotoData data = new PhotoData();
                data.id = i;
                data.type = 3;
                if (i < 3)
                {
                    data.islock = false;
                }
                data.photoname = "animal";
                data.reward = new[] { 100, 200, 300 };
                _data.datas.Add(data);
            }
            for (int i = 0; i < 9; i++)
            {
                PhotoData data = new PhotoData();
                data.id = i;
                data.type = 6;
                if (i < 3)
                {
                    data.islock = false;
                }
                data.photoname = "cartoon";
                data.reward = new[] { 100, 200, 300 };
                _data.datas.Add(data);
            }
            for (int i = 0; i < 20; i++)
            {
                PhotoData data = new PhotoData();
                data.id = i;
                data.type = 4;
                if (i < 3)
                {
                    data.islock = false;
                }
                data.photoname = "flowers";
                data.reward = new[] { 100, 200, 300 };
                _data.datas.Add(data);
            }
            for (int i = 0; i < 5; i++)
            {
                PhotoData data = new PhotoData();
                data.id = i;
                data.type = 2;
                if (i < 3)
                {
                    data.islock = false;
                }
                data.photoname = "landmark";
                data.reward = new[] { 100, 200, 300 };
                _data.datas.Add(data);
            }
            for (int i = 0; i < 6; i++)
            {
                PhotoData data = new PhotoData();
                data.id = i;
                data.type = 0;
                if (i < 3)
                {
                    data.islock = false;
                }
                data.photoname = "mystery";
                data.reward = new[] { 100, 200, 300 };
                _data.datas.Add(data);
            }
            for (int i = 0; i < 38; i++)
            {
                PhotoData data = new PhotoData();
                data.id = i;
                data.type = 1;
                if (i < 3)
                {
                    data.islock = false;
                }
                data.photoname = "nature";
                data.reward = new[] { 100, 200, 300 };
                _data.datas.Add(data);
            }
            for (int i = 0; i < 12; i++)
            {
                PhotoData data = new PhotoData();
                data.id = i;
                data.type = 5;
                if (i < 3)
                {
                    data.islock = false;
                }
                data.photoname = "pet";
                data.reward = new[] { 100, 200, 300 };
                _data.datas.Add(data);
            }
            _data.recommenddata = _data.datas[Random.Range(0, 7)];
        }
        
        public List<PhotoData> GetData(int type)
        {
            List<PhotoData> datas = new List<PhotoData>();
            if (type==0)
            {
                return _data.datas;
            }
            if (type == 1)
            {
                for (int i = 0; i < _data.datas.Count; i++)
                {
                    if (_data.datas[i].collect)
                    {
                        datas.Add(_data.datas[i]);
                    }
                }
                return datas;
            }
            for (int i = 0; i < _data.datas.Count; i++)
            {
                if (_data.datas[i].type == type-2)
                {
                    datas.Add(_data.datas[i]);
                }
            }
            return datas;
        }

        public void CollectPhoto(PhotoData data)
        {
            data.collect =!data.collect;
            _opition.SetDirty(true);
            lisioner.Emit(EventType.UpdatePhoto);
        }

        public void UnLockPhoto(PhotoData data)
        {
            for (int i = 0; i < _data.datas.Count; i++)
            {
                if (data == _data.datas[i])
                {
                    _data.datas[i].islock = false;
                }
            }
            _opition.SetDirty(true);
            lisioner.Emit(EventType.UpdatePhoto);
        }

        public GameData GameData => _data;
    }
}