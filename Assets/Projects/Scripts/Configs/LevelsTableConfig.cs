using System;
using UnityEngine;

namespace Projects.Scripts.Configs
{
    /// <summary>
    ///     レベルの設定を保存するためのScriptableObject
    /// </summary>
    [CreateAssetMenu(fileName = "Level Config", menuName = "Configs/Level Config", order = 0)]
    public class LevelsTableConfig : ScriptableObject
    {
        [SerializeField] private LevelConfig[] levels;

        public LevelConfig GetLevelConfig(int level)
        {
            foreach (var levelConfig in levels)
                if (levelConfig.level == level)
                    return levelConfig;

            Debug.LogError($"{level} レベルの {nameof(LevelConfig)} が見つかりません。");
            return null;
        }
    }

    [Serializable]
    public class LevelConfig
    {
        public string name;
        public int level;
        public TypingSentence[] sentences;
    }

    [Serializable]
    public class TypingSentence
    {
        public string japanese;
        public string romaji;
    }
}