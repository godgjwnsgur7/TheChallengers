using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Firebase;
using Firebase.Database;
using Firebase.Extensions;
using System.Threading.Tasks;
using System;

/// <summary>
/// Insert나 Delete는 클라에서 해줄 요청이 아니다.
/// </summary>

public interface IPlatformDB
{
    bool InsertDB<T>(string[] hierachyPath, T data, Action<T> OnSuccess = null, Action OnFailed = null, Action OnCanceled = null);
    bool UpdateDB<T>(string[] hierachyPath, T data, Action<T> OnSuccess = null, Action OnFailed = null, Action OnCanceled = null);
    bool SelectDB<T>(string[] hierachyPath, Action<T> OnSuccess = null, Action OnFailed = null, Action OnCanceled = null);

}

public delegate void DataSnapAction(DataSnapshot shot);

/// <summary>
/// 파이어베이스의 DB는 Json 형식으로 데이터를 저장함
/// </summary>

public class PlatformDB : IPlatformDB
{
    private readonly string URL = "https://projectfg-c8bc3-default-rtdb.firebaseio.com";

    private FirebaseApp app;
    private FirebaseDatabase database;

    private DatabaseReference dbRootReference;

    public void InitDataBase()
    {
        app = FirebaseApp.DefaultInstance;
        app.Options.DatabaseUrl = new Uri(URL);

        database = FirebaseDatabase.DefaultInstance;
        dbRootReference = database.RootReference;

        DBSession.RegisterDB(this);
    }
     
    public bool UpdateDB<T>(string[] hierachyPath, T data, Action<T> OnSuccess = null, Action OnFailed = null, Action OnCanceled = null)
    {
        if (!typeof(T).IsSerializable)
        {
            Debug.LogError("넣을 데이터가 Serializable한 형식이 아닙니다.");
            return false;
        }

        DatabaseReference reference = dbRootReference;

        foreach (string path in hierachyPath)
        {
            reference = reference.Child(path);
        }

        reference.SetValueAsync(data).ContinueWithOnMainThread(task =>
        { 
            if(task.IsFaulted)
            {
                OnFailed?.Invoke();
            }
            else if(task.IsCanceled) 
            {
                OnCanceled?.Invoke();
            }
            else if(task.IsCompleted)
            {
                OnSuccess?.Invoke(data);
            }
        });

        return true;
    }

    public bool SelectDB<T>(string[] hierachyPath, Action<T> OnSuccess = null, Action OnFailed = null, Action OnCanceled = null)
    {
        if (!typeof(T).IsSerializable)
        {
            Debug.LogError("넣을 데이터가 Serializable한 형식이 아닙니다.");
            return false;
        }
            
        DatabaseReference reference = dbRootReference;

        foreach (string path in hierachyPath)
        {
            reference = reference.Child(path);
        }

        reference.GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                OnFailed?.Invoke();
            }
            else if (task.IsCanceled)
            {
                OnCanceled?.Invoke();
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                T data = (T)(snapshot.Value);

                OnSuccess?.Invoke(data);
            }
        });

        return true;
    }

	public bool InsertDB<T>(string[] hierachyPath, T data, Action<T> OnSuccess = null, Action OnFailed = null, Action OnCanceled = null)
	{
        return true;
    }
}
