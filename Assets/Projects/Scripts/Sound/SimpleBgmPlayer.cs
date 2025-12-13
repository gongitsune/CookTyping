using UnityEngine;

namespace Projects.Scripts.Sound
{
    public class SimpleBgmPlayer : MonoBehaviour
    {
        [SerializeField] private AudioClip bgmClip;

        private void Start()
        {
            var soundManager = SoundManager.Instance;
            soundManager.PlayBgm(bgmClip);
        }
    }
}