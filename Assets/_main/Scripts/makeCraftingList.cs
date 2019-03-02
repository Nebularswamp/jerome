using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class makeCraftingList {
    [MenuItem("Assets/Create/Crafting List")]
    public static void CreateMyAsset() {
        craftingList asset = ScriptableObject.CreateInstance<craftingList>();

        AssetDatabase.CreateAsset(asset, "Assets/craftingList.asset");
        AssetDatabase.SaveAssets();

        EditorUtility.FocusProjectWindow();

        Selection.activeObject = asset;
    }
}
