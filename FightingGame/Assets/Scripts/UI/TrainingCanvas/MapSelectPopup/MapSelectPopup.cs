using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using FGDefine;

public class MapSelectPopup : PopupUI
{
    [SerializeField] MapPanel mapPanel;
    ENUM_MAP_TYPE selectedMapType = ENUM_MAP_TYPE.ForestMap;

    bool isInit = false;

    private void OnEnable()
    {
        Open();
    }

    public void OnClick_MapElementCallBack(ENUM_MAP_TYPE _mapType)
    {
        Managers.Scene.CurrentScene.GetComponent<TrainingScene>().Summon_Map(_mapType);
        Managers.UI.currCanvas.OnClick_Deactivate(this.gameObject);
    }

    public void Open()
    {
        if (!isInit)
        {
            isInit = true;
            mapPanel.Init(Set_SelectedMapTypeCallBack);
        }

        mapPanel.Reset_MapElement();
        this.gameObject.SetActive(true);
    }

    public void Set_SelectedMapTypeCallBack(ENUM_MAP_TYPE _selectedMapType)
    {
        selectedMapType = _selectedMapType;
        mapPanel.OnClick_SelectMap(OnClick_MapElementCallBack, selectedMapType);
    }

    public void OnClick_Exit()
    {
        this.gameObject.SetActive(false);
        selectedMapType = ENUM_MAP_TYPE.ForestMap;
    }
}
