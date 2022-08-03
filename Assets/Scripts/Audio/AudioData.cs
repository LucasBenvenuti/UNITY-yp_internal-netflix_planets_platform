using UnityEngine;

namespace PWE.Audio
{
    [CreateAssetMenu(fileName = "AudioData", menuName = "Data/Audio", order = 1)]
    public class AudioData : ScriptableObject
    {
        public AudioClip AudioClip;
        [Range(0, 1)]
        public float Volume = 1;
        [Range(-3, 3)]
        public float Pitch = 1;
        public bool Loop;
    }
}