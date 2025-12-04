using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Projects.Scripts.UI
{
    /// <summary>
    ///     シーン遷移時の演出を管理する
    /// </summary>
    public class SceneTransitionManager : MonoBehaviour
    {
        private const string RootSceneName = "RootScene";
        private const string TitleSceneName = "TitleScene";

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void OnLoad()
        {
#if UNITY_EDITOR
            // UnityEditorの時だけ現在開いているシーンのオブジェクトを無効化する
            var currentScene = SceneManager.GetActiveScene();
            if (currentScene.name != RootSceneName)
            {
                var rootGos = currentScene.GetRootGameObjects();
                foreach (var go in rootGos) go.SetActive(false);
                SceneManager.LoadScene(RootSceneName, LoadSceneMode.Additive);
            }
#else
            SceneManager.LoadScene(RootSceneName);
#endif

            // ルートシーンの全てのコンポーネントを初期化
            var rootScene = SceneManager.GetSceneByName(RootSceneName);
            var rootSceneComponents = rootScene.GetRootGameObjects()
                .SelectMany(go => go.GetComponents<IRootSceneInitializer>());
            foreach (var initializer in rootSceneComponents) initializer.Initialize();

#if UNITY_EDITOR
            if (currentScene.name == RootSceneName)
            {
                SceneManager.LoadScene(TitleSceneName, LoadSceneMode.Additive);
            }
            else
            {
                var rootGos = currentScene.GetRootGameObjects();
                foreach (var go in rootGos) go.SetActive(true);
            }
#else
            SceneManager.LoadScene(TitleSceneName, LoadSceneMode.Additive);
#endif
        }
    }
}