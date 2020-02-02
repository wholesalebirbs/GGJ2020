using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Buga
{
    [RequireComponent(typeof(AudioSource))]
    public class RandomAudioPlayer : MonoBehaviour
    {
        [SerializeField]
        List<AudioClip> clips = new List<AudioClip>();

        AudioSource source;

        protected virtual void Awake()
        {
            source = GetComponent<AudioSource>();
        }

        protected virtual void PlayRandomSound()
        {
            if (!source.isPlaying)
            {
                source.clip = clips[Random.Range(0, clips.Count)];
                source.Play();
            }
        }
    }

}