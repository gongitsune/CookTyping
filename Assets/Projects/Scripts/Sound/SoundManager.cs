using Cysharp.Threading.Tasks;
using Projects.Scripts.UI;
using Projects.Scripts.Utils;
using UnityEngine;
using UnityEngine.Audio;

namespace Projects.Scripts.Sound
{
    internal sealed class SoundManager : SingletonMonoBehavior<SoundManager>, IRootSceneInitializer
    {
        [SerializeField] private AudioGlobalConfig config;
        private BgmPlayer _bgmPlayer; // BGMの再生を管理するクラス
        private AudioMixerGroup _masterMixerGroup; // マスターミキサー
        private SePlayer _sePlayer; // 効果音の再生を管理するクラス

        public AudioGlobalConfig Config => config;

        public float BgmVolume
        {
            get => _bgmPlayer.Volume;
            set => _bgmPlayer.Volume = value;
        }

        public float SeVolume
        {
            get => _sePlayer.Volume;
            set => _sePlayer.Volume = value;
        }

        public float MasterVolume
        {
            get => _masterMixerGroup.audioMixer.GetFloat("MasterVolume", out var volume) ? volume : 0;
            set => _masterMixerGroup.audioMixer.SetFloat("MasterVolume", value);
        }

        /// <summary>
        ///     BGMのピッチ
        /// </summary>
        public float BgmPitch
        {
            get => _bgmPlayer.Pitch;
            set => _bgmPlayer.Pitch = value;
        }

        /// <summary>
        ///     効果音のピッチ
        /// </summary>
        public float SePitch
        {
            get => _sePlayer.Pitch;
            set => _sePlayer.Pitch = value;
        }

        /// <summary>
        ///     全体のピッチ
        /// </summary>
        public float MasterPitch
        {
            set => BgmPitch = SePitch = value;
        }

        public void Initialize()
        {
            // オーディオソースの生成
            var go = transform.gameObject;
            var bgmSources = new AudioSource[2];
            bgmSources[0] = go.AddComponent<AudioSource>();
            bgmSources[1] = go.AddComponent<AudioSource>();

            var seSource = go.AddComponent<AudioSource>();

            _masterMixerGroup = config.mixerSetting.masterMixerGroup;
            _bgmPlayer = new BgmPlayer(bgmSources, config, transform.GetCancellationTokenOnDestroy());
            _sePlayer = new SePlayer(seSource, config);

            DontDestroyOnLoad(gameObject);
        }

        /// <summary>
        ///     BGMを再生
        /// </summary>
        /// <param name="clip">再生するBGMのAudioClip</param>
        public void PlayBgm(AudioClip clip)
        {
            _bgmPlayer.PlayBgm(clip).Forget();
        }

        /// <summary>
        ///     SEを再生
        /// </summary>
        /// <param name="clip">再生するSEのAudioClip</param>
        /// <param name="volume">SEの音量</param>
        public void PlaySe(AudioClip clip, float volume = 1)
        {
            _sePlayer.PlaySe(clip, volume);
        }
    }
}