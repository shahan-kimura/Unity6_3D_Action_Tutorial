using UnityEngine;

// 攻撃判定（剣や体当たり）に付けるスクリプト
public class AttackInfo : MonoBehaviour
{
    [Header("Data Source")]
    // 💡 直接数値を書かず、データファイル(ScriptableObject)を参照します。
    // これにより、プランナーがデータファイルをいじるだけでバランス調整が可能になります。
    [SerializeField] private CharacterStatsData statsData;

    [Header("Weapon Bonus")]
    // 武器ごとの追加ダメージ（必要な場合のみ設定）
    [SerializeField] private int weaponBonus = 0;

    // 💡 プロパティ (Property) の導入
    // 変数のように見えますが、アクセスされた瞬間に {} の中身（getter）を実行して値を返す機能です。
    // 「attackInfo.CurrentDamage」と書くだけで、常に最新の計算結果を取得できます。
    public int CurrentDamage
    {
        get
        {
            // データがセットされているか安全確認
            if (statsData != null)
            {
                // 基礎攻撃力(データ) + 武器ボーナス(個別設定) を計算して返す
                return statsData.AttackPower + weaponBonus;
            }
            // データがない場合は安全に 0 を返す
            return 0; 
        }
    }
}