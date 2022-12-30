using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCanvas : MonoBehaviour
{
    bool isBreak = false;

    public void OnClick_Destory(GameObject g)
    {
        Destroy(g);
        Debug.Log("Destroy!");
    }

    public void OnClick_Deactivating(GameObject g)
    {
        g.SetActive(false);
        Debug.Log("Deactive!");
    }
    
    public void OnClick_Activating(GameObject g)
    {
        g.SetActive(true);
        Debug.Log("Activate!");
    }

    public void OnClick_Start()
    {
        StartCoroutine(ITest());
    }

    public void OnClick_Break()
    {
        StopCoroutine(ITest());
    }

    public IEnumerator ITest()
    {
        while(!isBreak)
        {
            
            yield return null;

        }

        if (isBreak)
            yield break;

        Debug.Log("3");
    }
}
