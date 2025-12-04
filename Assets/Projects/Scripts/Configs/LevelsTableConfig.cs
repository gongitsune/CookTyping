using System;
using System.Collections.Generic;
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

        public IList<LevelConfig> Levels => levels;
    }

    [Serializable]
    public class LevelConfig
    {
        public string name;
        public int level;
    }
}