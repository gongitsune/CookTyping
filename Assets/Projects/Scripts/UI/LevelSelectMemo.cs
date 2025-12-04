using Projects.Scripts.Configs;
using TMPro;
using UnityEngine;

namespace Projects.Scripts.UI
{
    // レベル選択メモの挙動を制御するクラス
    // publicは他のクラスからもアクセス可能にする修飾子
    // classはクラスを定義するキーワード
    // LevelSelectMemoはクラス名
    public class LevelSelectMemo : MonoBehaviour
    {
        // レベル設定に基づいてメモを設定するメソッド
        // public は他のクラスからアクセス可能にする修飾子
        // void は戻り値がないことを示すキーワード
        // SetMemoという名前のメソッドを定義
        // levelConfigはLevelConfig型の引数
        public void SetMemo(LevelConfig levelConfig)
        {
            // var は型推論を使用して変数を宣言するキーワード
            // この場合、textはTextMeshProUGUI型として推論される
            // メモのテキストコンポーネントを取得し、レベル名を設定
            var text = GetComponentInChildren<TextMeshProUGUI>();

            // 取得したテキストコンポーネントのtextプロパティにレベル名を設定
            text.text = levelConfig.name;
        }
    }
}