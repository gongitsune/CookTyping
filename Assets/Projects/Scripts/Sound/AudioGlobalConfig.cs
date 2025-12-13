using System;
using UnityEngine;
using UnityEngine.Audio;

namespace Projects.Scripts.Sound
{
    [CreateAssetMenu(fileName = "AudioGlobalConfig", menuName = "Configs/Audio Global Config", order = 0)]
    internal class AudioGlobalConfig : ScriptableObject
    {
        [Header("Audio Mixerの設定")] public MixerSetting mixerSetting;

        [Header("再生設定")] [Header("BGMのフェード間隔")]
        public float fadeDuration = 2f;

        [Header("その他の設定")] [Header("使用していないと判断されるまでの秒数")]
        public float garbageDuration = 120;


        [Header("Clips")] [Header("BGM")] public AudioClip titleBgmClip;
        public AudioClip levelSelectBgmClip;
        public AudioClip gameMainBgmClip;
        public AudioClip resultBgmClip;

        [Header("SE")] public AudioClip buttonClickSeClip;
        public AudioClip keyboardTypingSeClip;

        [Serializable]
        public class MixerSetting
        {
            [Header("MasterのAudioMixerGroup")] public AudioMixerGroup masterMixerGroup;
            [Header("BGMのAudioMixerGroup")] public AudioMixerGroup bgmMixerGroup;
            [Header("SEのAudioMixerGroup")] public AudioMixerGroup seMixerGroup;
        }
    }
}