using System.Collections.Generic;
using DLBASE;
using UnityEngine;

namespace DLAM
{
    public enum PhotoType
    {
        People,
        LandScape,
        Architecture,
        Animal,
        Cartoon
    }
    
    public class PhotoData
    {
        public bool islock;
        public PhotoType type;
        public int id;
        public string photoname;
        public Vector2Int size;
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
            for (int i = 0; i < 50; i++)
            {
                PhotoData data = new PhotoData();
                data.id = i;
                data.photoname = "people_";
                data.size = new Vector2Int(8, 12);
                _data.datas.Add(data);
            }
            _data.recommenddata = _data.datas[Random.Range(0, 7)];
        }

        public GameData GameData => _data;
    }
}