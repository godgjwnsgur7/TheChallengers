using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

#if UNITY_EDITOR

using UnityEditor;

public class DefineController : AssetPostprocessor
{
    private static DefineSetter characterDefineSetter = new DefineSetter(CharacterDamageMenuName, "TEST_MODE");
    private static DefineSetter googleLoginDefineSetter = new GoogleLoginDefineSetter(GoggleLoginMenuName, "GOOGLE_LOGIN_MODE");
	private static DefineSetter bannerDefineSetter = new DefineSetter(BannerLoginMenuName, "ENABLE_BANNER");
	private static DefineSetter interstitialDefineSetter = new DefineSetter(InterstitialMenuName, "ENABLE_INTERSTITIAL");

	private const string GoggleLoginMenuName = "Debug/구글 로그인 활성화";
    private const string CharacterDamageMenuName = "Debug/캐릭터 데미지 치트";
	private const string BannerLoginMenuName = "Debug/배너 활성화";
	private const string InterstitialMenuName = "Debug/전면 광고 활성화";

	private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
    {
        googleLoginDefineSetter.SetDefine();
        characterDefineSetter.SetDefine();
    }

    [MenuItem(GoggleLoginMenuName)]
    public static void SetGoggleLoginMode()
    {
        googleLoginDefineSetter.SetTestMode();
    }

    [MenuItem(CharacterDamageMenuName)]
    public static void SetCharacterDamageMode()
    {
        characterDefineSetter.SetTestMode();
    }

	[MenuItem(BannerLoginMenuName)]
	public static void SetBannerMode()
	{
		bannerDefineSetter.SetTestMode();
	}

	[MenuItem(InterstitialMenuName)]
	public static void SetInterstitialMode()
	{
		interstitialDefineSetter.SetTestMode();
	}
}

public class DefineSetter
{
    protected string menuName = null;
    protected string[] symbols = null;

    public DefineSetter(string menuName, params string[] symbols)
    {
        this.menuName = menuName;
        this.symbols = symbols;
    }

    public void SetDefine()
    {
        Menu.SetChecked(menuName, HasTestModeSymbol);
        Debug.Log($"{menuName} : {HasTestModeSymbol}");
    }

    public void SetTestMode()
    {
        Menu.SetChecked(menuName, ToggleTestMode());
        Debug.Log($"{menuName} : {HasTestModeSymbol}");
    }

    protected virtual bool ToggleTestMode()
    {
        BuildTargetGroup targetGroup = EditorUserBuildSettings.selectedBuildTargetGroup;
        string definesString = PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup);
        List<string> allDefines = definesString.Split(';').ToList();

        if (!HasTestModeSymbol) // 기존에 심볼을 가지고 있지 않았다면 추가
        {
            allDefines.AddRange(symbols.Except(allDefines));
        }
        else
        {
            allDefines = allDefines.Except(symbols).ToList();
        }

        PlayerSettings.SetScriptingDefineSymbolsForGroup(
            targetGroup,
            string.Join(";", allDefines.ToArray()));

        return HasTestModeSymbol;
    }

    protected bool HasTestModeSymbol
    {
        get
        {
            BuildTargetGroup targetGroup = EditorUserBuildSettings.selectedBuildTargetGroup;
            string definesString = PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup);
            List<string> allDefines = definesString.Split(';').ToList();

            return symbols.All(allDefines.Contains);
        }
    }
}

public class GoogleLoginDefineSetter : DefineSetter
{
    public GoogleLoginDefineSetter(string menuName, params string[] symbols) : base(menuName, symbols)
    {

    }

    // 무조건 True
    // 추후엔 FGPlatform Library에 넣는 코드로 수정함
    //protected override bool ToggleTestMode()
    //{
    //    BuildTargetGroup targetGroup = EditorUserBuildSettings.selectedBuildTargetGroup;
    //    string definesString = PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup);
    //    List<string> allDefines = definesString.Split(';').ToList();

    //    allDefines.AddRange(symbols.Except(allDefines));

    //    PlayerSettings.SetScriptingDefineSymbolsForGroup(
    //        targetGroup,
    //        string.Join(";", allDefines.ToArray()));

    //    return HasTestModeSymbol;
    //}
}

#endif
