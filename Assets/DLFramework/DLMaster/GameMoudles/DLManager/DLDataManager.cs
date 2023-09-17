using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace DLBASE
{
    public class SMetaData
    {
        public List<string> list;
    }
    
    public class DLDataManager:DLSingleton<DLDataManager>
    {
        public static SMetaData _data = new SMetaData();
        private static string _key = "__keys";

        public static DLOpition<T> GetOpition<T>()where T:new ()
        {
            DLOpition<T> opition = new DLOpition<T>();
            bool issave = false;
            opition.data = new T();

            if (PlayerPrefs.HasKey(_key))
            {
                string str = PlayerPrefs.GetString(_key);
                _data = JsonConvert.DeserializeObject<SMetaData>(str);
            }
            if (_data.list == null)
            {
                _data.list = new List<string>();
            }
            foreach (var item in _data.list)
            {
                if (item == opition.data.GetType().ToString())
                {
                    issave = true;
                    string key = PlayerPrefs.GetString(item);
                    opition.data = JsonConvert.DeserializeObject<T>(key);
                    if (opition.data == null)
                    {
                        opition.data = new T();
                    }
                }
            }
            if (!issave)
            {
                _data.list.Add(opition.data.GetType().ToString());
            }
            string json = JsonConvert.SerializeObject(_data);
            PlayerPrefs.SetString(_key,json);
            return opition;
        }
    }
}