using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCanvas : MonoBehaviour
{
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
}
