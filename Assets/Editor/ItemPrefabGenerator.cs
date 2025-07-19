using UnityEngine;
using UnityEditor;
using System.IO;

public class ItemPrefabVariantGenerator : EditorWindow
{
    // UI Fields
    public GameObject basePrefab;
    public string spritesFolder = "Assets/Sprites/Items";
    public string outputFolder = "Assets/Prefabs/GeneratedItems";

    [MenuItem("Tools/Item Prefab Variant Generator")]
    public static void ShowWindow()
    {
        GetWindow<ItemPrefabVariantGenerator>("Item Prefab Generator");
    }

    void OnGUI()
    {
        GUILayout.Label("Generate Prefab Variants from Sprites", EditorStyles.boldLabel);

        basePrefab = (GameObject)EditorGUILayout.ObjectField("Base Prefab", basePrefab, typeof(GameObject), false);
        spritesFolder = EditorGUILayout.TextField("Sprites Folder", spritesFolder);
        outputFolder = EditorGUILayout.TextField("Output Folder", outputFolder);

        if (GUILayout.Button("Generate Variants"))
        {
            GenerateVariants();
        }
    }

    void GenerateVariants()
    {
        if (basePrefab == null)
        {
            Debug.LogError("Base prefab not assigned!");
            return;
        }

        if (!Directory.Exists(spritesFolder))
        {
            Debug.LogError($"Sprites folder not found: {spritesFolder}");
            return;
        }

        if (!Directory.Exists(outputFolder))
        {
            Directory.CreateDirectory(outputFolder);
            AssetDatabase.Refresh();
        }

        string[] spriteGUIDs = AssetDatabase.FindAssets("t:Sprite", new[] { spritesFolder });

        foreach (string guid in spriteGUIDs)
        {
            string spritePath = AssetDatabase.GUIDToAssetPath(guid);
            Sprite sprite = AssetDatabase.LoadAssetAtPath<Sprite>(spritePath);

            if (sprite == null)
                continue;

            string variantName = sprite.name;
            string variantPath = Path.Combine(outputFolder, variantName + ".prefab");
            variantPath = AssetDatabase.GenerateUniqueAssetPath(variantPath);

            // Create variant prefab
            GameObject variantPrefab = PrefabUtility.SaveAsPrefabAsset(basePrefab, variantPath);

            // Load prefab contents to override sprite
            GameObject instance = PrefabUtility.LoadPrefabContents(variantPath);

            // Try SpriteRenderer
            SpriteRenderer sr = instance.GetComponentInChildren<SpriteRenderer>();
            if (sr != null)
            {
                sr.sprite = sprite;
            }
            else
            {
                Debug.LogWarning($"No SpriteRenderer found on {variantName}, skipping.");
                PrefabUtility.UnloadPrefabContents(instance);
                continue;
            }

            PrefabUtility.SaveAsPrefabAsset(instance, variantPath);
            PrefabUtility.UnloadPrefabContents(instance);
            Debug.Log($"Created variant: {variantPath}");
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("✅ Finished generating prefab variants.");
    }
}
