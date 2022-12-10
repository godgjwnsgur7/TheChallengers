using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AddressableAssets;
using FGDefine;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.Threading.Tasks;


public enum ResourceType
{
    None = 0,
    Material = 1,
    Prefab = 2,
    Scene = 3,
    Animation = 4,
    Sprite = 5,
    Music = 6,
    Data = 7
}


[System.AttributeUsage(AttributeTargets.Class)]
public class ResourceAttribute : Attribute
{
    private ResourceType resourceType;
    private string resourcePath;

    public ResourceAttribute(ResourceType resourceType, string resourcePath)
    {
        this.resourceType = resourceType;
        this.resourcePath = resourcePath;
    }

    public string GetPath()
    {
        return resourcePath;
    }

    public ResourceType GetResourceType()
	{
        return resourceType;
	}
}

public class AsyncHandle<TObject> where TObject : UnityEngine.Object
{
    private AsyncOperationHandle<TObject> handle;

    public AsyncHandle(AsyncOperationHandle<TObject> handle = default)
	{
        this.handle = handle;
    }

    public bool IsDone
    {
        get
        {
            return handle.IsDone;
        }
    }

    public bool IsValid
    {
        get
        {
            return handle.Status == AsyncOperationStatus.Succeeded;
        }
    }

	public TObject GetObject()
	{
        return handle.Result;
	}
}

public class AddressableMgr
{
#if !UNITY_EDITOR
    private int loadedAssetCount = 0;
#endif
    public bool IsLoadDone
    {
        get
        {
#if !UNITY_EDITOR
            return loadedAssetCount == Enum.GetValues(typeof(ResourceType)).Length;
#else
            return true;
#endif
        }
    }

    public void Initialize()
    {
#if !UNITY_EDITOR
        foreach(var label in Enum.GetValues(typeof(ResourceType)))
        {
            DownLoadAsset(label.ToString());
        }
#endif
    }

#if !UNITY_EDITOR
    private void DownLoadAsset(string label)
    {
        Addressables.DownloadDependenciesAsync(label).Completed += (op) =>
            {
                if(op.IsDone && op.Status == AsyncOperationStatus.Succeeded)
                {
                    Addressables.Release(op);
                    loadedAssetCount++;
                }
            };
    }
#endif

    public AsyncHandle<GameObject> Instantiate(string path, Transform parent = null, Vector3 pos = default, Quaternion quaternion = default, bool isAttachComponent = true)
    {
        if (path == null || path == string.Empty)
            return null;

        return Instantiate<MonoBehaviour>(path, parent, pos, quaternion, isAttachComponent, null, null);
    }

    public AsyncHandle<GameObject> Instantiate<T>(Transform parent = null, Vector3 pos = default, Quaternion quaternion = default, bool isAttachComponent = true, Action<T> PushObj = null) where T : MonoBehaviour
    {
        string path = AttributeUtil.GetResourcePath<T>();

        if (path == null)
            return null;

        return Instantiate(path, parent, pos, quaternion, isAttachComponent, null, PushObj);
    }

    private AsyncHandle<GameObject> Instantiate<T>(string path, Transform parent, Vector3 pos, Quaternion quaternion, bool isAttachComponent, Action OnCompleted, Action<T> ResultCallBack) where T : MonoBehaviour
    {
        var async = Addressables.InstantiateAsync(path, pos, quaternion, parent);

        async.Completed += (op) =>
        {
            if (op.IsDone && op.Status == AsyncOperationStatus.Succeeded)
            {
                T monoObj = null;

                if (isAttachComponent)
                {
                    monoObj = op.Result.GetOrAddComponent<T>();
                }
                else
                {
                    monoObj = op.Result.GetComponent<T>();
                }

                OnCompleted?.Invoke();
                ResultCallBack?.Invoke(monoObj);
            }
        };

        return new AsyncHandle<GameObject>(async);
    }

    [Obsolete]
    public GameObject InstantiateSync(GameObject obj)
    {
        if (obj == null)
            return null;

        return UnityEngine.Object.Instantiate(obj);
    }

    public AsyncHandle<T> Load<T>(Action OnCompleted = null, Action<T> ResultCallBack = null) where T : UnityEngine.Object
	{
        string path = AttributeUtil.GetResourcePath<T>();
        return Load<T>(path, OnCompleted, ResultCallBack);
    }

    public AsyncHandle<T> Load<T>(string path, Action OnCompleted, Action<T> ResultCallBack) where T : UnityEngine.Object
    {
        var async = Addressables.LoadAssetAsync<T>(path);

        async.Completed += (op) =>
        {
            if (op.IsDone && op.Status == AsyncOperationStatus.Succeeded)
            {
                OnCompleted?.Invoke();
                ResultCallBack?.Invoke(op.Result);
            }
        };

        return new AsyncHandle<T>(async);
    }
}