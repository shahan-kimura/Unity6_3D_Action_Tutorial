using UnityEngine;
using UnityEngine.VFX;

public class EnvironmentToVFX : MonoBehaviour
{
    [SerializeField] VisualEffect visualEffect; // 大元のVFX
    [SerializeField] string vfxPropertyName = "TargetMesh"; // VFX側の名前

    void Start()
    {
        // 1. 自分以下のすべてのメッシュを探す
        MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();
        CombineInstance[] combine = new CombineInstance[meshFilters.Length];

        // 2. 合体準備
        for (int i = 0; i < meshFilters.Length; i++)
        {
            combine[i].mesh = meshFilters[i].sharedMesh;
            combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
        }

        // 3. 巨大な1つのメッシュを作成
        Mesh combinedMesh = new Mesh();
        // 頂点数が多い場合のおまじない（IndexFormatを32bitにする）
        combinedMesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        combinedMesh.CombineMeshes(combine);

        // 4. VFX Graphに渡す
        if (visualEffect != null)
        {
            visualEffect.SetMesh(vfxPropertyName, combinedMesh);
        }
    }
}