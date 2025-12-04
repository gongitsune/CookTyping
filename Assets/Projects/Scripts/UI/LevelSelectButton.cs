using System.Linq;
using Projects.Scripts.Configs;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Projects.Scripts.UI
{
    public class LevelSelectButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private LevelsTableConfig levelsTableConfig;
        [SerializeField] private LevelSelectMemo levelSelectMemo;
        [SerializeField] private int level;

        private LevelConfig _levelConfig;

        private void Start()
        {
            _levelConfig = levelsTableConfig.Levels.First(l => l.level == level);
            Debug.Assert(_levelConfig != null, $"{level} レベルの {nameof(LevelConfig)} が見つかりません。");

            var text = GetComponentInChildren<TMP_Text>();
            text.text = $"Level {level}";

            levelSelectMemo.SetMemo(_levelConfig);
            levelSelectMemo.gameObject.SetActive(false);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            levelSelectMemo.gameObject.SetActive(true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            levelSelectMemo.gameObject.SetActive(false);
        }
    }
}