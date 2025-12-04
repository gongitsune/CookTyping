using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Projects.Scripts.UI
{
    public sealed class TitleUIController : MonoBehaviour
    {
        [SerializeField] private Button startButton;

        private SceneTransitionManager _transitionManager;

        private void Start()
        {
            _transitionManager = SceneTransitionManager.Instance;
            startButton.onClick.AddListener(OnClickStartButton);
        }

        private void OnClickStartButton()
        {
            _transitionManager.LoadScene("LevelSelectScene").Forget();
        }
    }
}