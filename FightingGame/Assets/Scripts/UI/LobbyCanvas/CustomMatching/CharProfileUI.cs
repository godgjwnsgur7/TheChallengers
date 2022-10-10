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

    Coroutine allReadyCheckCoroutine;

    public void Init()
    {
        if (isInit) return;

        isInit = true;

        Set_UserNickname("닉네임 받아와야 함");
    }

    public void Set_UserNickname(string userNickname)
    {
        userNicknameText.text = userNickname; // 서버 전달
    }

    public void Set_Character(ENUM_CHARACTER_TYPE _charType)
    {
        if ((int)_charType <= (int)ENUM_CHARACTER_TYPE.Max 
            || currCharType == _charType)
            return;

        currCharType = _charType;

        PhotonLogicHandler.Instance.ChangeCharacter(currCharType);

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
                charNameText.text = "알 수 없음";
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
            isReady = true;
            PhotonLogicHandler.Instance.Ready();
            allReadyCheckCoroutine = StartCoroutine(IAllReadyCheck());
        }
        else
        {
            StopCoroutine(allReadyCheckCoroutine);
            StartCoroutine(IReadyLock(2f));
            readyStateImage.sprite = unreadySprite;
            isReady = false;
            PhotonLogicHandler.Instance.UnReady();
        }
    }

    public void OnClick_SeleteChar()
    {
        if (isReady)
        {
            StopCoroutine(allReadyCheckCoroutine);
            readyStateImage.sprite = unreadySprite;
            isReady = false;
            PhotonLogicHandler.Instance.UnReady();
        }

        Managers.UI.popupCanvas.Open_CharSelectPopup(Set_Character);
    }

    public void Clear()
    {
        Set_UserNickname("");
        Set_Character(ENUM_CHARACTER_TYPE.Default);

        Set_ReadyState(false);
        isInit = false;
    }

    protected IEnumerator IReadyLock(float waitTime)
    {
        isReadyLock = true;

        yield return new WaitForSeconds(waitTime);

        isReadyLock = false;
    }

    protected IEnumerator IAllReadyCheck()
    {
        bool allReadyState = false;

        while(isReady)
        {
            allReadyState = PhotonLogicHandler.Instance.IsAllReady();

            if(allReadyState)
            {
                // 일단 그냥 시작시켜 나중에 ㅋㅋ 확인해

                break;
            }
            yield return null;
        }
        
    }

    protected IEnumerator IUnReadyCheck()
    {
        while(true)
        {

            yield return null;
        }

        
    }
}
