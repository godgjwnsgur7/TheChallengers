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
            if(!PhotonLogicHandler.IsMasterClient)
			{
                Debug.LogWarning("현재 포톤용 싱글톤을 생성하는 주체가 마스터 클라이언트가 아닙니다. 제어가 어려울 수 있습니다.");
			}

            if(instance == null)
			{
                if (PhotonLogicHandler.IsConnected)
                {
                    var go = Managers.Resource.InstantiateEveryone($"SingletonPhoton/{typeof(T)}");
                    instance = go.GetOrAddComponent<T>();
                }
                else
                {
                    var go = new GameObject($"{typeof(T)}");
                    instance = go.AddComponent<T>();
                }

                DontDestroyOnLoad(instance.gameObject);

                instance.Init();
                instance.OnInit();
            }

            return instance;
		}
    }
    private static T instance;

    private static bool isOnInitialized = false;

    public new void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        Init();

        if(!isOnInitialized)
            OnInit();

        isOnInitialized = true;
    }

    protected override void OnDestroy()
	{
        base.OnDestroy();
        OnFree();
	}
}
