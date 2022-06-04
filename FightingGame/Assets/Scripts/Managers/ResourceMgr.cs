using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FGDefine;

public class ResourceMgr
{
    public T Load<T>(string path) where T : Object
    {
        return Resources.Load<T>(path);
    }

    public T[] LoadAll<T>(string path) where T : Object
    {
        return Resources.LoadAll<T>(path);
    }

    public GameObject Instantiate(string path, Transform parent = null)
    {
        GameObject prefab = Load<GameObject>($"Prefabs/{path}");
        if (prefab == null)
        {
            Debug.Log($"Failed to load prefab : {path}");
            return null;
        }

        GameObject go = Object.Instantiate(prefab, parent);
        go.name = prefab.name;

        return go;
    }

    public RuntimeAnimatorController GetAnimator(ENUM_CHARACTER_TYPE charType, ENUM_ANIMATOR_TYPE animType)
    {
        string path = charType.ToString() + animType.ToString();

        RuntimeAnimatorController animator = Load<RuntimeAnimatorController>($"Art/Animators/{path}");

        return animator;
    }

    public void Destroy(GameObject go)
    {
        if (go == null)
            return;

        // 만약 풀링이 필요하면 오브젝트 풀 매니저한테 위탁

        Object.Destroy(go);
    }
}
