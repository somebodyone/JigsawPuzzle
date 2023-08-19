using System;
using UnityEngine;

namespace DLBASE
{
    public class DLPlayer
    {
        public enum EventType
        {
            OnNewDay,
            FiexdUpdate,
            Update,
            SecondTrick
        }

        public class Lisioner : DLLisioner
        {
            public void OnNewDay(object key, Action callback)
            {
                Add(key, EventType.OnNewDay, callback);
            }

            public void FiexdUpdate(object key, Action callback)
            {
                Add(key, EventType.FiexdUpdate, callback);
            }

            public void Update(object key, Action callback)
            {
                Add(key, EventType.Update, callback);
            }

            public void SecondTrick(object key, Action callback)
            {
                Add(key, EventType.SecondTrick, callback);
            }
        }

        private static DLBase _base;
        public static Timer timer;
        public static Lisioner lisioner = new Lisioner();

        public static void CheckInit()
        {
            GameObject go = new GameObject();
            timer = new Timer();
            go.name = "DLPlayer";
            go.AddComponent<DLBase>();
            _base = go.GetComponent<DLBase>();
        }
    }
}