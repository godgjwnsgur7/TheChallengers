using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using UnityEngine;

public static class AddressableHelper
{
    public static AddressableAssetEntry CreateAssetEntry<T>(T source, string groupName, string labelName) where T : Object
    {
        var entry = CreateAssetEntry(source, groupName);

        if (!LabelExists(labelName))
            CreateLabel(labelName);

        if (source != null)
            source.AddAddressableAssetLabel(labelName);

        return entry;
    }

    public static AddressableAssetEntry CreateAssetEntry<T>(T source, string groupName) where T : Object
    {
        if (source == null || string.IsNullOrEmpty(groupName) || !AssetDatabase.Contains(source))
            return null;

        var addressableSettings = AddressableAssetSettingsDefaultObject.Settings;
        var sourcePath = AssetDatabase.GetAssetPath(source);
        var sourceGuid = AssetDatabase.AssetPathToGUID(sourcePath);
        var group = !GroupExists(groupName) ? CreateGroup(groupName) : GetGroup(groupName);

        var entry = addressableSettings.CreateOrMoveEntry(sourceGuid, group);

        if (entry == null)
            return null;

        entry.address = sourcePath;
        addressableSettings.SetDirty(AddressableAssetSettings.ModificationEvent.EntryMoved, entry, true);

        return entry;
    }

    public static AddressableAssetEntry CreateAssetEntry<T>(T source) where T : Object
    {
        if (source == null || !AssetDatabase.Contains(source))
            return null;

        var addressableSettings = AddressableAssetSettingsDefaultObject.Settings;
        var sourcePath = AssetDatabase.GetAssetPath(source);
        var sourceGuid = AssetDatabase.AssetPathToGUID(sourcePath);
        var entry = addressableSettings.CreateOrMoveEntry(sourceGuid, addressableSettings.DefaultGroup);
        entry.address = sourcePath;

        addressableSettings.SetDirty(AddressableAssetSettings.ModificationEvent.EntryMoved, entry, true);

        return entry;
    }

    public static AddressableAssetGroup GetGroup(string groupName)
    {
        if (string.IsNullOrEmpty(groupName))
            return null;

        var addressableSettings = AddressableAssetSettingsDefaultObject.Settings;
        return addressableSettings.FindGroup(groupName);
    }

    public static AddressableAssetGroup CreateGroup(string groupName)
    {
        if (string.IsNullOrEmpty(groupName))
            return null;

        var addressableSettings = AddressableAssetSettingsDefaultObject.Settings;
        var group = addressableSettings.CreateGroup(groupName, false, false, false, addressableSettings.DefaultGroup.Schemas);

        addressableSettings.SetDirty(AddressableAssetSettings.ModificationEvent.GroupAdded, group, true);

        return group;
    }

    public static void CreateLabel(string labelName)
    {
        if (string.IsNullOrEmpty(labelName))
            return;

        var addressableSettings = AddressableAssetSettingsDefaultObject.Settings;
        addressableSettings.AddLabel(labelName);

        addressableSettings.SetDirty(AddressableAssetSettings.ModificationEvent.LabelAdded, null, true);
    }

    public static bool GroupExists(string groupName)
    {
        var addressableSettings = AddressableAssetSettingsDefaultObject.Settings;
        return addressableSettings.FindGroup(groupName) != null;
    }

    public static bool LabelExists(string label)
    {
        var addressableSettings = AddressableAssetSettingsDefaultObject.Settings;
        return addressableSettings.GetLabels().Exists(lab => lab.Equals(label));
    }
}