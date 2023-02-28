using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FGDefine;
using System;

public class ObjectOrderLayerElement : MonoBehaviour
{
    [SerializeField] ENUM_OBJECTLAYERLEVEL_TYPE layerLevelType = ENUM_OBJECTLAYERLEVEL_TYPE.Untagged;
    SpriteRenderer spriteRenderer = null;

    private void Awake()
    {
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();

        spriteRenderer.sortingOrder = 0;
    }

    private void OnEnable()
    {
        if(spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();

        spriteRenderer.sortingOrder = Managers.OrderLayer.Get_SequenceOrderLayer(layerLevelType);
    }

    private void OnDisable()
    {
        Managers.OrderLayer.Return_OrderLayer(layerLevelType, spriteRenderer.sortingOrder);
    }
}
