using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 💡 このスクリプトは、敵が単体で「突進」と「クールダウン」を繰り返す行動を管理します。
public class EnemyActionDash : MonoBehaviour
{
    [SerializeField] Transform target;
    
    // --- インスペクターから設定できる項目 ---
    [Header("Dash Settings")]
    [SerializeField] float dashSpeed = 15f;         // 突進時の速度
    [SerializeField] float dashDuration = 0.5f;     // 突進が持続する時間
    [SerializeField] float dashCooldown = 1.0f;     // 突進後の待機時間
    [SerializeField] float dashPreparationTime = 0.5f; // 突進前の予備動作にかかる時間

    private Rigidbody rb;
    private Vector3 dashDirection;
    
    // 突進動作のループを停止するために、コルーチンを保持します。
    private Coroutine dashRoutine;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        // "Player"というタグのオブジェクトを探して、ターゲットとして設定します。
        target = GameObject.FindWithTag("Player").GetComponent<Transform>();
        
        if (rb == null)
        {
            Debug.LogError("Rigidbody component not found on the EnemyActionDash object.");
            enabled = false;
            return;
        }

        // ゲーム開始時に、敵の行動（突進のループ）をすぐに開始します。
        dashRoutine = StartCoroutine(DashLoopRoutine()); 
    }

    // 💡 外部からこの行動を停止するための命令
    // 敵がスタンした時や破壊された時などに、確実に行動を止められます。
    public void StopDash()
    {
        if (dashRoutine != null) StopCoroutine(dashRoutine);
        if (rb != null) rb.linearVelocity = Vector3.zero;
    }

    // 💡 敵が繰り返し行動するために必要なメインのルーチンです。
    private IEnumerator DashLoopRoutine()
    {
        // 敵が破壊されるまで無限に繰り返す
        while (true)
        {
            // 突進の一連の行動（予備動作、突進、クールダウン）が完了するまで待機します。
            yield return StartCoroutine(DashSequence());
        }
    }


    // 突進の一連の行動（予備動作 -> 突進 -> クールダウン）を管理します。
    private IEnumerator DashSequence()
    {
        // ------------------ 1. 予備動作 (タメ) ------------------
        // 💡 プレイヤーに回避の猶予を与えるため、突進前にタメの時間を設けます。
        float startTime = Time.time;
        
        // タメ動作中、ターゲットの方を向き続けます
        while (Time.time < startTime + dashPreparationTime)
        {
            // ターゲットを凝視し、向きを固定します 
            transform.LookAt(target.position); 
            // 物理演算のタイミングに合わせて次のフレームを待ちます
            yield return new WaitForFixedUpdate(); 
        }

        // 予備動作が完了した時点のターゲット方向を最終決定
        dashDirection = (target.position - transform.position);
        dashDirection.y = 0; // 水平方向のみに突進させるためY軸は無視
        dashDirection.Normalize();

        // ------------------ 2. 突進実行 ------------------
        startTime = Time.time;
        
        while (Time.time < startTime + dashDuration)
        {
            // Y軸（落下速度）を維持しつつ、水平方向の速度を上書き
            Vector3 finalVelocity = dashDirection * dashSpeed;
            
            // 💡 Rigidbody.linearVelocityを使用し、重力による落下速度を維持します。
            finalVelocity.y = rb.linearVelocity.y; 

            rb.linearVelocity = finalVelocity;
            yield return new WaitForFixedUpdate();
        }

        // 突進終了後、水平方向の速度を0にして停止
        Vector3 stopVelocity = rb.linearVelocity;
        stopVelocity.x = 0;
        stopVelocity.z = 0;
        rb.linearVelocity = stopVelocity;

        // ------------------ 3. クールダウン ------------------
        // 次の突進までの待機時間
        yield return new WaitForSeconds(dashCooldown);
    }
}