using System.Linq;
using Cysharp.Threading.Tasks;
using LitMotion;
using Projects.Scripts.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Projects.Scripts.UI
{
    /// <summary>
    ///     シーン遷移時の演出を管理する
    /// </summary>
    public class SceneTransitionManager : SingletonMonoBehavior<SceneTransitionManager>, IRootSceneInitializer
    {
        private const string RootSceneName = "RootScene";
        private const string TitleSceneName = "TitleScene";

        [SerializeField] private CanvasGroup canvasGroup;

        public void Initialize()
        {
            DontDestroyOnLoad(gameObject);
            canvasGroup.gameObject.SetActive(false);
        }

        public async UniTask LoadScene(string sceneName, bool withAnimation = true)
        {
            canvasGroup.gameObject.SetActive(true);
            await LMotion.Create(0f, 1f, withAnimation ? 0.5f : 0).Bind(alpha => canvasGroup.alpha = alpha);
            await SceneManager.LoadSceneAsync(sceneName);
            await LMotion.Create(1f, 0f, withAnimation ? 0.5f : 0).Bind(alpha => canvasGroup.alpha = alpha);
            canvasGroup.gameObject.SetActive(false);
        }

        [RuntimeInitializeOnLoadMethod]
        private static void OnLoad()
        {
            OnLoadAsync().Forget();
        }

        private static async UniTaskVoid OnLoadAsync()
        {
#if UNITY_EDITOR
            // UnityEditorの時だけ現在開いているシーンのオブジェクトを無効化する
            var currentScene = SceneManager.GetActiveScene();
            if (currentScene.name != RootSceneName)
            {
                var rootGos = currentScene.GetRootGameObjects();
                foreach (var go in rootGos) go.SetActive(false);
                await SceneManager.LoadSceneAsync(RootSceneName, LoadSceneMode.Additive);
            }
#else
            SceneManager.LoadScene(RootSceneName);
#endif

            // ルートシーンの全てのコンポーネントを初期化
            await UniTask.Yield();
            var rootScene = SceneManager.GetSceneByName(RootSceneName);
            var rootSceneComponents = rootScene.GetRootGameObjects()
                .SelectMany(go => go.GetComponents<IRootSceneInitializer>());
            foreach (var initializer in rootSceneComponents) initializer.Initialize();

#if UNITY_EDITOR
            if (currentScene.name == RootSceneName)
            {
                await SceneManager.LoadSceneAsync(TitleSceneName);
            }
            else
            {
                await SceneManager.UnloadSceneAsync(rootScene);
                var rootGos = currentScene.GetRootGameObjects();
                foreach (var go in rootGos) go.SetActive(true);
            }
#else
            SceneManager.LoadScene(TitleSceneName);
#endif
        }
    }
}