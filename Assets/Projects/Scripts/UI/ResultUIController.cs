using System;
using System.Linq;
using Cysharp.Threading.Tasks;
using LitMotion;
using Projects.Scripts.Configs;
using Projects.Scripts.Sound;
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

        [Space] [Header("Score List")] [SerializeField]
        private TMP_Text scoreListItemPrefab;

        [SerializeField] private RectTransform scoreListContent;
        [SerializeField] private TMP_InputField nameInputField;
        [SerializeField] private Button saveScoreButton;

        private ApplicationManager _applicationManager;
        private int _score;
        private SoundManager _soundManager;

        private void Start()
        {
            _applicationManager = ApplicationManager.Instance;
            _soundManager = SoundManager.Instance;
            var result = _applicationManager.Result;

            timeText.text = $"{result.TimeTaken:F2}";
            wrongCountText.text = $"{result.WrongCount}";
            var tps = result.TotalTypedCharacters / result.TimeTaken;
            typesPerSecondText.text = $"{tps:F2}";

            // 簡単なスコア計算例
            _score = (int)(tps * 100 - result.WrongCount * 20);
            scoreText.text = $"{Math.Max(_score, 0)}";

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

            // スコアリストを構築
            BuildScoreList();

            // スコア保存ボタンのクリックイベント
            saveScoreButton.onClick.AddListener(OnScoreSaveButtonClicked);
        }

        private void OnDestroy()
        {
            // ボタンのクリックイベントを解除
            returnButton.onClick.RemoveListener(OnReturnButtonClicked);
            saveScoreButton.onClick.RemoveListener(OnScoreSaveButtonClicked);
        }

        private void BuildScoreList()
        {
            // 既存のリストをクリア
            foreach (Transform child in scoreListContent) Destroy(child.gameObject);

            // スコアをロードして表示
            var scores = ScoreManager.LoadScores();
            if (scores.Any())
                Array.Sort(scores, (a, b) => b.score.CompareTo(a.score)); // スコアの降順でソート
            foreach (var (scoreData, i) in scores.Select((data, i) => (data, i)))
            {
                var item = Instantiate(scoreListItemPrefab, scoreListContent);
                item.text = $"{i + 1}. {scoreData.name}: {scoreData.score}";
            }
        }

        private void OnScoreSaveButtonClicked()
        {
            _soundManager.PlaySe(_soundManager.Config.buttonClickSeClip);

            var playerName = nameInputField.text;
            if (string.IsNullOrWhiteSpace(playerName))
            {
                Debug.LogWarning("Player name is empty. Score not saved.");
                return;
            }

            ScoreManager.SaveScore(playerName, _score);

            // スコアリストを更新
            BuildScoreList();
        }

        private void OnReturnButtonClicked()
        {
            _soundManager.PlaySe(_soundManager.Config.buttonClickSeClip);
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