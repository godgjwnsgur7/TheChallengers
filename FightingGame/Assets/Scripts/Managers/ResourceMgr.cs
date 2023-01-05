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
            if (go != null)
                return go as T;
        }

        return Resources.Load<T>(path);
    }

    public T[] LoadAll<T>(string path) where T : Object
    {
        return Resources.LoadAll<T>(path);
    }

    public GameObject Instantiate(string fullPath, Vector3 position, Quaternion rotation, bool isActive = false)
    {
        GameObject original = Load<GameObject>(fullPath);
        if (original == null)
        {
            Debug.Log($"Failed to load prefab : {fullPath}");
            return null;
        }

        // 풀링된 오브젝트일 경우 위탁
        if (original.GetComponent<Poolable>() != null)
            return Managers.Pool.Pop(original, position, rotation, isActive).gameObject;

        GameObject go = Object.Instantiate(original);
        go.name = original.name;

        go.SetActive(isActive);
        go.transform.position = position;
        go.transform.rotation = rotation;

        return go;
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

    public GameObject Instantiate(string path, Vector2 position, Transform parent = null)
    {
        GameObject original = Load<GameObject>($"Prefabs/{path}");
        if (original == null)
        {
            Debug.Log($"Failed to load prefab : {path}");
            return null;
        }

        // 풀링된 오브젝트일 경우 위탁
        if (original.GetComponent<Poolable>() != null)
            return Managers.Pool.Pop(original, position).gameObject;

        GameObject go = Object.Instantiate(original, parent);
        go.transform.position = position;
        go.name = original.name;
        return go;
    }

    public GameObject InstantiateEveryone(string path, Vector2 position = default)
    {
        return PhotonLogicHandler.Instance.TryInstantiate($"Prefabs/{path}", position);
    }

    public void DestroyEveryone(MonoBehaviourPhoton obj)
	{
        PhotonLogicHandler.Instance.TryDestroy(obj);
	}

    public AttackObject GetAttackObject(string path)
    {
        GameObject original = Load<GameObject>($"Prefabs/AttackObjects/{path}");
        if(original == null)
        {
            Debug.Log($"Failed to load prefab : {path}");
            return null;
        }

        if (original.GetComponent<Poolable>() != null)
        {
            AttackObject attackObject = Managers.Pool.Pop(original, false) as AttackObject;

            if (attackObject != null)
                return attackObject;
        }

        Debug.Log($"Not Pooling Object : {path}");
        return null;
    }

    public EffectObject GetEffectObject(string path)
    {
        GameObject original = Load<GameObject>($"Prefabs/EffectObjects/{path}");
        if(original == null)
        {
            Debug.Log($"Failed to load prefab : {path}");
            return null;
        }

        if (original.GetComponent<Poolable>() != null)
        {
            EffectObject effectObject = Managers.Pool.Pop(original, false) as EffectObject;

            if(effectObject != null)
                return effectObject;
        }

        Debug.Log($"Not Pooling Object : {path}");
        return null;
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
        if (original == null)
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
