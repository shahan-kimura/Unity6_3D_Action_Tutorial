using UnityEngine;
using TMPro; // 💡 TextMeshProを使うために必要

public class DamagePopup : MonoBehaviour
{
    // TextMeshProコンポーネントへの参照
    [SerializeField] private TextMeshPro textMesh;

    // 演出パラメータ
    [SerializeField] float moveSpeed = 2.0f;
    [SerializeField] float lifeTime = 1.0f;

    // 💡 生成時に呼ばれる初期化メソッド
    public void Setup(int damageAmount)
    {
        // 数値をテキストに反映
        textMesh.text = damageAmount.ToString();

        // 指定時間後に消滅
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        // 1. 上へ移動（ふわっと浮かぶ）
        transform.position += Vector3.up * moveSpeed * Time.deltaTime;

        // 2. カメラの方向を向く（ビルボード）
        // これがないと文字が裏返ったりして読めない
        if (Camera.main != null)
        {
            // カメラと逆方向（自分 - カメラ）を向くのが定石
            transform.rotation = Quaternion.LookRotation(transform.position - Camera.main.transform.position);
        }
    }
}