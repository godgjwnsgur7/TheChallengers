using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseEffect : MonoBehaviour
{
    public GameObject starPrefab;
    float spawnTime;
    public float defaultTime = 0.05f;
    void Update()
    {
        if (Input.GetMouseButton(0) && spawnTime >= defaultTime)
        {
            StarCreate();
            spawnTime = 0;
        }
        spawnTime += Time.deltaTime;
    }

    void StarCreate() 
    {
        Vector3 mPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mPosition.z = 0;
        Instantiate(starPrefab, mPosition, Quaternion.identity);
    }
}
