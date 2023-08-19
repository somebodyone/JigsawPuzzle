using System;
using System.Collections.Generic;
using UnityEngine;

namespace DLBASE
{
    public class DLLisioner
    {
        private Dictionary<Enum, Dictionary<object, List<Action>>> _callback =
            new Dictionary<Enum, Dictionary<object, List<Action>>>();
        public void Emit(Enum type)
        {
            if (_callback.ContainsKey(type))
            {
                foreach (var item in _callback.Keys)
                {
                    if (type.Equals(item))
                    {
                        foreach (var dickey in _callback[type].Keys)
                        {
                            for (int i = 0; i < _callback[type][dickey].Count; i++)
                            {
                                _callback[type][dickey][i]?.Invoke();
                            }
                        }
                    }
                }
            }
        }
        public void Add(object key,Enum eventtype,Action callback)
        {
            if (!_callback.ContainsKey(eventtype))
            {
                Dictionary<object, List<Action>> dic = new Dictionary<object, List<Action>>();
                List<Action> list = new List<Action>();
                list.Add(callback);
                dic.Add(key,list);
                _callback.Add(eventtype,dic);
            }
            else
            {
                if (!_callback[eventtype].ContainsKey(key))
                {
                    List<Action> list = new List<Action>();
                    list.Add(callback);
                    _callback[eventtype].Add(key,list);
                }
                else
                {
                    foreach (var item in _callback[eventtype].Keys)
                    {
                        if (item == key)
                        {
                            _callback[eventtype][item].Add(callback);
                        }
                    }
                }
            }
        }
        public void Remove(object key)
        {
            foreach (var items in _callback.Keys)
            {
                if (_callback[items].ContainsKey(key))
                {
                    _callback[items].Remove(key);
                }
            }
        }
    }
}