using System.Collections.Generic;
using DLBASE;
using UnityEngine;

namespace DLAM
{
    public enum PhotoType
    {
        Mystery,
        Nature,
        Landmark,
        Animal,
        Flowers,
        Pet,
        Inkpainting,
        Cartoon,
        Gril
    }
    
    public class PhotoData
    {
        public bool islock = true;
        public bool collect;
        public int type;
        public int id;
        public string photoname;
        public Vector2Int size;
        public int[] reward;
        public int lockgold;//解锁所需金币
        public int month;//开始时间
        public int time;//持续时间
        public int dayily;//日期
        public int year;//日期
        public int selecetId;
        public bool daying = false;
    }
    
    public class GameData
    {
        public PhotoData recommenddata;
        public List<PhotoData> datas;
    }
    
    public class GamePresenter : IPresenter<GamePresenter>
    {
        private DLOpition<GameData> _opition;
        private GameData _data;
        
        public override void OnInit()
        {
            _opition = DLDataManager.GetOpition<GameData>();
            _data = _opition.data;
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
                data.size = new Vector2Int(4, 6);
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
                data.size = new Vector2Int(4, 6);
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
                data.size = new Vector2Int(4, 6);
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
                data.size = new Vector2Int(4, 6);
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
                data.size = new Vector2Int(4, 6);
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
                data.size = new Vector2Int(4, 6);
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
                data.size = new Vector2Int(4, 6);
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
                data.size = new Vector2Int(4, 6);
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
        }

        public GameData GameData => _data;
    }
}