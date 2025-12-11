using System;
using Cysharp.Threading.Tasks;
using Projects.Scripts.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Projects.Scripts.UI
{
    public class ResultUIController : MonoBehaviour
    {
        [SerializeField] private TMP_Text timeText, wrongCountText, typesPerSecondText, scoreText;
        [SerializeField] private Button returnButton;

        private ApplicationManager _applicationManager;

        private void Start()
        {
            _applicationManager = ApplicationManager.Instance;
            var result = _applicationManager.Result;

            timeText.text = $"{result.TimeTaken:F2}";
            wrongCountText.text = $"{result.WrongCount}";
            var tps = result.TotalTypedCharacters / result.TimeTaken;
            typesPerSecondText.text = $"{tps:F2}";

            // 簡単なスコア計算例
            var score = (int)(tps * 100 - result.WrongCount * 50);
            scoreText.text = $"{Math.Max(score, 0)}";

            // ボタンのクリックイベントを設定
            returnButton.onClick.AddListener(OnReturnButtonClicked);
        }

        private void OnDestroy()
        {
            // ボタンのクリックイベントを解除
            returnButton.onClick.RemoveListener(OnReturnButtonClicked);
        }

        private static void OnReturnButtonClicked()
        {
            SceneTransitionManager.Instance.LoadScene("TitleScene").Forget();
        }
    }

    public class ResultData
    {
        public float TimeTaken = -1;
        public int TotalTypedCharacters = -1;
        public int WrongCount = -1;
    }
}