using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Crashlytics;
using System;

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
		Crashlytics.IsCrashlyticsCollectionEnabled = true;
		Firebase.FirebaseApp.LogLevel = Firebase.LogLevel.Debug;
#endif
	}

	public void Report(System.Exception e)
	{
		Crashlytics.LogException(e);
	}

	public void Report(string message)
	{
		Crashlytics.Log(message);
	}
	
}
