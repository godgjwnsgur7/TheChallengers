using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FGDefine;
using System;

public class CharProfileUI : MonoBehaviour
{
    // 확인용 public (임시)
    public bool isInit = false;
    public bool isReady = false;
    public bool isMine = false;
    public ENUM_CHARACTER_TYPE currCharType = ENUM_CHARACTER_TYPE.Default;

    [Header ("Set In Editor")]
    [SerializeField] Image charImage;
    [SerializeField] Image readyStateImage;

    [SerializeField] Text charNameText;
    [SerializeField] Text userNicknameText; // 닉네임 받아와야 함

    [Header("Setting Resources With Editor")]
    [SerializeField] Sprite readySprite;
    [SerializeField] Sprite unreadySprite;

    Action<ENUM_CHARACTER_TYPE, bool, string> OnUpdateProfileRequest = null;

    public void Init(Action<ENUM_CHARACTER_TYPE, bool, string> _OnUpdateProfileRequest)
    {
        if (isInit) return;

        isInit = true;

        OnUpdateProfileRequest = _OnUpdateProfileRequest;
    }

    public void Request_SyncProfile()
    {
        if(OnUpdateProfileRequest != null)
            OnUpdateProfileRequest(currCharType, isReady, userNicknameText.text);
    }

    public void Set_UserNickname(string userNickname)
    {
        userNicknameText.text = userNickname;

        Request_SyncProfile();
    }

    public void Set_Character(ENUM_CHARACTER_TYPE _charType)
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

        Request_SyncProfile();
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

        Request_SyncProfile();
    }

    public void OnClick_SeleteChar()
    {
        if (!isMine) return;

        Managers.UI.popupCanvas.Open_CharSelectPopup(Set_Character);
    }

    public void Clear()
    {
        Set_UserNickname("");
        Set_Character(ENUM_CHARACTER_TYPE.Default);
        Set_ReadyState(false);

        isInit = false;
        isMine = false;

        Request_SyncProfile();
    }
}
