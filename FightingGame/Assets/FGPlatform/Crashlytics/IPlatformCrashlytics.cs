using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Crashlytics;
using System;
using JetBrains.Annotations;

public static class Debug
{
	public static void LogError(object message, bool isException = false)
	{
        UnityEngine.Debug.LogError(message);

		if(isException)
        {
            PlatformCrashlytics.Report(new Exception(message.ToString()));
        }
        else
		{
			PlatformCrashlytics.Report(message.ToString());
		}
    }

	public static void LogWarning(object message)
	{
        UnityEngine.Debug.LogWarning(message);
    }

	public static void Log(object message)
	{
		UnityEngine.Debug.Log(message);
	}

	public static void LogFormat(string format, params object[] param)
	{
		UnityEngine.Debug.LogFormat(format, param);
	}
}

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

	public static void Report(System.Exception e)
	{
		Crashlytics.LogException(e);
	}

	public static void Report(string message)
	{
		Crashlytics.Log(message);
	}
	
}
