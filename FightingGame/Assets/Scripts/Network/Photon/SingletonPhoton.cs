using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonPhoton : MonoBehaviourPhoton
{
	/// <summary>
	/// MonobehaviourPhoton 내 Init()이 호출된 이후 불리는 Initialize
	/// </summary>

    public virtual void OnInit() { }
    public virtual void OnFree() { }
}

public class SingletonPhoton<T> : SingletonPhoton, IPunInstantiateMagicCallback where T : SingletonPhoton
{
    public static T Instance
    {
		get
		{
            if(instance == null)
			{
                GameObject g = null;

                if (PhotonLogicHandler.IsConnected)
                {
                    g = Managers.Resource.InstantiateEveryone("PhotonViewObject");
                    g.name = $"{typeof(T)}";
                }
                else
                {
                    g = new GameObject($"{typeof(T)}");
                }

                DontDestroyOnLoad(g);

                instance = g.AddComponent<T>();
                instance.Init();
                instance.OnInit();

                isOnInitialized = true;
            }

            return instance;
		}
    }
    private static T instance;

    private static bool isOnInitialized = false;
    protected static bool isSynchronized = false;

    public static bool IsAliveInstance
	{
        get
		{
            return instance != null && isOnInitialized;
		}
	}

    public new void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        instance = gameObject.AddComponent<T>();
        
        if(!IsInitialized)
            instance.Init();

        if (!isOnInitialized)
            instance.OnInit();

        isOnInitialized = true;
    }

    protected override void OnDestroy()
	{
        base.OnDestroy();
        OnFree();
	}
}
