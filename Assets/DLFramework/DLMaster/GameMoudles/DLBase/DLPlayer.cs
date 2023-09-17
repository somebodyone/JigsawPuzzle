using System;
using UnityEngine;

namespace DLBASE
{
    public class DLPlayer
    {
        public class PlayerData
        {
            public long day = -1;
        }
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

        private static DLOpition<PlayerData> _opition;
        private static PlayerData _data =>_opition.data;
        private static DLBase _base;
        public static Timer timer;
        public static Lisioner lisioner = new Lisioner();

        public static void CheckInit()
        {
            _opition = DLDataManager.GetOpition<PlayerData>();
            GameObject go = new GameObject();
            timer = new Timer();
            go.name = "DLPlayer";
            go.AddComponent<DLBase>();
            _base = go.GetComponent<DLBase>();
            OnNewDay();
        }

        private static void OnNewDay()
        {
            int todayMidnight = DateTime.Now.Day;
            if (_data.day != todayMidnight)
            {
                lisioner.Emit(EventType.OnNewDay);
                _data.day = todayMidnight;
            }
            _opition.SetDirty(true);
        }
        
    }
}