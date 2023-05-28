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
		Firebase.Crashlytics.Crashlytics.ReportUncaughtExceptionsAsFatal = true;
		Firebase.FirebaseApp.LogLevel = Firebase.LogLevel.Debug;

		// throw new System.Exception("파이어 베이스 에러 테스트");
	}
}
