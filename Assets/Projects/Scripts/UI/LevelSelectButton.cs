using System.Linq;
using Projects.Scripts.Configs;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

// Projects.Scripts.UI名前空間を定義
namespace Projects.Scripts.UI
{
    // レベル選択ボタンの挙動を制御するクラス
    // publicは他のクラスからもアクセス可能にする修飾子
    // classはクラスを定義するキーワード
    // LevelSelectButtonはクラス名
    public class LevelSelectButton : // : は継承の意味
        MonoBehaviour, // MonoBehaviourを継承  
        IPointerEnterHandler, // ポインターが入ったときのイベントを処理するためのインターフェース
        IPointerExitHandler // ポインターが出たときのイベントを処理するためのインターフェース
    {
        // [SerializeField] はプライベートフィールドをインスペクターで設定可能にする属性
        // private は他のクラスからアクセスできない修飾子
        // LevelsTableConfig型のlevelsTableConfigという名前のフィールドを定義
        // レベル設定テーブル
        [SerializeField] private LevelsTableConfig levelsTableConfig;

        // [SerializeField] はプライベートフィールドをインスペクターで設定可能にする属性
        // private は他のクラスからアクセスできない修飾子
        // LevelSelectMemo型のlevelSelectMemoという名前のフィールドを定義
        // レベル選択メモコンポーネント(インスペクターから設定)
        [SerializeField] private LevelSelectMemo levelSelectMemo;

        // [SerializeField] はプライベートフィールドをインスペクターで設定可能にする属性
        // private は他のクラスからアクセスできない修飾子
        // int型のlevelという名前のフィールドを定義
        // レベル
        [SerializeField] private int level;

        // private は他のクラスからアクセスできない修飾子
        // LevelConfig型の_levelConfigという名前のフィールドを定義
        // Startメソッドで代入するlevelに従ったレベル設定
        private LevelConfig _levelConfig;

        // private は他のクラスからアクセスできない修飾子
        // void は戻り値がないことを示すキーワード
        // Startという名前のメソッドを定義
        // Startメソッド (コンポーネントが初めにアクティブになった時一度だけ呼ばれる)
        private void Start()
        {
            // レベルに対応するレベル設定を取得
            // _levelConfigにlevelsTableConfigのLevelsからlevelと一致する最初の要素を代入
            // = は代入演算子
            _levelConfig = levelsTableConfig.Levels.First(l => l.level == level);

            // レベル設定が見つからなかった場合はエラーを出力
            // DebugクラスのAssertという静的メソッドを使用
            // 第一引数に条件式、第二引数にエラーメッセージを指定
            // _levelConfig != nullはレベル設定がnullでないことを確認する条件式
            // $"{level} レベルの {nameof(LevelConfig)} が見つかりません。"はエラーメッセージ
            Debug.Assert(_levelConfig != null, $"{level} レベルの {nameof(LevelConfig)} が見つかりません。");

            // var は型推論を使用して変数を宣言するキーワード
            // この場合TMP_Text型のtext変数を宣言
            // GetComponentInChildren<TMP_Text>()はこのコンポーネントの子オブジェクトからTMP_Textコンポーネントを取得するメソッド
            // ボタンのテキストをレベルに設定
            var text = GetComponentInChildren<TMP_Text>();

            // textのtextプロパティに "Level {level}"という文字列を設定
            text.text = $"Level {level}";

            // レベル選択メモにレベル設定をセットし、非表示にする
            // levelSelectMemoのSetMemoメソッドを呼び出し、_levelConfigを引数として渡す
            levelSelectMemo.SetMemo(_levelConfig);
            // levelSelectMemoのgameObjectプロパティのSetActiveメソッドを呼び出し、falseを引数として渡す
            levelSelectMemo.gameObject.SetActive(false);
        }

        // public は他のクラスからもアクセス可能にする修飾子
        // void は戻り値がないことを示すキーワード
        // OnPointerEnterという名前のメソッドを定義
        // PointerEventData型のeventDataという名前の引数を第一引数として受け取る
        // ポインターがボタンに入ったときに呼ばれるメソッド
        public void OnPointerEnter(PointerEventData eventData)
        {
            // levelSelectMemoのgameObjectプロパティのSetActiveメソッドを呼び出し、trueを引数として渡す
            // レベル選択メモを表示
            levelSelectMemo.gameObject.SetActive(true);
        }

        // public は他のクラスからもアクセス可能にする修飾子
        // void は戻り値がないことを示すキーワード
        // OnPointerExitという名前のメソッドを定義
        // PointerEventData型のeventDataという名前の引数を第一引数として受け取る
        // ポインターがボタンから出たときに呼ばれるメソッド
        public void OnPointerExit(PointerEventData eventData)
        {
            // levelSelectMemoのgameObjectプロパティのSetActiveメソッドを呼び出し、falseを引数として渡す
            // レベル選択メモを非表示
            levelSelectMemo.gameObject.SetActive(false);
        }
    }
}