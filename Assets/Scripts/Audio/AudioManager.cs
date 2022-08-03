using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using SF = UnityEngine.SerializeField;

namespace PWE.Audio
{
    [CreateAssetMenu(fileName = "AudioManager", menuName = "Managers/Audio", order = 1)]
    public class AudioManager : ScriptableObject
    {
        [Header("Audio Mixer")]
        [SF] private AudioMixer _masterAudioMixer;
        [SF] private AudioMixerGroup _masterMixerGroup;
        [SF] private AudioMixerGroup _musicMixerGroup;
        [SF] private AudioMixerGroup _sfxMixerGroup;

        [Header("Volume")]
        [SF] private string _masterVolumeParameterName = "MasterVolume";
        [SF] private string _musicVolumeParameterName = "MusicVolume";
        [SF] private string _sfxVolumeParameterName = "SfxVolume";

        [Space(10)]
        [SF, Range(0.001f, 1)] private float _masterVolumeSlider = 1;
        [SF, Range(0.001f, 1)] private float _musicVolumeSlider = 1;
        [SF, Range(0.001f, 1)] private float _sfxVolumeSlider = 1;

        [Header("Logging")]
        public bool EnableLogs;

        // TODO: Separar canais em ScriptableObjects utilizando-os como keys
        private List<AudioSource> _allInstantiatedSources = new List<AudioSource>();
        private List<AudioSource> _instantiatedGenericSources = new List<AudioSource>();
        private List<AudioSource> _instantiatedMusicSources = new List<AudioSource>();
        private List<AudioSource> _instantiatedSfxSources = new List<AudioSource>();
        private Transform _spawnedGenericAudioSourcesContainer;
        private Transform _spawnedMusicAudioSourcesContainer;
        private Transform _spawnedSfxAudioSourcesContainer;

        #region Public Properties
        public AudioMixer MasterAudioMixer => _masterAudioMixer;
        public AudioMixerGroup MasterMixerGroup => _masterMixerGroup;
        public AudioMixerGroup MusicMixerGroup => _musicMixerGroup;
        public AudioMixerGroup SfxMixerGroup => _sfxMixerGroup;

        public IReadOnlyList<AudioSource> InstantiatedGenericSources => _instantiatedGenericSources;
        public IReadOnlyList<AudioSource> InstantiatedMusicSources => _instantiatedMusicSources;
        public IReadOnlyList<AudioSource> InstantiatedSfxSources => _instantiatedSfxSources;

        /* TODO deixar o retorno dos getters de volume mais intuitivos (talvez normalizar o valor para 0 e 1)
        /* necessitaria converter de escala logaritimica*/

        public float MasterVolume
        {
            get { return GetMasterAudioMixerFloat(_masterVolumeParameterName); }
            set
            {
                SetMasterAudioMixerFloat(_masterVolumeParameterName,
                                         ConvertToVolumeRange(value));
            }
        }

        public float MusicVolume
        {
            get { return GetMasterAudioMixerFloat(_musicVolumeParameterName); }
            set
            {
                SetMasterAudioMixerFloat(_musicVolumeParameterName,
                                         ConvertToVolumeRange(value));
            }
        }

        public float SfxVolume
        {
            get { return GetMasterAudioMixerFloat(_sfxVolumeParameterName); }
            set
            {
                SetMasterAudioMixerFloat(_sfxVolumeParameterName,
                                         ConvertToVolumeRange(value));
            }
        }

        #endregion

        #region Unity Messages

        private void OnValidate()
        {
            MasterVolume = _masterVolumeSlider;
            MusicVolume = _musicVolumeSlider;
            SfxVolume = _sfxVolumeSlider;
        }

        #endregion

        #region Public Methods

        public static float ConvertToVolumeRange(float value)
        {
            value = Mathf.Clamp(value, 0.001f, 1);
            return Mathf.Log(value) * 20;
        }

        public void ToggleMasterMute()
        {
            MasterVolume = MasterVolume > -138f ? 0 : 1;
        }

        public void ToggleMasterMute(bool mute)
        {
            MasterVolume = mute ? 0 : 1;
        }

        public void ToggleMusicMute()
        {
            MusicVolume = MusicVolume > -138f ? 0 : 1;
        }

        public void ToggleMusicMute(bool mute)
        {
            MusicVolume = mute ? 0 : 1;
        }

        public void ToggleMasterPause(bool pause)
        {
            foreach (var src in _allInstantiatedSources)
            {
                if (pause)
                {
                    src.Pause();
                }
                else
                {
                    src.Play();
                }
            }
        }

        public float GetMasterAudioMixerFloat(string parameterName)
        {
            float value = 0;
            if (MasterAudioMixer)
            {
                if (!MasterAudioMixer.GetFloat(parameterName, out value))
                {
                    Debug.LogError("AudioManager -> Parameter name " +
                                   "\"" + parameterName + "\" does not" +
                                   "correspond to a valid parameter");
                }
            }
            else
            {
                Debug.LogError("AudioManager -> MasterAudioMixer reference is missing");
            }
            return value;
        }

        public void SetMasterAudioMixerFloat(string parameterName,
                                             float value)
        {
            if (MasterAudioMixer)
            {
                if (!MasterAudioMixer.GetFloat(parameterName, out float _))
                {
                    Debug.LogError("AudioManager -> Parameter name " +
                                   "\"" + parameterName + "\" does not" +
                                   "correspond to a valid parameter");
                }
                else if (!MasterAudioMixer.SetFloat(parameterName, value))
                {
                    Debug.LogWarning("AudioManager -> Parameter cannot be set " +
                                     "while snapshot is being edited");
                }

            }
            else
            {
                Debug.LogError("AudioManager -> MasterAudioMixer reference is missing");
            }
        }

        public AudioSource PlayAudioClip(AudioData audioData)
        {
            return PlayAudioClip(audioData, Vector3.zero);
        }

        public AudioSource PlayAudioClip(AudioData audioData,
                                         Vector3 position,
                                         Transform parent = null,
                                         float spacialBlend = 1)
        {
            if (!parent)
            {
                parent = SetupGenericAudioContainer();
            }
            AudioSource audioSrc = SpawnAudioSource(audioData,
                                                    position,
                                                    parent,
                                                    MasterMixerGroup,
                                                    spacialBlend);
            _instantiatedGenericSources.Add(audioSrc);
            audioSrc.Play();
            return audioSrc;
        }

        public AudioSource PlayMusicClip(AudioData audioData,
                                         Transform parent = null,
                                         bool playAsUnique = true)
        {
            if (!parent)
            {
                parent = SetupMusicContainer();
            }
            AudioSource audioSrc = SpawnAudioSource(audioData,
                                                    Vector3.zero,
                                                    parent,
                                                    MusicMixerGroup);
            if (playAsUnique)
            {
                _instantiatedMusicSources.ForEach(ms => Destroy(ms.gameObject));
            }
            _instantiatedMusicSources.Add(audioSrc);
            audioSrc.Play();
            return audioSrc;
        }

        public AudioSource PlaySfxClip(AudioData audioData)
        {
            return PlaySfxClip(audioData, Vector3.zero);
        }

        public AudioSource PlaySfxClip(AudioData audioData,
                                       Vector3 position,
                                       Transform parent = null,
                                       float spacialBlend = 1)
        {
            if (!parent)
            {
                parent = SetupSfxContainer();
            }
            AudioSource audioSrc = SpawnAudioSource(audioData,
                                                    position,
                                                    parent,
                                                    SfxMixerGroup,
                                                    spacialBlend);
            _instantiatedSfxSources.Add(audioSrc);
            audioSrc.Play();
            return audioSrc;
        }

        #endregion

        #region Private Methods

        private AudioSource SpawnAudioSource(AudioData audioData,
                                             Vector3 position,
                                             Transform parent,
                                             AudioMixerGroup mixerGroup,
                                             float spacialBlend = 1)
        {
            AudioSource audioSrc = SpawnAudioSource(audioData, mixerGroup);
            audioSrc.transform.position = position;
            audioSrc.transform.parent = parent;
            audioSrc.spatialBlend = spacialBlend;
            audioSrc.Play();
            return audioSrc;
        }

        private AudioSource SpawnAudioSource(AudioData audioData,
                                             AudioMixerGroup mixerGroup)
        {
            var audioSrcGO = new GameObject(audioData.name);
            AudioSource audioSrc = audioSrcGO.AddComponent<AudioSource>();
            audioSrc.clip = audioData.AudioClip;
            audioSrc.volume = audioData.Volume;
            audioSrc.pitch = audioData.Pitch;
            audioSrc.loop = audioData.Loop;
            audioSrc.outputAudioMixerGroup = mixerGroup;
            var audioLS = audioSrcGO.AddComponent<AudioSourceLifeSpan>();
            audioLS.Destroyed += OnAudioSourceDestroyed;
            _allInstantiatedSources.Add(audioSrc);
            return audioSrc;
        }

        private void OnAudioSourceDestroyed(AudioSource audioSrc)
        {
            _allInstantiatedSources.Remove(audioSrc);
            _instantiatedGenericSources.Remove(audioSrc);
            _instantiatedMusicSources.Remove(audioSrc);
            _instantiatedSfxSources.Remove(audioSrc);
            if (EnableLogs)
            {
                Debug.Log("AudioManager -> " + audioSrc.name + " " +
                          "has been destroyed " +
                          "and removed from all lists");
            }
        }
        private Transform SetupGenericAudioContainer()
        {
            if (!_spawnedGenericAudioSourcesContainer)
            {
                _spawnedGenericAudioSourcesContainer =
                    new GameObject("SpawnedGenericAudioSources").transform;
            }
            return _spawnedGenericAudioSourcesContainer;
        }

        private Transform SetupSfxContainer()
        {
            if (!_spawnedSfxAudioSourcesContainer)
            {
                _spawnedSfxAudioSourcesContainer =
                    new GameObject("SpawnedSfxSources").transform;
            }
            return _spawnedSfxAudioSourcesContainer;
        }

        private Transform SetupMusicContainer()
        {
            if (!_spawnedMusicAudioSourcesContainer)
            {
                _spawnedMusicAudioSourcesContainer =
                    new GameObject("SpawnedMusicSources").transform;
            }
            return _spawnedMusicAudioSourcesContainer;
        }

        #endregion
    }
}