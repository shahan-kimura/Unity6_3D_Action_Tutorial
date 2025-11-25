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
        }
        else
        {
            Debug.LogWarning("StatsDataが設定されていません。デフォルト値(100)を使用します。");
            hp = 100;
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
