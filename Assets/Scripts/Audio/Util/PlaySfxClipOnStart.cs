using UnityEngine;

namespace PWE.Audio
{
    public class PlaySfxClipOnStart : MonoBehaviour
    {
        public enum DestroyMode
        {
            GAME_OBJECT,
            MONO_BEHAVIOUR
        }

        public AudioManager AudioManager;
        public AudioData MusicAudioData;
        public DestroyMode DestructionMode;

        private void Start()
        {
            AudioManager.PlaySfxClip(MusicAudioData);
            switch (DestructionMode)
            {
                case DestroyMode.GAME_OBJECT:
                    Destroy(gameObject);
                    break;
                case DestroyMode.MONO_BEHAVIOUR:
                    Destroy(this);
                    break;
            }
        }
    }
}