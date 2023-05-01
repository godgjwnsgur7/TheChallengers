using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroutineHelper : MonoBehaviour
{
    private static MonoBehaviour monoInstance;

    [RuntimeInitializeOnLoadMethod]
    private static void Initializer()
    {
        monoInstance = new GameObject($"[{nameof(CoroutineHelper)}]").AddComponent<CoroutineHelper>();
        DontDestroyOnLoad(monoInstance.gameObject);
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    public new static Coroutine StartCoroutine(IEnumerator coroutine)
    {
        return monoInstance.StartCoroutine(coroutine);
    }

    public new static void StopCoroutine(Coroutine coroutine)
    {
        monoInstance.StopCoroutine(coroutine);
    }
}