using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FGDefine;
using System;

public class CharProfileUI : MonoBehaviour
{
    public bool isInit = false;

    [Header ("Set In Editor")]
    [SerializeField] Image charImage;
    [SerializeField] Image readyStateImage;

    [SerializeField] Text charNameText;
    [SerializeField] Text userNicknameText; // 닉네임 받아와야 함

    [Header("Setting Resources With Editor")]
    [SerializeField] Sprite readySprite;
    [SerializeField] Sprite unreadySprite;

    // 확인용 public (임시)
    public ENUM_CHARACTER_TYPE currCharType = ENUM_CHARACTER_TYPE.Default;
    public int userId = 0;
    public bool isReady = false;
    public bool isMine = false;

    Action <ENUM_CHARACTER_TYPE, string, bool> OnUpdateProfileRequest;

    public void Init()
    {
        if (isInit) return;

        isInit = true;

        Clear();

    }

    public void Update_Profile(ENUM_CHARACTER_TYPE charType, string userNickname, bool readyState)
    {
        Select_Char(charType);
        userNicknameText.text = userNickname;
        Set_ReadyState(readyState);
    }

    public void Select_Char(ENUM_CHARACTER_TYPE _charType)
    {
        if (!isMine || currCharType == _charType) return;

        currCharType = _charType;

        switch (currCharType)
        {
            case ENUM_CHARACTER_TYPE.Default:

                charNameText.text = "캐릭터 미선택";
                break;
            case ENUM_CHARACTER_TYPE.Knight:

                charNameText.text = "나이트";
                break;
            case ENUM_CHARACTER_TYPE.Wizard:

                charNameText.text = "위저드";
                break;
            default:
                charNameText.text = "없는 캐릭터?";
                break;
        }
    }

    public void Set_ReadyState(bool readyState)
    {
        if (isReady == readyState) return;

        if(readyState)
        {
            readyStateImage.sprite = readySprite;
            isReady = true;
        }
        else
        {
            readyStateImage.sprite = unreadySprite;
            isReady = false;
        }
    }

    public void OnClick_SeleteChar()
    {
        if (!isMine) return;

        Managers.UI.popupCanvas.Open_CharSelectPopup(Select_Char);
    }

    public void Clear()
    {
        Select_Char(ENUM_CHARACTER_TYPE.Default);

        isInit = false;
        isReady = false;
        isMine = false;
        userId = 0;
    }
}
