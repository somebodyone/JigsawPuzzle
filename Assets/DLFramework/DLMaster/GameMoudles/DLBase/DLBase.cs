
using UnityEngine;

namespace DLBASE
{
    
    public class DLBase : MonoBehaviour
    {
        private float _speedcount;
        private float _time = 0;

        public void Awake()
        {
            _speedcount = 1.0f / Time.fixedDeltaTime;
        }


        public void OnNewDay()
        {
        
        }

        public void Update()
        {
            DLPlayer.lisioner.Emit(DLPlayer.EventType.Update);
            DLPlayer.timer.UpdateTimer(Time.deltaTime);
        }
        public void FixedUpdate()
        {
            _time ++;
            if (_time >= _speedcount)
            {
                DLPlayer.lisioner.Emit(DLPlayer.EventType.SecondTrick);
                _time = 0;
            }
            DLPlayer.lisioner.Emit(DLPlayer.EventType.FiexdUpdate);
        }
    }
}
