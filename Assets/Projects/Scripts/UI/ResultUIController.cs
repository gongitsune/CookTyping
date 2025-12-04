using System;
using Projects.Scripts.Utils;
using TMPro;
using UnityEngine;

namespace Projects.Scripts.UI
{
    public class ResultUIController : MonoBehaviour
    {
        [SerializeField] private TMP_Text timeText, wrongCountText, typesPerSecondText, scoreText;

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
        }
    }

    public class ResultData
    {
        public float TimeTaken = -1;
        public int TotalTypedCharacters = -1;
        public int WrongCount = -1;
    }
}