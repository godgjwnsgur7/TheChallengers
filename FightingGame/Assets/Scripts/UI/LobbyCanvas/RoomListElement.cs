using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FGDefine;

public class CustomRoomData
{
    public float uniqueKey;
    public int personnelIndex;
    public ENUM_MAP_TYPE mapType;

    public CustomRoomData(float _uniqueKey, int _personnelIndex, ENUM_MAP_TYPE _mapType)
    {
        uniqueKey = _uniqueKey;
        personnelIndex = _personnelIndex;
        mapType = _mapType;
    }
}

public class RoomListElement : UIElement
{
    private float uniqueKey;

    [Header ("Set ")]
    [SerializeField] Image MapImage;
    [SerializeField] Text roomNameText;
    [SerializeField] Text masterNicknameText;

    [SerializeField] Image personnelImage;
    [SerializeField] Text personnelText;

    [Header("")]
    [SerializeField] Image personnel_ExistImage;

    public bool isInit = false;

    // DB 활용해서 해야할듯 일단 전체 올 스톱 -> CustomRoomData 자체가 DB에 있고, Key만 저장?
    public void Init(CustomRoomData roomData)
    {
        isInit = true;

        Set_Personnel(roomData.personnelIndex);
        Set_MapImage(roomData.mapType);
    }

    public void Set_Personnel(int _personnelIndex)
    {
        personnelText.text = $"{_personnelIndex} / 2";

        if (_personnelIndex == 1)
        {

        }
        else if (_personnelIndex == 2)
            personnelImage = personnel_ExistImage;
        else
            Debug.LogError($"personnelIndex 값 오류 : {_personnelIndex}");
    }

    public void Set_MapImage(ENUM_MAP_TYPE _mapType)
    {

    }
}
