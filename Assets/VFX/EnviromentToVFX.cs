using UnityEngine;
using UnityEngine.VFX;

// 💡 複数の静的メッシュ（地形など）を結合し、単一のVFX Graphへ渡すスクリプト
public class EnvironmentToVFX : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] VisualEffect visualEffect; // 適用するVFX
    [SerializeField] string propertyName = "TargetMesh"; // VFX側のプロパティ名

    void Start()
    {
        // 1. 自分以下のすべてのメッシュを探す
        MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();

        if (meshFilters.Length == 0) return;

        // 2. 合体（Combine）の準備
        CombineInstance[] combine = new CombineInstance[meshFilters.Length];

        for (int i = 0; i < meshFilters.Length; i++)
        {
            // メッシュデータと、その位置・回転・サイズ情報を取得
            combine[i].mesh = meshFilters[i].sharedMesh;
            combine[i].transform = meshFilters[i].transform.localToWorldMatrix;

            // 元のメッシュは見えなくていいので消す（影だけ残す設定の場合は適宜調整）
            var renderer = meshFilters[i].GetComponent<MeshRenderer>();
            if (renderer != null) renderer.enabled = false;
        }

        // 3. 巨大な1つのメッシュを作成
        Mesh combinedMesh = new Mesh();
        // 頂点数が多くなっても大丈夫なように32bit設定にする（必須）
        combinedMesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        combinedMesh.CombineMeshes(combine);

        // 4. VFX Graphに渡す
        if (visualEffect != null)
        {
            visualEffect.SetMesh(propertyName, combinedMesh);
        }
    }
}