using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FGDefine;

public class CustormRoom_MapInfo : MonoBehaviour
{
    [SerializeField] Text mapNameText;
    [SerializeField] Text mapExplanationText;

    [SerializeField] Image currMapImage;
    [SerializeField] Image selectionEffectImage;
    
    [SerializeField] RectTransform[] mapImageRectTrs = new RectTransform[3];

    public void Set_CurrMapInfo(ENUM_MAP_TYPE _mapType)
    {
        currMapImage.sprite = Managers.Resource.Load<Sprite>($"Art/Sprites/Maps/{_mapType}");
        mapNameText.text = Managers.Data.Get_MapNameDict(_mapType);
        
        if(mapExplanationText != null)
            mapExplanationText.text = Managers.Data.Get_MapExplanationDict(_mapType);

        RectTransform selectionEffectImageRectTr = selectionEffectImage.gameObject.GetComponent<RectTransform>();
        selectionEffectImageRectTr.anchoredPosition = mapImageRectTrs[(int)_mapType].anchoredPosition;
    }
}
