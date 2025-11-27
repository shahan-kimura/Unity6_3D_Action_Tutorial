using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusManager : MonoBehaviour
{
    // 💡 このキャラクターのステータスデータ
    [Header("Data Source")]
    [SerializeField] private CharacterStatsData statsData;

    [SerializeField] GameObject MainObject;  //このスクリプトをアタッチするオブジェクト
    private int hp ;                             //hp現在値
    
    // 💡 Step8.3 追加: 現在の攻撃力を管理する変数（レベルアップ等で変動可能）
    [SerializeField] private int currentAttack;

    // 💡 Step8.3 追加: 外部（DamageSource）へ現在の攻撃力を公開するプロパティ
    public int CurrentAttack
    {
        get { return currentAttack; }
    }
    // 💡 Step8.4 追加: 外部（DamageSource）へ現在のCrit率を公開するプロパティ
    [SerializeField] private float currentCritRate;

    public float CurrentCritRate
    {
        get { return currentCritRate; }
    }

    // 💡 追加: ポップアップのプレハブ
    [Header("Effects")]
    [SerializeField] private DamagePopup popupPrefab;
    [SerializeField] GameObject destroyEffect;  //撃破エフェクト
    [SerializeField] GameObject damageEffect;   //被弾エフェクト

    // 💡 Step6.1新規追加：ダメージを受けたことを通知するイベント
    // Vector3は攻撃者の位置。購読者はこれを受け取り、ノックバック方向を計算します。
    public event Action<Vector3> OnDamageTaken; 
    
    void Start()
    {
        // データがあれば、そこからMaxHpを読み込んで初期化
        if (statsData != null)
        {
            hp = statsData.MaxHp;
            currentAttack = statsData.AttackPower;  // 💡 Step8.3 追加: 攻撃力もコピーして初期化
            currentCritRate = statsData.CriticalRate;

        }
        else
        {
            Debug.LogWarning("StatsDataが設定されていません。デフォルト値(100)を使用します。");
            hp = 100;
            currentAttack = 10;                     // 💡 Step8.3 追加: デフォルト攻撃力
            currentCritRate = 0f;
        }
    }
    // Update is called once per frame
    void Update()
    {
        //hpが0以下なら、撃破エフェクトを生成してMainを破壊
        if (hp <= 0)
        {
            DestoryMainObject();
        }
    }

    public void Damage(int damage,Vector3 attackerPosition)
    {
        // HPを減少させ、ダメージエフェクトを発生させる
        hp -= damage;
        var effect = Instantiate(damageEffect);
        effect.transform.position = transform.position;

        // Step8.2 ダメージポップアップ
        // 頭上（Y + 1.5m）に出す
        Vector3 spawnPos = transform.position + Vector3.up * 1.5f;
        var popup = Instantiate(popupPrefab, spawnPos, Quaternion.identity);
        // 数値を渡してセットアップ
        popup.Setup(damage);

        // 💡 Step6.1新規追加：ダメージを受けたことを通知します
        OnDamageTaken?.Invoke(attackerPosition); 
    }
    private void DestoryMainObject()
    {
        // 破壊エフェクトを発生させてから、MainObjectに設定したもの（自分自身や部位破壊対象）を破壊
        hp = 0;
        var effect = Instantiate(destroyEffect);
        effect.transform.position = transform.position;
        Destroy(effect, 5);
        Destroy(MainObject);
    }

}
