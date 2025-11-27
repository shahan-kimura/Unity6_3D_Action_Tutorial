using UnityEngine;

// 旧: AttackInfo (リネーム)
// 「ダメージの発生源」として、攻撃の威力を計算するクラス
public class DamageSource : MonoBehaviour
{
    // 持ち主のStatusManagerを保持する変数
    private StatusManager ownerStatus;

    [Header("Attack Specs")]
    // 💡 変更点: 固定値加算をやめ、「倍率（Multiplier）」に変更
    // 例: 1.0 = 標準, 0.5 = 弱い, 2.0 = 強い
    [SerializeField] private float damageMultiplier = 1.0f;

    // 外部から動的に持ち主をセットする場合のメソッド
    public void Initialize(StatusManager owner)
    {
        this.ownerStatus = owner;
    }

    // 💡 変更点: プロパティをやめ、メソッドにする
    // ロジック変更: 「足し算」から「掛け算（ステータス × 倍率）」へ
    public int CalculateDamage()
    {
        // 持ち主がいない場合の安全策
        if (ownerStatus == null) return 0;

        // 💡 1. 持ち主から「今の攻撃力」をもらう
        // ここでアクセスしている CurrentAttack は、
        // StatusManagerで「変数の値を返す」ように修正されたプロパティです。
        int baseAttack = ownerStatus.CurrentAttack;

        // 基礎攻撃力 × 倍率 を計算し、整数に直して返す
        return Mathf.RoundToInt(baseAttack * damageMultiplier);
    }
}