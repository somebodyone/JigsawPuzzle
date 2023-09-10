﻿using UnityEngine;
using System;
using System.Collections;
using Object = UnityEngine.Object;

namespace DTT.Tweening
{
    /// <summary>
    /// Entry class for creating tweens that can be used to animate objects.
    /// </summary>
    public static class DTTween
    {
        /// <summary>
        /// Worker object that is used for starting and stopping coroutines.
        /// </summary>
        private static DTTweenWorker _worker;

        /// <summary>
        /// Initializes with a worker object.
        /// </summary>
        static DTTween()
        {
            _worker = new GameObject("DTTween Worker").AddComponent<DTTweenWorker>();
            Object.DontDestroyOnLoad(_worker);
        }

        /// <summary>
        /// Starts a tween.
        /// </summary>
        /// <param name="from">Starting value.</param>
        /// <param name="to">End value.</param>
        /// <param name="time">Duration of the tween.</param>
        /// <param name="delay">Amount of time before the tween starts.</param>
        /// <param name="easing">What type of ease should be used.</param>
        /// <param name="onValueChanged">Callback per frame.</param>
        /// <param name="onComplete">Callback when tween has completed.</param>
        /// <returns>Coroutine of the tween.</returns>
        public static Coroutine Value
            (float from, 
            float to, 
            float time, 
            float delay = 0, 
            Easing easing = Easing.EASE_IN_OUT_SINE, 
            Action<float> onValueChanged = null, 
            Action onComplete = null)
        {
            return _worker.StartCoroutine(ValueC(from, to, time, delay, easing, onValueChanged, onComplete));
        }
        
        /// <summary>
        /// Starts a tween.
        /// </summary>
        /// <param name="from">Starting value.</param>
        /// <param name="to">End value.</param>
        /// <param name="time">Duration of the tween.</param>
        /// <param name="easing">What type of ease should be used.</param>
        /// <param name="onValueChanged">Callback per frame.</param>
        /// <param name="onComplete">Callback when tween has completed.</param>
        /// <returns>Coroutine of the tween.</returns>
        public static Coroutine Value
            (float from, 
            float to, 
            float time, 
            Easing easing = Easing.EASE_IN_OUT_SINE, 
            Action<float> onValueChanged = null, 
            Action onComplete = null)
        {
            return _worker.StartCoroutine(ValueC(from, to, time, 0, easing, onValueChanged, onComplete));
        }
        
        public static Coroutine TwoValue
        (float from1, 
            float from2,
            float to1, 
            float to2, 
            float time, 
            Easing easing = Easing.EASE_IN_OUT_SINE, 
            Action<float,float> onValueChanged = null,
            Action onComplete = null)
        {
            
            return _worker.StartCoroutine(TwoValueC(from1, from2,to1,to2, time, 0, easing, onValueChanged, onComplete));
            
        }
        
        
        /// <summary>
        /// Starts a tween.
        /// </summary>
        /// <param name="from">Starting value.</param>
        /// <param name="to">End value.</param>
        /// <param name="time">Duration of the tween.</param>
        /// <param name="delay">Amount of time before the tween starts.</param>
        /// <param name="easing">What type of ease should be used.</param>
        /// <param name="onValueChanged">Callback per frame.</param>
        /// <param name="onComplete">Callback when tween has completed.</param>
        /// <returns>Startable coroutine.</returns>
        public static IEnumerator ValueC
            (float from, 
            float to, 
            float time, 
            float delay = 0, 
            Easing easing = Easing.EASE_IN_OUT_SINE, 
            Action<float> onValueChanged = null, 
            Action onComplete = null)
        {
            yield return new WaitForSeconds(delay);
            float value = from;
            float startTime = Time.time;
            while (startTime + time > Time.time)
            {
                float normalized = (Time.time - startTime) / time;

                value = Mathf.LerpUnclamped(from, to, TweenDefinitions.TweenMap[easing](normalized));

                onValueChanged?.Invoke(value);

                yield return null;
            }
            yield return null;
            onComplete?.Invoke();
        }
        
        
        public static IEnumerator TwoValueC
        (float from1, 
            float from2, 
            float to1, 
            float to2, 
            float time, 
            float delay = 0, 
            Easing easing = Easing.EASE_IN_OUT_SINE, 
            Action<float,float> onValueChanged = null,
            Action onComplete = null)
        {
            yield return new WaitForSeconds(delay);
            float value1 = from1;
            float value2 = from2;
            float startTime = Time.time;
            while (startTime + time > Time.time)
            {
                float normalized = (Time.time - startTime) / time;

                value1 = Mathf.LerpUnclamped(from1, to1, TweenDefinitions.TweenMap[easing](normalized));

                value2 = Mathf.LerpUnclamped(from2, to2, TweenDefinitions.TweenMap[easing](normalized));
                onValueChanged?.Invoke(value1,value2);

                yield return null;
            }
            yield return null;
            onComplete?.Invoke();
        }
        
        
    }
}