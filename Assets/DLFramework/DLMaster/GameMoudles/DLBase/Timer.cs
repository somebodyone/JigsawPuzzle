using System;
using System.Collections.Generic;

namespace DLBASE
{
    public class TimerData
    {
        public float time;
        public Action callback;
    }
    public class Timer
    {
        private Dictionary<string, TimerData> _dics = new Dictionary<string, TimerData>();

        public void UpdateTimer(float time)
        {
            foreach (var item in _dics.Keys)
            {
                _dics[item].time -= time;
                if (_dics[item].time <= 0)
                {
                    _dics[item].callback?.Invoke();
                    _dics.Remove(item);
                    return;
                }
            }     
        }
        
        public void SetTimer(string key, float time,Action callback)
        {
            TimerData data = new TimerData();
            data.time = time;
            data.callback = callback;
            _dics.Add(key,data);
        }
    }
}