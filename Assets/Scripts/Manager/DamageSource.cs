using UnityEngine;

// 旧: AttackInfo (リネーム)
// 「ダメージの発生源」として、攻撃の威力を計算するクラス
public class DamageSource : MonoBehaviour
{
    [Header("Data Source")]
    // 💡 変数名は statsData のまま維持（8.1からの継続性）
    [SerializeField] private CharacterStatsData statsData;

    [Header("Attack Specs")]
    // 💡 変更点: 固定値加算をやめ、「倍率（Multiplier）」に変更
    // 例: 1.0 = 標準, 0.5 = 弱い, 2.0 = 強い
    [SerializeField] private float damageMultiplier = 1.0f;

    // 外部から動的に持ち主をセットする場合のメソッド
    public void Initialize(CharacterStatsData owner)
    {
        this.statsData = owner;
    }

    // 💡 変更点: プロパティをやめ、メソッドにする
    // ロジック変更: 「足し算」から「掛け算（ステータス × 倍率）」へ
    public int CalculateDamage()
    {
        // 持ち主がいない場合の安全策
        if (statsData == null) return 0;

        // 基礎攻撃力 × 倍率 を計算し、整数に直して返す
        return Mathf.RoundToInt(statsData.AttackPower * damageMultiplier);
    }
}