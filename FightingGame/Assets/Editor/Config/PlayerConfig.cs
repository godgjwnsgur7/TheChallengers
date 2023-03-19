using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using GoogleMobileAds.Editor;

[InitializeOnLoad]
public class PlayerConfig
{
	const string password = "123456";
	const string keyPath = "UserSettings/user.keystore";
	const string projectKey = "fightinggame";

	static PlayerConfig()
	{
#if UNITY_ANDROID

		PlayerSettings.Android.keystoreName = keyPath;
		PlayerSettings.keystorePass = password;

		PlayerSettings.Android.keyaliasName = projectKey;
		PlayerSettings.keyaliasPass = password;
#endif
	}
}
