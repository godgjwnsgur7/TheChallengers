using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FGDefine;
using System;

public class CharProfileUI : MonoBehaviour
{
    public bool isInit = false;
    public bool isReady = false;
    public bool isMine = false;
    public bool isReadyLock = false;

    public ENUM_CHARACTER_TYPE currCharType = ENUM_CHARACTER_TYPE.Default;

    [Header ("Set In Editor")]
    [SerializeField] Image charImage;
    [SerializeField] Image readyStateImage;

    [SerializeField] Text charNameText;
    [SerializeField] Text userNicknameText; // 닉네임 받아와야 함

    [Header("Setting Resources With Editor")]
    [SerializeField] Sprite readySprite;
    [SerializeField] Sprite unreadySprite;

    public void Init()
    {
        if (isInit) return;

        isInit = true;

        // Set_UserNickname(string userNickname) // 내 닉네임 받아와
    }

    public void Set_UserNickname(string userNickname)
    {
        userNicknameText.text = userNickname; // 서버 전달
    }

    public void Set_Character(ENUM_CHARACTER_TYPE _charType)
    {
        if (!isMine || currCharType == _charType) return;

        currCharType = _charType;

        // 서버 전달

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
        if (isReadyLock || readyState == isReady) return;

        if (readyState)
        {
            StartCoroutine(IReadyLock(1f));
            readyStateImage.sprite = readySprite;
            isReady = true; // 서버 전달
        }
        else
        {
            StartCoroutine(IReadyLock(2f));
            readyStateImage.sprite = unreadySprite;
            isReady = false; // 서버 전달
        }
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
    }

    protected IEnumerator IReadyLock(float waitTime)
    {
        isReadyLock = true;

        yield return new WaitForSeconds(waitTime); // 테스트

        isReadyLock = false;
    }
}
