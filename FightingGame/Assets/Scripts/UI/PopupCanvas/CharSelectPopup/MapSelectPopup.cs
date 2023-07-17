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

    protected override void OnEnable()
    {
        isUsing = true;
        Managers.UI.Push_WindowExitStack(OnClick_Exit);
    }

    protected override void OnDisable()
    {
        isUsing = false;
        Managers.UI.Pop_WindowExitStack();
    }

    public void Open(Action<ENUM_MAP_TYPE> _onSelectionMap)
    {
        if (_onSelectionMap == null)
        {
            Debug.LogError("SelectionMapCallBack is Null!");
            return;
        }

        onSelectionMap = _onSelectionMap;
        
        gameObject.SetActive(true);
    }
    
    public void Close()
    {
        onSelectionMap = null;
        selectedMapType = ENUM_MAP_TYPE.CaveMap;
        gameObject.SetActive(false);
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
        Managers.Sound.Play_SFX(ENUM_SFX_TYPE.UI_Click_Enter);

        onSelectionMap?.Invoke(selectedMapType);
        Close();
    }

    public override void OnClick_Exit()
    {
        Managers.Sound.Play_SFX(ENUM_SFX_TYPE.UI_Click_Cancel);

        Close();
    }
}
