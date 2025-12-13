using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Projects.Scripts.Sound
{
    /// <summary>
    ///     BGMの再生を管理するクラス
    /// </summary>
    internal class BgmPlayer
    {
        private readonly AudioGlobalConfig _config; // 設定
        private readonly CancellationToken _destroyToken; // 破棄時にキャンセルするトークン
        private readonly GameObject _sourceObj; // AudioSourceをアタッチするオブジェクト
        private readonly AudioSource[] _sources; // BGMを再生するAudioSource
        private CancellationTokenSource _tokenSource; // 再生中のBGMをキャンセルするトークン

        public BgmPlayer(AudioSource[] sources, AudioGlobalConfig config, CancellationToken destroyToken)
        {
            _destroyToken = destroyToken;
            _tokenSource = new CancellationTokenSource();
            _sources = sources;
            _config = config;

            foreach (var audioSource in sources)
            {
                // ループ再生をオンに
                audioSource.loop = true;
                audioSource.outputAudioMixerGroup = config.mixerSetting.bgmMixerGroup;
            }
        }

        /// <summary>
        ///     BGMを再生するAudioSourceのピッチ
        /// </summary>
        public float Pitch
        {
            get => _sources[0].pitch;
            set => _sources[0].pitch = _sources[1].pitch = value;
        }

        /// <summary>
        ///     BGMの音量
        /// </summary>
        public float Volume
        {
            get
            {
                _config.mixerSetting.bgmMixerGroup.audioMixer.GetFloat("BgmVolume", out var volume);
                return volume;
            }
            set => _config.mixerSetting.bgmMixerGroup.audioMixer.SetFloat("BgmVolume", value);
        }

        /// <summary>
        ///     BGMを再生
        /// </summary>
        /// <param name="clip">再生するAudioClip</param>
        public async UniTask PlayBgm(AudioClip clip)
        {
            _tokenSource.Dispose();
            _tokenSource = new CancellationTokenSource();

            var fadeInSourceIdx = _sources[0].volume <= 0.5f ? 0 : 1;
            var fadeOutSourceIdx = 1 - fadeInSourceIdx;

            var fadeInSource = _sources[fadeInSourceIdx];
            var fadeOutSource = _sources[fadeOutSourceIdx];

            // クリップ設定
            fadeInSource.clip = clip;
            // フェード
            await CrossFadeBGM(fadeInSource, fadeOutSource, _config.fadeDuration, 1);
            // 使用していないClipは解放
            if (fadeOutSource.clip != null) fadeOutSource.clip = null;
        }

        private async UniTask CrossFadeBGM(AudioSource fadeInSource, AudioSource fadeOutSource, float duration,
            float targetVolume)
        {
            // フェードインとフェードアウトを同時に行う
            var linkedSource = CancellationTokenSource.CreateLinkedTokenSource(_destroyToken, _tokenSource.Token);

            fadeInSource.volume = 0;
            fadeInSource.Play();
            var firstVol = fadeOutSource.volume;
            try
            {
                for (float time = 0; time < duration; time += Time.deltaTime)
                {
                    // 0から1になるように変換
                    var value = Mathf.Cos(time / duration * 2f);
                    fadeInSource.volume = (1 - value) / 2 * targetVolume;
                    fadeOutSource.volume = (1 + value) / 2 * firstVol;
                    await UniTask.DelayFrame(1, cancellationToken: linkedSource.Token);
                }
            }
            finally
            {
                if (Application.isPlaying)
                {
                    // フェードアウトが終わったら停止
                    fadeInSource.volume = targetVolume;
                    fadeOutSource.volume = 0;
                    fadeOutSource.Stop();
                }
            }
        }
    }
}