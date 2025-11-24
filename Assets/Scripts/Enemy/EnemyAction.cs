using System.Collections;
using UnityEngine;

// 💡 行動の種類を定義
public enum ActionType
{
    Attack, // 攻撃 (Dash, Watchなど)
    Chase   // 追跡 (移動)
}

// 💡 abstractクラス：これ自体はアタッチできない「抽象的な概念」としてのクラス
public abstract class EnemyAction : MonoBehaviour
{
    public ActionType actionType; 

    // 親AIが呼ぶ共通の命令
    public abstract IEnumerator Execute(); // 行動実行（コルーチン）
    public abstract void Stop();           // 強制停止
}