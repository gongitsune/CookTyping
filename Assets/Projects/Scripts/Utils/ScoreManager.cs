using System;
using System.IO;
using UnityEngine;

namespace Projects.Scripts.Utils
{
    public static class ScoreManager
    {
        private static string GetJsonPath()
        {
            return Path.Combine(Application.persistentDataPath, "scores.json");
        }

        public static void SaveScore(string playerName, int score)
        {
            var scores = LoadScores();
            var newScore = new ScoreData { name = playerName, score = score };
            Array.Resize(ref scores, scores.Length + 1);
            scores[^1] = newScore;

            try
            {
                var json = JsonUtility.ToJson(new Scores(scores), true);
                File.WriteAllText(GetJsonPath(), json);
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to save score: {e.Message}");
            }
        }

        public static ScoreData[] LoadScores()
        {
            var path = GetJsonPath();
            if (!File.Exists(path)) return Array.Empty<ScoreData>();
            try
            {
                var json = File.ReadAllText(path);
                return JsonUtility.FromJson<Scores>(json).scores ?? Array.Empty<ScoreData>();
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to load scores: {e.Message}");
                return Array.Empty<ScoreData>();
            }
        }

        [Serializable]
        private class Scores
        {
            public ScoreData[] scores;

            public Scores(ScoreData[] scores)
            {
                this.scores = scores;
            }
        }
    }

    [Serializable]
    public class ScoreData
    {
        public string name;
        public int score;
    }
}