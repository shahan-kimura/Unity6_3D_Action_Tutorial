using UnityEngine;
using UnityEngine.VFX;
using System.Collections;

public class VFXDamageFeedback : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] VisualEffect vfx;
    [SerializeField] string propertyName = "GlitchIntensity";
    [SerializeField] string trailPropertyName = "TrailRate";

    [Header("Glitch Parameters")]
    [SerializeField] float glitchDuration = 0.2f; // 一瞬だけ揺らす
    [SerializeField] float glitchPower = 25f;    // Turbulenceの強さ（20〜25くらい）

    private StatusManager status;

    void Start()
    {
        // 親子関係を考慮して取得
        if (vfx == null) vfx = GetComponentInChildren<VisualEffect>();
        status = GetComponent<StatusManager>();

        // イベント購読
        if (status != null)
        {
            status.OnDamageTaken += PlayGlitch;
        }
    }

    void OnDestroy()
    {
        if (status != null) status.OnDamageTaken -= PlayGlitch;
    }

    // ダメージイベントから呼ばれる
    void PlayGlitch(Vector3 attackerPos)
    {
        // 既に揺れていても上書きして再生
        StopAllCoroutines();
        StartCoroutine(GlitchRoutine());
    }

    IEnumerator GlitchRoutine()
    {
        // 1. ノイズON（数値を渡す）
        if (vfx != null)
        {
            vfx.SetFloat(trailPropertyName, 0f);        // ★トレイルをOFF！
            vfx.SetFloat(propertyName, glitchPower);    // ノイズON
        }

        // 2. 指定時間待つ
        yield return new WaitForSeconds(glitchDuration);

        // 3. ノイズOFF（0に戻す）
        if (vfx != null)
        {
            vfx.SetFloat(trailPropertyName, 1f);        // ★トレイルをONに戻す
            vfx.SetFloat(propertyName, 0f);             // ノイズOFF
        }
    }
}