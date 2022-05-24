using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ENUM_ITEM_TYPE
{
    Boom = 0,
    Hammer = 1,
    Sword = 2,
    Sycthe = 3,

    Max
}

public class Item : MonoBehaviour
{
    [SerializeField] ENUM_ITEM_TYPE itemType;

    private float rotateSpeed = 100.0f;

    private void Update()
    {
        transform.Rotate(0, rotateSpeed * Time.deltaTime, 0);
    }
}
