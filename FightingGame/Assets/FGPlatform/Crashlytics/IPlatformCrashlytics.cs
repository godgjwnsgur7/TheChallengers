using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Crashlytics;
public interface IPlatformCrashlytics
{
	void Init();
}

public class PlatformCrashlytics : IPlatformCrashlytics
{
	public void Init()
	{
#if UNITY_ANDROID
		Crashlytics.ReportUncaughtExceptionsAsFatal = true;
		Firebase.FirebaseApp.LogLevel = Firebase.LogLevel.Debug;
#endif
	}
}
