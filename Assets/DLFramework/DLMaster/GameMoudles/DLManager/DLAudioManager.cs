using System.Collections;
using System.Collections.Generic;
using DLBASE;
using UnityEngine;

namespace DLAM
{
    public class DLAudioManager : DLSingleton<DLAudioManager>
    {
        private AudioSource _source;
        private AudioSource _bgsource;

        public void PlayBGAudio(string url)
        {
            if (_source == null)
            {
                GameObject go = new GameObject();
                go.name = "BGAudioSource";
                _bgsource = go.AddComponent<AudioSource>();
            }
            _bgsource.clip = Resources.Load<AudioClip>(url);
            _bgsource.Play();
            _bgsource.loop = true;
        }
        
        public void PlayAudio(string url)
        {
            if (_source == null)
            {
                GameObject go = new GameObject();
                go.name = "AudioSource";
                _source = go.AddComponent<AudioSource>();
            }
            _source.PlayOneShot(Resources.Load<AudioClip>(url));
        }
    }
}
