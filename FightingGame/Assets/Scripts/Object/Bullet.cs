using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FGDefine;

public class Bullet : Poolable
{
    float startTime;

    private void FixedUpdate()
    {
        startTime += Time.deltaTime;
        if (startTime >= 0.30f) 
        {
            startTime = Time.deltaTime;
            Managers.Resource.Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == (int)ENUM_LAYER_TYPE.Wall)
        {
            startTime = Time.deltaTime;
            Managers.Resource.Destroy(gameObject);
        }
    }
}
