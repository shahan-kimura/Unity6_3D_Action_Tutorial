using System.Collections;
using UnityEngine;

// 💡 状態の定義
public enum EnemyState
{
    Battle, // 通常行動（今はこれだけ）
    Stun    // ノックバック中
}

public class EnemyAI : MonoBehaviour
{
    private Rigidbody rb;
    private StatusManager statusManager;

    // 💡 行動リスト
    private EnemyAction[] actions;

    // ノックバック設定
    [Header("Knockback Settings")]
    [SerializeField] float knockbackPower = 10f;
    [SerializeField] float knockbackDuration = 0.5f;
    [SerializeField] float actionWaitDuration = 0.2f;

    // 現在の状態
    private EnemyState currentState = EnemyState.Battle;
    // 現在実行中のアクション
    private EnemyAction currentAction;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        statusManager = GetComponent<StatusManager>();

        // イベント購読
        if (statusManager != null) statusManager.OnDamageTaken += OnDamageTaken;

        // 💡自分についている「EnemyActionを継承したコンポーネント」を全部取ってくる
        // DashもWatchも、親がEnemyActionだからここに入る。これが継承のパワー！
        actions = GetComponents<EnemyAction>();

        // AIループ開始
        StartCoroutine(MainStateMachine());
    }

    void OnDestroy()
    {
        if (statusManager != null) statusManager.OnDamageTaken -= OnDamageTaken;
    }

    // 🧠 メインステートマシン（思考のループ）
    private IEnumerator MainStateMachine()
    {
        while (true)
        {
            switch (currentState)
            {
                case EnemyState.Battle:
                    yield return StartCoroutine(BattleRoutine());
                    break;

                case EnemyState.Stun:
                    // Stun中は操作不能なので、復帰するまでただ待つ
                    // （実際の物理挙動は OnDamageTaken で瞬間的に加えている）
                    yield return new WaitForSeconds(knockbackDuration);

                    // 復帰
                    currentState = EnemyState.Battle;
                    break;
            }

            yield return null; // 1フレーム待機
        }
    }

    // ⚔️ バトル状態のロジック（ランダム行動）
    private IEnumerator BattleRoutine()
    {
        // アクションがあれば実行
        if (actions.Length > 0)
        {
            // ランダムに1つ選ぶ
            currentAction = actions[Random.Range(0, actions.Length)];

            // 実行して、終わるまで待つ
            yield return StartCoroutine(currentAction.Execute());

            // 行動間のインターバル
            yield return new WaitForSeconds(actionWaitDuration);
        }
        else
        {
            // アクションがない場合の待機
            yield return new WaitForSeconds(actionWaitDuration);
        }
    }

    // ⚡ ダメージ検知（イベント）
    private void OnDamageTaken(Vector3 attackerPosition)
    {
        if (currentState == EnemyState.Stun) return;

        // 1. 強制的にStun状態へ移行
        // これにより、BattleRoutineの途中でも次のフレームからStun処理に移る
        currentState = EnemyState.Stun;

        // 2. 実行中のアクションを強制停止
        StopAllCoroutines(); // MainStateMachine も止まるので再起動が必要
        // 💡 実行中のアクションを停止
        if (currentAction != null)
        {
            currentAction.Stop();
            currentAction = null;
        }

        // 3. 物理ノックバック適用
        ApplyKnockbackForce(attackerPosition);

        // 4. ステートマシンを再起動（Stun状態から始まる）
        StartCoroutine(MainStateMachine());
    }

    private void ApplyKnockbackForce(Vector3 attackerPosition)
    {
        Vector3 dir = (transform.position - attackerPosition).normalized;
        dir.y = 0;

        rb.linearVelocity = Vector3.zero;
        Vector3 force = (dir * knockbackPower) + (Vector3.up * knockbackPower);
        rb.AddForce(force, ForceMode.Impulse);
    }
}