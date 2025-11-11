using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 💡 敵の「監視（警戒しながらターゲットを向いて待機）」という行動だけを管理するスクリプトです。
public class EnemyActionWatch : MonoBehaviour
{
    // ターゲット（主にPlayer）の情報を入れるための箱です。
    Transform target;
    
    // 💡 実行中のコルーチン（行動ルーチン）を停止させるために覚えておく変数です。
    private Coroutine watchRoutine;

    // ゲーム開始時に一度だけ呼ばれます
    void Start()
    {
        // "Player"というタグのオブジェクトを探して、ターゲットとして設定します。
        target = GameObject.FindWithTag("Player").GetComponent<Transform>(); 
        
        // 動作確認のため、ゲームが始まったらすぐに待機行動を開始します。
        StartWatch();
    }
    
    // =========================================================

    // 「監視しながら待機する」行動を開始する命令です。
    public void StartWatch()
    {
        // もし既に動いている待機ルーチンがあったら、一度止めます。
        if (watchRoutine != null) StopCoroutine(watchRoutine);
        
        // 「凝視と待機」の一連の動作をコルーチンとして開始します。
        watchRoutine = StartCoroutine(WatchRoutine());
    }
    
    // 「監視しながら待機する」行動を停止する命令です。
    public void StopWatch()
    {
        // 実行中のコルーチンを停止します。
        if (watchRoutine != null) StopCoroutine(watchRoutine);
    }

    // =========================================================
    
    // 凝視の動作を管理します。
    private IEnumerator WatchRoutine()
    {
        // 物理演算の処理が終わるタイミングで実行を開始します。（ガクつきを防ぐため）
        yield return new WaitForFixedUpdate(); 

        while (true) // 停止が指示されるまで無限に繰り返します。
        {
            // ターゲット（Player）の方向を追従して向きます。
            transform.LookAt(target.position);
            
            // 物理演算の処理が終わるまで待ってからループを繰り返します。
            yield return new WaitForFixedUpdate();        
        }
    }
}