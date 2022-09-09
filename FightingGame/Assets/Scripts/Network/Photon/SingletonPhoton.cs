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

public class SingletonPhoton<T> : SingletonPhoton where T : SingletonPhoton
{
    public static T Instance
    {
		get
		{
            if(instance == null)
			{
                GameObject go = new GameObject();
                instance = go.AddComponent<T>();
                instance.Init();
                instance.OnInit();
			}

            return instance;
		}
    }
    private static T instance;

    protected override void OnDestroy()
	{
        base.OnDestroy();
        OnFree();
	}
}
