using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using FGDefine;

public class MapSelectPopup : PopupUI
{
    [SerializeField] Image selectMapImage;
    [SerializeField] Image[] mapImages;
    [SerializeField] RectTransform selectionEffectRectTr;

    [SerializeField] Text mapNameText;
    [SerializeField] Text mapDescriptionText;

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
        Set_MapInfo(ENUM_MAP_TYPE.CaveMap);
        
        RectTransform rectTr = mapImages[0].GetComponent<RectTransform>();
        selectionEffectRectTr.position = rectTr.position;

        if (!selectionEffectRectTr.gameObject.activeSelf)
            selectionEffectRectTr.gameObject.SetActive(true);

        gameObject.SetActive(true);
    }

    private void Set_MapInfo(ENUM_MAP_TYPE _mapType)
    {
        selectedMapType = _mapType;
        mapNameText.text = Managers.Data.Get_MapNameDict(_mapType);
        mapDescriptionText.text = Managers.Data.Get_MapExplanationDict(_mapType);
        selectMapImage.sprite = Managers.Resource.Load<Sprite>($"Art/Sprites/Maps/{_mapType}");
    }

    public void OnClick_MapSelectImage(int _mapTypeNum)
    {
        Set_MapInfo((ENUM_MAP_TYPE)_mapTypeNum);
        RectTransform rectTr = mapImages[_mapTypeNum].GetComponent<RectTransform>();
        selectionEffectRectTr.position = rectTr.position;

        mapDescriptionText.text = Managers.Data.Get_MapExplanationDict(selectedMapType);

        if (!selectionEffectRectTr.gameObject.activeSelf)
            selectionEffectRectTr.gameObject.SetActive(true);
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
