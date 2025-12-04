using System.Collections.Generic;
using Projects.Scripts.Configs;
using Projects.Scripts.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Projects.Scripts.Typing
{
    public class TypingManager : MonoBehaviour
    {
        [SerializeField] private LevelsTableConfig levelsTableConfig;
        [SerializeField] private TMP_Text romajiText, japaneseText;

        private readonly HashSet<char> _ignoreChars = new() { ' ', '\n', '\r', '\b' };
        private ApplicationManager _applicationManager;
        private LevelConfig _levelConfig;
        private int _sentenceIndex; // 現在の文のインデックス
        private string _typingText; // 入力中の文字列

        private void Start()
        {
            _applicationManager = ApplicationManager.Instance;
            _levelConfig = levelsTableConfig.GetLevelConfig(_applicationManager.SelectedLevel);

            Keyboard.current.onTextInput += OnTextInput;

            UpdateObjectiveSentence();
        }

        private void OnDestroy()
        {
            Keyboard.current.onTextInput -= OnTextInput;
        }

        private void OnTextInput(char c)
        {
            // 無視する文字の場合は処理を行わない
            if (_ignoreChars.Contains(c)) return;

            var sentence = _levelConfig.sentences[_sentenceIndex];
            var romaji = sentence.romaji;
            if (c == romaji[_typingText.Length])
            {
                _typingText += c;
                romajiText.text =
                    $"<color=green>{_typingText}</color><color=grey>{romaji[_typingText.Length..]}</color>";
            }
            else
            {
                romajiText.text =
                    $"<color=green>{_typingText}</color><color=red>{c}</color><color=grey>{romaji[_typingText.Length..]}</color>";
            }

            if (_typingText.Length != romaji.Length) return;
            _sentenceIndex++;
            UpdateObjectiveSentence();
        }

        private void UpdateObjectiveSentence()
        {
            var sentence = _levelConfig.sentences[_sentenceIndex];
            // 表示するUIのテキストを目標とするテキストに設定
            romajiText.text = $"<color=grey>{sentence.romaji}</color>";
            japaneseText.text = sentence.japanese;
            // 入力中の文字列を初期化
            _typingText = "";
        }
    }
}