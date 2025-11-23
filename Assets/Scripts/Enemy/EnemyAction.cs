using System.Collections;
using UnityEngine;

// 💡 abstractクラス：これ自体はアタッチできない「抽象的な概念」としてのクラス
public abstract class EnemyAction : MonoBehaviour
{
    // 親AIが呼ぶ共通の命令
    public abstract IEnumerator Execute(); // 行動実行（コルーチン）
    public abstract void Stop();           // 強制停止
}