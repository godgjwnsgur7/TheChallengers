using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FGDefine;
using System;

public class MapPanelElement : MonoBehaviour
{
    Action<ENUM_MAP_TYPE> selectedMapTypeCallBack;

    [SerializeField] Image mapAreaImage;
    ENUM_MAP_TYPE myMapType;

    bool isInit = false;

    public void Init(Action<ENUM_MAP_TYPE> _mapTypeCallBack, ENUM_MAP_TYPE _myMapType)
    {
        if (isInit)
            return;

        isInit = true;
        selectedMapTypeCallBack = _mapTypeCallBack;
        myMapType = _myMapType;

        gameObject.name = myMapType.ToString();
        mapAreaImage.gameObject.GetComponent<Button>().onClick.AddListener(OnClick_MapType);

        mapAreaImage.gameObject.transform.GetComponent<Image>().sprite
            = Managers.Resource.Load<Sprite>($"Art/Sprites/Maps/{_myMapType}");
    }

    public void Set_MapAreaImageColor(Color _color) => mapAreaImage.color = _color;
    public void OnClick_MapType() => selectedMapTypeCallBack(myMapType);
}
