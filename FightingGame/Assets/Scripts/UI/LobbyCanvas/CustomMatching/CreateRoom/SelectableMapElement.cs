using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FGDefine;
using System;

public class SelectableMapElement : MonoBehaviour
{
    [SerializeField] ENUM_MAP_TYPE mapType;
    [SerializeField] Image mapImage;
    [SerializeField] GameObject selectedStatusObject;

    Action<ENUM_MAP_TYPE> OnChangeMapCallBack;

    public void Init(Action<ENUM_MAP_TYPE> _OnChangeMapCallBack)
    {
        OnChangeMapCallBack = _OnChangeMapCallBack;
    }

    public Sprite Get_MapImageSprite(ENUM_MAP_TYPE _mapType)
    {
        if(mapType != _mapType)
        {
            selectedStatusObject.SetActive(false);
            return null;
        }
        else
        {
            selectedStatusObject.SetActive(true);
            return mapImage.sprite;
        }
    }

    public void OnClick_ChangeMap()
    {
        OnChangeMapCallBack(mapType);
    }
}
