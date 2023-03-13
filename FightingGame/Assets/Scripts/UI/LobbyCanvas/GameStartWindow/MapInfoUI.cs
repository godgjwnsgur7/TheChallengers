using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FGDefine;

public class MapInfoUI : MonoBehaviour
{
    [SerializeField] Image mapImage;
    [SerializeField] Text mapNameText;

    public void Open(ENUM_MAP_TYPE _mapType)
    {
        mapImage.sprite = Managers.Resource.Load<Sprite>($"Art/Sprites/Maps/{_mapType}");
        mapNameText.text = Managers.Data.Get_MapNameDict(_mapType);

        gameObject.SetActive(true);
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }
}
