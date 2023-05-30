using Firebase.Crashlytics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlatformCrashlytics
{
	void Init();
}

public class PlatformCrashlytics : IPlatformCrashlytics
{
	public void Init()
	{
		Crashlytics.ReportUncaughtExceptionsAsFatal = true;
		Firebase.FirebaseApp.LogLevel = Firebase.LogLevel.Debug;
	}
}
