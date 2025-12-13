using UnityEngine;

namespace Projects.Scripts.Sound
{
    /// <summary>
    ///     効果音の再生を制御
    /// </summary>
    internal class SePlayer
    {
        private readonly AudioGlobalConfig _config; // 設定
        private readonly AudioSource _source; // 効果音を再生するAudioSource

        public SePlayer(AudioSource source, AudioGlobalConfig config)
        {
            _config = config;
            _source = source;

            _source.outputAudioMixerGroup = config.mixerSetting.seMixerGroup;
        }

        /// <summary>
        ///     SEの音量
        /// </summary>
        public float Volume
        {
            get
            {
                _config.mixerSetting.seMixerGroup.audioMixer.GetFloat("SeVolume", out var volume);
                return volume;
            }
            set => _config.mixerSetting.seMixerGroup.audioMixer.SetFloat("SeVolume", value);
        }

        /// <summary>
        ///     SEを再生するAudioSourceのピッチ
        /// </summary>
        public float Pitch
        {
            get => _source.pitch;
            set => _source.pitch = value;
        }

        /// <summary>
        ///     効果音を再生
        /// </summary>
        /// <param name="clip">再生するクリップ</param>
        /// <param name="volume">再生する効果音の音量</param>
        public void PlaySe(AudioClip clip, float volume = 1)
        {
            _source.PlayOneShot(clip, volume);
        }
    }
}