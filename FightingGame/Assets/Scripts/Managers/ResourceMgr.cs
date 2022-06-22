using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FGDefine;

public class ResourceMgr
{
    public T Load<T>(string path) where T : Object
    {
        if (typeof(T) == typeof(GameObject))
        {
            string name = path;
            int index = name.LastIndexOf('/'); // 마지막 인덱스
            if (index >= 0)
                name = name.Substring(index + 1);

            GameObject go = Managers.Pool.GetOriginal(name);
            if(go != null)
                return go as T;
        }

        return Resources.Load<T>(path);
    }

    public T[] LoadAll<T>(string path) where T : Object
    {
        return Resources.LoadAll<T>(path);
    }

    public GameObject Instantiate(string path, Transform parent = null)
    {
        GameObject original = Load<GameObject>($"Prefabs/{path}");
        if (original == null)
        {
            Debug.Log($"Failed to load prefab : {path}");
            return null;
        }

        // 풀링된 오브젝트일 경우 위탁
        if (original.GetComponent<Poolable>() != null)
            return Managers.Pool.Pop(original, parent).gameObject;

        GameObject go = Object.Instantiate(original, parent);
        go.name = original.name;
        return go;
    }

    public RuntimeAnimatorController GetAnimator(ENUM_CHARACTER_TYPE charType)
    {
        string path = charType.ToString();

        RuntimeAnimatorController animator = Load<RuntimeAnimatorController>($"Art/Animators/{path}");

        return animator;
    }

    public void GenerateInPool(string path, int count, Transform parent = null)
    {
        GameObject original = Load<GameObject>($"Prefabs/{path}");
        if(original == null)
        {
            Debug.Log($"Failed to load prefab : {path}");
            return;
        }

        Managers.Pool.GeneratePool(original, count, parent);
    }

    public void Destroy(GameObject go)
    {
        if (go == null)
            return;

        // 풀링된 오브젝트일 경우 위탁
        Poolable poolable = go.GetComponent<Poolable>();
        if (poolable != null)
        {
            Managers.Pool.Push(poolable);
            return;
        }

        Object.Destroy(go);
    }
}
