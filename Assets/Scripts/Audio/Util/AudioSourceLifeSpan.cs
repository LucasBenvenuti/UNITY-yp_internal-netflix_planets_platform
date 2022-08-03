using System;
using UnityEngine;

namespace PWE.Audio
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioSourceLifeSpan : MonoBehaviour
    {
        public event Action<AudioSource> Destroyed;
        private AudioSource _source;
        private bool _hasStart;

        public bool IsDone => !_source.loop && _hasStart && _source.time <= 0.0f;

        private void Awake()
        {
            _source = GetComponent<AudioSource>();
        }

        private void Update()
        {
            _hasStart |= _source.time > 0.0f;

            if (IsDone)
                Destroy(gameObject);
        }

        private void OnDestroy()
        {
            Destroyed.Invoke(_source);
        }
    }
}