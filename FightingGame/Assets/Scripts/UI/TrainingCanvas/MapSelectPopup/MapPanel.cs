using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using FGDefine;

public class MapPanel : MonoBehaviour
{
    Dictionary<ENUM_MAP_TYPE, MapPanelElement> mapPanelElementsDict = new Dictionary<ENUM_MAP_TYPE, MapPanelElement>();

    public void Init(Action<ENUM_MAP_TYPE> _selectedMapCallBack)
    {
        for (int i = 1; i < (int)ENUM_MAP_TYPE.Max; i++)
        {
            ENUM_MAP_TYPE mapType = (ENUM_MAP_TYPE)i;
            if (mapPanelElementsDict.ContainsKey(mapType))
                return;

            MapPanelElement mapPanelElement = Managers.Resource.Instantiate("UI/MapPanelElement", this.transform).GetComponent<MapPanelElement>();
            mapPanelElement.Init(_selectedMapCallBack, mapType);
            mapPanelElementsDict.Add(mapType, mapPanelElement);
        }

        Reset_MapElement();
    }

    public void Reset_MapElement()
    {
        foreach (MapPanelElement mapPanelElement in mapPanelElementsDict.Values)
            mapPanelElement.Set_MapAreaImageColor(Color.white);
    }

    public void OnClick_SelectMap(Action<ENUM_MAP_TYPE> _selectMapCallBack, ENUM_MAP_TYPE _selectedMapType)
    {
        _selectMapCallBack.Invoke(_selectedMapType);
    }
}
