using FGDefine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class AutoAddressableChecker : AssetPostprocessor
{
    public static Dictionary<ResourceType, string[]> assetExtensionFilterDictionary = new Dictionary<ResourceType, string[]>()
    {
        { ResourceType.Material, new string[]{ ".mat"} },
        { ResourceType.Prefab, new string[]{ ".prefab" } },
        { ResourceType.Scene, new string[]{ ".unity", } },
        { ResourceType.Animation, new string[]{ ".anim", ".controller",} },
        { ResourceType.Sprite, new string[] { ".png", ".jpg", ".jpeg"} },
        { ResourceType.Music, new string[]{ ".mp3", ".ogg"} },
        { ResourceType.Data, new string[] { ".json", ".xlsx", ".csv", ".tsv", ".xml"} },
    };

    private static readonly string DefaultGroupName = "FG_BUNDLE_GROUP";

    public static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
    {
        if (importedAssets == null ||
            importedAssets.Length <= 0)
            return;

        foreach (var importedAsset in importedAssets)
        {
            if (IsBuildScene(importedAsset))
                continue;

            string assetExtension = Path.GetExtension(importedAsset);

            foreach(var extensionPair in assetExtensionFilterDictionary)
            {
                var labelName = extensionPair.Key;
                var extensionFilters = extensionPair.Value;

                if (extensionFilters.Contains(assetExtension))
                {
                    var obj = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(importedAsset);
                    AddressableHelper.CreateAssetEntry(obj, DefaultGroupName, labelName.ToString());

                    break;
                }
            }
        }  
    }

    private static bool IsBuildScene(string importedAsset)
	{
        string scenename = Path.GetFileNameWithoutExtension(importedAsset);

        var sceneNames = System.Enum.GetNames(typeof(ENUM_SCENE_TYPE));
        if (sceneNames.Contains(scenename))
            return true;

        return false;
    }
}
