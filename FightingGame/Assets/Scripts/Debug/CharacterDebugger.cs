using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

#if UNITY_EDITOR

using UnityEditor;

public class CharacterDebugger : AssetPostprocessor
{
    private const string TestModeMenuName = "Debug/테스트 모드 세팅";
	private static string[] Symbols = new string[1]
	{
		"TEST_MODE"
	};

	public static bool IsTestMode
	{
		get { return HasTestModeSymbol; }
	}

	private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
	{
		Menu.SetChecked(TestModeMenuName, IsTestMode);
	}

	[MenuItem(TestModeMenuName)]
    public static void SetTestMode()
    {
        Menu.SetChecked(TestModeMenuName, ToggleTestMode()); 
	}

    private static bool ToggleTestMode()
    {
		BuildTargetGroup targetGroup = EditorUserBuildSettings.selectedBuildTargetGroup;
		string definesString = PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup);
		List<string> allDefines = definesString.Split(';').ToList();

		if (!IsTestMode) // 기존에 심볼을 가지고 있지 않았다면 추가
        {
			allDefines.AddRange(Symbols.Except(allDefines));
		}
		else
        {
			allDefines = allDefines.Except(Symbols).ToList();
		}

		PlayerSettings.SetScriptingDefineSymbolsForGroup(
			targetGroup,
			string.Join(";", allDefines.ToArray()));

		return IsTestMode;
    }

	private static bool HasTestModeSymbol
	{
		get
		{
			BuildTargetGroup targetGroup = EditorUserBuildSettings.selectedBuildTargetGroup;
			string definesString = PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup);
			List<string> allDefines = definesString.Split(';').ToList();

			return Symbols.All(allDefines.Contains);
		}
	}
}

#endif
