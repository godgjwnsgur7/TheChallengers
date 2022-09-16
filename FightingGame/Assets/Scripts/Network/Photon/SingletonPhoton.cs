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
    public static T MasterInstance
    {
		get
		{
            if(masterInstance == null)
			{
                InstantiateMaster();
            }

            return masterInstance;
		}
    }
    protected static T masterInstance;

    public static T SlaveInstance
    {
        get
        {
            if (slaveInstance == null)
            {
                InstantiateSlave();
            }

            return slaveInstance;
        }
    }
    protected static T slaveInstance;

    public static bool IsAliveInstance
	{
        get
		{
            return IsAliveMasterInstance && IsAliveSlaveInstance;

        }
	}

    private static bool isMasterOnInitialized = false;
    private static bool isSlaveOnInitialized = false;

    public static bool IsAliveMasterInstance
	{
        get
		{
            return masterInstance != null && isMasterOnInitialized;
		}
	}

    public static bool IsAliveSlaveInstance
	{
        get
		{
            return slaveInstance != null && isSlaveOnInitialized;
        }
	}

    public new void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        if(masterInstance == null)
		{
            gameObject.name = $"{typeof(T)} - Master";
            masterInstance = gameObject.GetComponent<T>();

            DontDestroyOnLoad(gameObject);

            if (!IsInitialized)
                masterInstance.Init();

            if (!isMasterOnInitialized)
                masterInstance.OnInit();

            isMasterOnInitialized = true;
            
            // 마스터 생성이 완료되면, 슬레이브를 이어 생성한다.
            if(!PhotonLogicHandler.IsMasterClient)
			{
                InstantiateSlave();
            }
        }
        else if(slaveInstance == null)
		{
            gameObject.name = $"{typeof(T)} - Slave";
            slaveInstance = gameObject.GetComponent<T>();

            DontDestroyOnLoad(gameObject);

            if (!IsInitialized)
                slaveInstance.Init();

            if (!isSlaveOnInitialized)
                slaveInstance.OnInit();

            isSlaveOnInitialized = true;
        }
    }

    private static void InstantiateMaster()
	{
        GameObject g = null;

        if (PhotonLogicHandler.IsConnected)
        {
            g = Managers.Resource.InstantiateEveryone("PhotonViewObject");
            g.name = $"{typeof(T)} - Master";
        }
        else
        {
            g = new GameObject($"{typeof(T)}");
        }

        DontDestroyOnLoad(g);

        masterInstance = g.GetComponent<T>();
        masterInstance.Init();
        masterInstance.OnInit();

        isMasterOnInitialized = true;
    }

    private static void InstantiateSlave()
    {
        if (!PhotonLogicHandler.IsConnected)
            return;

        GameObject g = Managers.Resource.InstantiateEveryone("PhotonViewObject");
        g.name = $"{typeof(T)} - Slave";

        DontDestroyOnLoad(g);

        slaveInstance = g.GetComponent<T>();
        slaveInstance.Init();
        slaveInstance.OnInit();

        isSlaveOnInitialized = true;
    }

    protected override void OnDestroy()
	{
        base.OnDestroy();
        OnFree();
	}
}
