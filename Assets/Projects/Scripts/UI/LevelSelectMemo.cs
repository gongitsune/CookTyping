using Projects.Scripts.Configs;
using TMPro;
using UnityEngine;

namespace Projects.Scripts.UI
{
    public class LevelSelectMemo : MonoBehaviour
    {
        public void SetMemo(LevelConfig levelConfig)
        {
            var text = GetComponentInChildren<TextMeshProUGUI>();
            text.text = levelConfig.name;
        }
    }
}