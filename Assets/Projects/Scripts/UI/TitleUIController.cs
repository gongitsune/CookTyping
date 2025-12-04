using System;
using UnityEngine;
using UnityEngine.UI;

namespace Projects.Scripts.UI
{
    public sealed class TitleUIController : MonoBehaviour
    {
        [SerializeField] private Button startButton;

        private void Start()
        {
            startButton.onClick.AddListener(OnClickStartButton);
        }
        
        private void OnClickStartButton()
        {
        }
    }
}
