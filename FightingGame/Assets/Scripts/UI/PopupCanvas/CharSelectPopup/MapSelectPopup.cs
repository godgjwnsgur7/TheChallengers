using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using FGDefine;

public class MapSelectPopup : PopupUI
{
    [SerializeField] CustormRoom_MapInfo mapInfo;
    Action<ENUM_MAP_TYPE> onSelectionMap;
    ENUM_MAP_TYPE selectedMapType = ENUM_MAP_TYPE.CaveMap;

    public void Open(Action<ENUM_MAP_TYPE> _onSelectionMap)
    {
        if (_onSelectionMap == null)
        {
            Debug.LogError("SelectionMapCallBack is Null!");
            return;
        }

        onSelectionMap = _onSelectionMap;
        mapInfo.Set_CurrMapInfo(ENUM_MAP_TYPE.CaveMap);
        
        gameObject.SetActive(true);
    }

    public void OnClick_MapSelectImage(int _mapTypeNum)
    {
        if ((int)selectedMapType == _mapTypeNum)
            return;

        selectedMapType = (ENUM_MAP_TYPE)_mapTypeNum;
        mapInfo.Set_CurrMapInfo((ENUM_MAP_TYPE)_mapTypeNum);
    }

    public void OnClick_SelectCompletion()
    {
        onSelectionMap?.Invoke(selectedMapType);
        OnClick_Exit();
    }

    public override void OnClick_Exit()
    {
        base.OnClick_Exit();

        onSelectionMap = null;
        selectedMapType = ENUM_MAP_TYPE.CaveMap;
        gameObject.SetActive(false);
    }
}
