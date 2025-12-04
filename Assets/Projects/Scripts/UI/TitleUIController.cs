using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Projects.Scripts.UI
{
    // タイトルUIの挙動を制御するクラス
    // publicは他のクラスからもアクセス可能にする修飾子
    // classはクラスを定義するキーワード
    // TitleUIControllerはクラス名
    public class TitleUIController : MonoBehaviour
    {
        // [SerializeField] はプライベートフィールドをインスペクターで設定可能にする属性
        // private は他のクラスからアクセスできない修飾子
        // Button型のstartButtonという名前のフィールドを定義
        [SerializeField] private Button startButton;

        // private は他のクラスからアクセスできない修飾子
        // SceneTransitionManager型の_transitionManagerという名前のフィールドを定義
        private SceneTransitionManager _transitionManager;

        // private は他のクラスからアクセスできない修飾子
        // void は戻り値がないことを示すキーワード
        // Startという名前のメソッドを定義
        private void Start()
        {
            // SceneTransitionManagerのインスタンスを取得して_transitionManagerに代入
            _transitionManager = SceneTransitionManager.Instance;

            // startButtonがクリックされたときにOnClickStartButtonメソッドを呼び出すリスナーを追加
            startButton.onClick.AddListener(OnClickStartButton);
        }

        // private は他のクラスからアクセスできない修飾子
        // void は戻り値がないことを示すキーワード
        // OnClickStartButtonという名前のメソッドを定義
        private void OnClickStartButton()
        {
            // シーン遷移マネージャーを使用してLevelSelectSceneに遷移する非同期処理を開始
            _transitionManager.LoadScene("LevelSelectScene").Forget();
        }
    }
}