using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FGDefine;

public class Item : InteractableObject
{
    [SerializeField] ENUM_ITEM_TYPE itemType;

    private float rotateSpeed = 100.0f;

    public override void Init()
    {
        base.Init();

        gameObject.layer = (int)ENUM_LAYER_TYPE.Item;
    }

    private void Update()
    {
        transform.Rotate(0, rotateSpeed * Time.deltaTime, 0);
    }

    public override void Interact()
    {
        
    }
}
