using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusManager : MonoBehaviour
{
    [SerializeField] GameObject MainObject; //このスクリプトをアタッチするオブジェクト
    [SerializeField] int hp = 3;             //hp現在値
    [SerializeField] int maxHp = 3;          //いずれmaxHp利用する際に使用

    [SerializeField] GameObject destroyEffect;  //撃破エフェクト
    [SerializeField] GameObject damageEffect;   //被弾エフェクト

    // 💡 Step6.1新規追加：ダメージを受けたことを通知するイベント
    // Vector3は攻撃者の位置。購読者はこれを受け取り、ノックバック方向を計算します。
    public event Action<Vector3> OnDamageTaken; 

    // Update is called once per frame
    void Update()
    {
        //hpが0以下なら、撃破エフェクトを生成してMainを破壊
        if (hp <= 0)
        {
            DestoryMainObject();
        }
    }

    public void Damage(Vector3 attackerPosition)
    {
        // HPを減少させ、ダメージエフェクトを発生させる
        hp--;
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
