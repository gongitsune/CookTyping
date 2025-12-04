using Projects.Scripts.UI;
using UnityEngine;

namespace Projects.Scripts.Utils
{
    public class ApplicationManager : SingletonMonoBehavior<ApplicationManager>, IRootSceneInitializer
    {
        public int SelectedLevel { get; set; } = 1;

        public void Initialize()
        {
            Application.targetFrameRate = 60;
            DontDestroyOnLoad(gameObject);
        }
    }
}