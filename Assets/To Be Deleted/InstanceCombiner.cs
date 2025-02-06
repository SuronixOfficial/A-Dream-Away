using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering; // Required for IndexFormat

public class InstanceCombiner : MonoBehaviour
{
    [SerializeField] private List<MeshFilter> listMeshFilter;
    [SerializeField] private MeshFilter TargetMesh;

    [ContextMenu("Combine Meshes")]
    private void CombineMesh()
    {
        if (listMeshFilter == null || listMeshFilter.Count == 0)
        {
            Debug.LogError("No meshes to combine!");
            return;
        }

        var combine = new CombineInstance[listMeshFilter.Count];

        for (int i = 0; i < listMeshFilter.Count; i++)
        {
            if (listMeshFilter[i] == null || listMeshFilter[i].sharedMesh == null)
            {
                Debug.LogWarning($"MeshFilter at index {i} is null or has no mesh. Skipping.");
                continue;
            }

            combine[i].mesh = listMeshFilter[i].sharedMesh;
            combine[i].transform = listMeshFilter[i].transform.localToWorldMatrix;
        }

        var mesh = new Mesh();

        // **Fix: Support more than 65k vertices**
        mesh.indexFormat = IndexFormat.UInt32;

        mesh.CombineMeshes(combine, true, true);

        TargetMesh.mesh = mesh;

        SaveMesh(mesh, gameObject.name, false, true);

        Debug.Log($"<color=#20E7B0>Combine Meshes was Successful!</color>");
    }

    public static void SaveMesh(Mesh mesh, string name, bool makeNewInstance, bool optimizeMesh)
    {
        string path = EditorUtility.SaveFilePanel("Save Separate Mesh Asset", "Assets/", name, "asset");
        if (string.IsNullOrEmpty(path)) return;

        path = FileUtil.GetProjectRelativePath(path);
        Mesh meshToSave = makeNewInstance ? Object.Instantiate(mesh) : mesh;

        if (optimizeMesh)
            MeshUtility.Optimize(meshToSave);

        AssetDatabase.CreateAsset(meshToSave, path);
        AssetDatabase.SaveAssets();
    }
}
