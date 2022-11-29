using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITest : MonoBehaviour
{
    private void OnDestroy()
    {
        Debug.Log("OnDestroy!");
    }

    private void OnDisable()
    {

        Debug.Log("OnDisable!");
    }

    private void OnEnable()
    {
        Debug.Log("OnEnable!");
    }
}
