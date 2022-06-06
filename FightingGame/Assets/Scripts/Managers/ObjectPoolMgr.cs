using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolMgr 
{
    #region Pool
    class Pool
    {
        public GameObject Original { get; private set; }
        public Transform Root { get; set; }

        Stack<Poolable> poolStack = new Stack<Poolable>(); 

        public void Init(GameObject original, int poolCount)
        {
            Original = original;
            Root = new GameObject().transform;
            Root.name = $"{original.name}_Root";
        
            for(int i = 0; i < poolCount; i++)
                Push(Create());
        }

        Poolable Create()
        {
            GameObject go = Object.Instantiate<GameObject>(Original);
            go.name = Original.name;
            return go.GetOrAddComponent<Poolable>();
        }

        public void Push(Poolable poolable)
        {
            if (poolable == null) return;

            poolable.transform.parent = Root;
            poolable.gameObject.SetActive(false);
            poolable.isUsing = false;

            poolStack.Push(poolable);
        }

        public Poolable Pop(Transform parent)
        {
            Poolable poolable;

            if (poolStack.Count > 0)
                poolable = poolStack.Pop();
            else
                poolable = Create();

            poolable.gameObject.SetActive(true);
            poolable.transform.parent = parent;
            poolable.isUsing = true;

            return poolable;
        }
    }
    #endregion

    Dictionary<string, Pool> pools = new Dictionary<string, Pool>();

    Transform root;

    int poolCount = 5;

    public void Init()
    {
        if (root == null)
        {
            root = new GameObject { name = "@Pool_Root" }.transform;
            Object.DontDestroyOnLoad(root); // 임시
        }
    }

    public void CreatePool(GameObject original, int count)
    {
        Pool pool = new Pool();
        pool.Init(original, count);
        pool.Root.parent = root;

        pools.Add(original.name, pool);
    }

    public void Push(Poolable poolable)
    {
        string name = poolable.gameObject.name;

        if(pools.ContainsKey(name) == false) // 이 경우엔 파괴
        {
            GameObject.Destroy(poolable.gameObject);
            return;
        }

        pools[name].Push(poolable);
    }

    public Poolable Pop(GameObject original, Transform parent = null)
    {
        if (pools.ContainsKey(original.name) == false)
            CreatePool(original, poolCount); // poolCount만큼 생성

        return pools[original.name].Pop(parent);
    }

    public void GeneratePool(GameObject original, int count, Transform parent = null)
    {
        if (pools.ContainsKey(original.name) == true)
        {
            Debug.Log($"{original} is Already Existing Pool");
            return;        
        }

        CreatePool(original, count);
    }

    public GameObject GetOriginal(string name)
    {
        if (pools.ContainsKey(name) == false)
            return null;

        return pools[name].Original;
    }

    public void Clear()
    {
        foreach(Transform child in root)
            GameObject.Destroy(child.gameObject);

        pools.Clear();
    }
}
