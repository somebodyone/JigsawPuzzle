using UnityEngine;

namespace DLBASE
{
    public class DLOpition<T>where T:new ()
    {
        private T target;

        public void SetDirty(bool dirty)
        {
            if (dirty)
            {
                string str = JsonUtility.ToJson(data);
                PlayerPrefs.SetString(target.GetType().ToString(),str);
            }
        }

        public T data
        {
            get
            {
                return target;
            }
            set
            {
                target = value;
            }
        }

        public bool HasKey()
        {
            return PlayerPrefs.HasKey(target.GetType().ToString());
        }
    }
}