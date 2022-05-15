using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Firebase;
using Firebase.Database;
using Firebase.Extensions;
using System.Threading.Tasks;
using System;

public interface IPlatformDB
{

}

public delegate void DataSnapAction(DataSnapshot shot);

/// <summary>
/// 파이어베이스의 DB는 Json 형식으로 데이터를 저장함
/// </summary>

public class PlatformDB : IPlatformDB
{
    private readonly string URL = "https://projectfg-c8bc3-default-rtdb.firebaseio.com/";

    private FirebaseApp app;
    private FirebaseDatabase database;

    private DatabaseReference dbRootReference;

    public void InitDataBase()
    {
        app = FirebaseApp.DefaultInstance;
        app.Options.DatabaseUrl = new Uri(URL);

        database = FirebaseDatabase.DefaultInstance;
        dbRootReference = database.GetReferenceFromUrl(URL);
    }
     
    public void UpdateDB<T>(string[] hierachyPath, T data, Action OnSuccess = null, Action OnFailed = null, Action OnCanceled = null) where T : class
    {
        if (!typeof(T).IsSerializable)
            return;

        DatabaseReference reference = dbRootReference;

        foreach (string path in hierachyPath)
        {
            reference = reference.Child(path);
        }

        string jsonData = JsonUtility.ToJson(data);

        reference.SetRawJsonValueAsync(jsonData).ContinueWithOnMainThread(task =>
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
                OnSuccess?.Invoke();
            }
        });
    }

    public void DeleteDB<T>(string[] hierachyPath, T data, Action OnSuccess = null, Action OnFailed = null, Action OnCanceled = null) where T : class
    {

    }

    public void InsertDB<T>(string[] hierachyPath, T data, Action OnSuccess = null, Action OnFailed = null, Action OnCanceled = null) where T : class
    {

    }

    public void SelectDB<T>(string[] hierachyPath, Action<T> pushData = null, Action OnSuccess = null, Action OnFailed = null, Action OnCanceled = null) where T : class
    {
        if (!typeof(T).IsSerializable)
            return;

        DatabaseReference reference = dbRootReference;

        foreach (string path in hierachyPath)
        {
            reference = reference.Child(path);
        }

        reference.GetValueAsync().ContinueWith(task =>
        {
            if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                pushData?.Invoke((T)snapshot.Value);
            }
        });
    }
}
