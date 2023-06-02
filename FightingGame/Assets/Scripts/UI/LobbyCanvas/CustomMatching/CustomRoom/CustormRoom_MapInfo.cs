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
    [SerializeField] RectTransform selectionEffectRectTr;
    
    [SerializeField] GameObject[] mapCoverImageObjects = new GameObject[3];

    public void Set_CurrMapInfo(ENUM_MAP_TYPE _mapType)
    {
        currMapImage.sprite = Managers.Resource.Load<Sprite>($"Art/Sprites/Maps/{_mapType}");
        mapNameText.text = Managers.Data.Get_MapNameDict(_mapType);
        
        if(mapExplanationText != null)
            mapExplanationText.text = Managers.Data.Get_MapExplanationDict(_mapType);

        selectionEffectRectTr.position = mapCoverImageObjects[(int)_mapType].GetComponent<RectTransform>().position;

        foreach (GameObject mapCoverImageObject in mapCoverImageObjects)
            mapCoverImageObject.SetActive(true);

        mapCoverImageObjects[(int)_mapType].SetActive(false);
    }
}
