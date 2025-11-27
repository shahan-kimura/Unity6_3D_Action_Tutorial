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
    private int maxHp;                           // 💡 Step8.6 追加: 計算された最大HP（回復時の上限用）


    // 💡 Step8.6 追加: レベル管理用変数
    [Header("Level System")]
    [SerializeField] private int level = 1; // ここを変えれば敵の強さが変わる
    [SerializeField] private int currentExp = 0;
    [SerializeField] private int expToNextLevel;

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
        // 💡 Step8.6 変更: 初期化処理を「ステータス再計算」メソッドへ置き換え
        UpdateStatus();

        // HPを最大値で開始
        hp = maxHp;
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
    // 💡 Step 8.5 変更: 引数に CriticalType を追加
    public void Damage(int damage, Vector3 attackerPosition, CriticalType type)
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
        popup.Setup(damage,type);

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

    // 💡 Step8.6 追加: ステータス計算メソッド
    // ScriptableObject(基本値) + Level(成長分) で現在値を決定する
    private void UpdateStatus()
    {
        if (statsData == null)
        {
            // データがない場合のフォールバック
            maxHp = 100;
            currentAttack = 10;
            currentCritRate = 0f;
            return;
        }

        // レベル補正値（Lv1が基準なので -1）
        int levelBonus = level - 1;

        // 計算式: 基礎値 + (レベルボーナス * 成長率)
        maxHp = statsData.MaxHp + (levelBonus * statsData.HpGrowth);
        currentAttack = statsData.AttackPower + (levelBonus * statsData.AttackGrowth);
        currentCritRate = statsData.CriticalRate + (levelBonus * statsData.CritRateGrowth);

        // 必要経験値の更新
        expToNextLevel = statsData.BaseExpToNext + (levelBonus * 50);
    }

    // 💡 Step8.6 追加: 経験値獲得
    public void AddExp(int amount)
    {
        currentExp += amount;
        // Expが一定以上でLvUP関数を呼び出し
        while (currentExp >= expToNextLevel)
        {
            currentExp -= expToNextLevel;
            LevelUp();
        }
    }

    // 💡 Step8.6 追加: レベルアップ処理
    private void LevelUp()
    {
        level++;

        // ステータス再計算
        UpdateStatus();

        // レベルアップ時はHP全回復
        hp = maxHp;

        // ログ確認用
        Debug.Log($"LEVEL UP! Lv.{level} HP:{maxHp} ATK:{currentAttack}");
    }

}
