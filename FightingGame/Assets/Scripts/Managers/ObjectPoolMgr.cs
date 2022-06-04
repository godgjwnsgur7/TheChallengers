using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolMgr 
{
    class Pool
    {
        public GameObject Original { get; private set; }
        public Transform Root { get; set; }

        Stack<Poolable> poolStack = new Stack<Poolable>(); 
    }

    Dictionary<string, Pool> pools = new Dictionary<string, Pool>();

    Transform root;

    public void Init()
    {
        if (root == null)
        {
            root = new GameObject { name = "@Pool_Root" }.transform;
            Object.DontDestroyOnLoad(root); // 임시
        }
    }

    public void Push(Poolable poolable)
    {
        
    }

    public Poolable Pop(GameObject original, Transform parent = null)
    {

        return null;
    }

    public GameObject GetOriginal(string name)
    {

        return null;
    }
}
