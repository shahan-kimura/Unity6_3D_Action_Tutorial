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
    [SerializeField] private float criticalMultiplier = 2.0f;

    // 💡 Step 8.4 修正: Hitboxと同じロジックで親を探す
    // これにより、近接攻撃（剣や体当たり）は自動的に機能するようになる
    void Start()
    {
        // まだ持ち主が登録されていなければ、親から探す
        if (ownerStatus == null)
        {
            ownerStatus = GetComponentInParent<StatusManager>();
        }
    }

    // 飛び道具（レーザー）用：親がいないので生成時に外から教える
    public void Initialize(StatusManager owner)
    {
        this.ownerStatus = owner;
    }

    public int CalculateDamage()
    {
        // 持ち主がいない場合の安全策
        if (ownerStatus == null) return 0;

        float finalDamage = ownerStatus.CurrentAttack * damageMultiplier;

        // 💡 Step 8.4 追加: クリティカル判定
        // ここで倍率をかけるだけで、外部への通知（bool）はまだ行わない
        if (Random.value < ownerStatus.CurrentCritRate)
        {
            finalDamage *= criticalMultiplier;

            // 確認用ログ
            Debug.Log("Critical Hit! Damage: " + finalDamage);
        }

        return Mathf.RoundToInt(finalDamage);
    }
}