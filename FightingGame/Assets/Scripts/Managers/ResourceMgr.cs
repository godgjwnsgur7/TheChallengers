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

        return Object.Instantiate(prefab, parent);
    }

    public void GetAnimator(string type)
    {
        // 여기서 3개를 가져와서 셋팅하게 만들어야쥐~
    }

    public void Destroy(GameObject go)
    {
        if (go == null)
            return;

        Object.Destroy(go);
    }
}
