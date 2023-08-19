using UnityEngine;

namespace DLBASE
{
    public class DLOpition<T>where T:new ()
    {
        private T target;
        private string _key = "__";

        public void SetDirty(bool dirty)
        {
            if (dirty)
            {
                string str = JsonUtility.ToJson(data);
                PlayerPrefs.SetString(_key+data.GetType(),str);
            }
        }

        public T data
        {
            get
            {
                if (PlayerPrefs.HasKey(_key+target.GetType()))
                {
                    string str = PlayerPrefs.GetString(_key + target.GetType());
                    return JsonUtility.FromJson<T>(str); 
                }
                return target;
            }
            set
            {
                target = value;
                SetDirty(true);
            }
        }
    }
}