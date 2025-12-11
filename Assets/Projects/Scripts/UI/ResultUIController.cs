using System;
using Cysharp.Threading.Tasks;
using LitMotion;
using Projects.Scripts.Configs;
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
        [SerializeField] private Image thumbnailImage, animatedImage;
        [SerializeField] private TMP_Text foodNameText;
        [SerializeField] private LevelsTableConfig levelsTableConfig;

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

            // ゲットした食材のサムネイルを表示
            var levelConfig = levelsTableConfig.GetLevelConfig(_applicationManager.SelectedLevel);
            Debug.Assert(levelConfig != null, "levelConfig != null");
            thumbnailImage.sprite = levelConfig.thumbnail;
            animatedImage.sprite = levelConfig.thumbnail;
            foodNameText.text = $"{levelConfig.name} をゲットした！";

            // 食材のアニメーション (拡大したり縮んだり)
            LMotion.Create(0.95f, 1.05f, 1f)
                .WithLoops(-1, LoopType.Flip)
                .Bind(scale => thumbnailImage.rectTransform.localScale = new Vector3(scale, scale, 1))
                .AddTo(this);
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