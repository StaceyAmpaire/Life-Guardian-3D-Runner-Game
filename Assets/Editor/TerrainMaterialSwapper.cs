using UnityEditor;
using UnityEngine;

public class TerrainMaterialSwapper : EditorWindow
{
    private Material _oldMaterial1;
    private Material _newMaterial1;
    private Material _oldMaterial2;
    private Material _newMaterial2;

    [MenuItem("Tools/Terrain Material Swapper")]
    public static void ShowWindow()
    {
        GetWindow<TerrainMaterialSwapper>("Terrain Material Swapper");
    }

    private void OnGUI()
    {
        GUILayout.Label("Swap Materials on Selected Terrains", EditorStyles.boldLabel);

        EditorGUILayout.Space();

        _oldMaterial1 = (Material)EditorGUILayout.ObjectField("Old Material 1", _oldMaterial1, typeof(Material), false);
        _newMaterial1 = (Material)EditorGUILayout.ObjectField("New Material 1", _newMaterial1, typeof(Material), false);

        EditorGUILayout.Space();

        _oldMaterial2 = (Material)EditorGUILayout.ObjectField("Old Material 2", _oldMaterial2, typeof(Material), false);
        _newMaterial2 = (Material)EditorGUILayout.ObjectField("New Material 2", _newMaterial2, typeof(Material), false);

        EditorGUILayout.Space();

        if (GUILayout.Button("Swap Materials"))
        {
            SwapMaterials();
        }
    }

    private void SwapMaterials()
    {
        if (_oldMaterial1 == null || _newMaterial1 == null || _oldMaterial2 == null || _newMaterial2 == null)
        {
            EditorUtility.DisplayDialog("Error", "Please assign all old and new materials.", "OK");
            return;
        }

        GameObject[] selectedObjects = Selection.gameObjects;

        if (selectedObjects.Length == 0)
        {
            EditorUtility.DisplayDialog("Info", "No GameObjects selected. Please select the terrain GameObjects you want to modify.", "OK");
            return;
        }

        foreach (GameObject obj in selectedObjects)
        {
            MeshRenderer meshRenderer = obj.GetComponent<MeshRenderer>();
            if (meshRenderer != null)
            {
                Material[] materials = meshRenderer.sharedMaterials;
                for (int i = 0; i < materials.Length; i++)
                {
                    if (materials[i] == _oldMaterial1)
                    {
                        materials[i] = _newMaterial1;
                    }
                    else if (materials[i] == _oldMaterial2)
                    {
                        materials[i] = _newMaterial2;
                    }
                }
                meshRenderer.sharedMaterials = materials;
                EditorUtility.SetDirty(meshRenderer);
            }
        }
        EditorUtility.DisplayDialog("Success", "Materials swapped on selected GameObjects.", "OK");
    }
}