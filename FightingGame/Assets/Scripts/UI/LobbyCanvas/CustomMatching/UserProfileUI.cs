using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FGDefine;
using System;

public class UserProfileUI : MonoBehaviour
{
    [Header("Set In Editor")]
    [SerializeField] UserInfoWindowUI userInfoWindow;

    [SerializeField] Image charImage;
    [SerializeField] Image readyStateImage;

    [SerializeField] Text charNameText;
    [SerializeField] Text userNicknameText;

    [SerializeField] ENUM_TEAM_TYPE teamType;
    [SerializeField] bool isMasterProfile;
    
    public ENUM_CHARACTER_TYPE currCharType = ENUM_CHARACTER_TYPE.Default;
    public bool isInit = false;
    public bool isMine = false;
    bool isReady = false;

    public bool IsReady
    {
        get { return isReady; }
        private set { IsReadyInfoUpdateCallBack(value); } 
    }

    public void Init()
    {
        if (isInit) return;

        isInit = true;
        isMine = true;
    }

    public void IsReadyInfoUpdateCallBack(bool _readyState)
    {
        if (isReady == _readyState)
            return;
        
        isReady = _readyState;

        if (isReady)
        {
            readyStateImage.sprite = Managers.Resource.Load<Sprite>("Art/Sprites/ReadySprite");
            if (isMine) // 제어권을 가졌다면 서버의 정보를 변경함
                PhotonLogicHandler.Instance.Ready();
        }
        else
        {
            readyStateImage.sprite = Managers.Resource.Load<Sprite>("Art/Sprites/UnreadySprite");
            if (isMine) // 제어권을 가졌다면 서버의 정보를 변경함
                PhotonLogicHandler.Instance.UnReady();
        }
    }

    public void Set_UserNickname(string userNickname) => userNicknameText.text = userNickname;
    
    public void Set_Character(ENUM_CHARACTER_TYPE _charType)
    {
        if ((int)_charType >= (int)ENUM_CHARACTER_TYPE.Max || (int)currCharType == (int)_charType)
            return;

        currCharType = _charType;
        
        if(isMine) // 제어권을 가졌다면 서버의 정보를 변경함
        {
            PhotonLogicHandler.Instance.ChangeCharacter(currCharType);
            Managers.Battle.Set_CharacterType(currCharType);
        }

        charNameText.text = Managers.Battle.Get_CharNameDict(currCharType);
    }

    public void Set_ReadyState(bool readyState)
    {
        if (readyState && currCharType == ENUM_CHARACTER_TYPE.Default)
        {
            Managers.UI.popupCanvas.Open_NotifyPopup("캐릭터를 선택하지 않았습니다.");
            return;
        }

        IsReady = readyState;
    }
    
    public void OnClick_UserProfile()
    {
        if (userInfoWindow.gameObject.activeSelf
            || userNicknameText.text == "")
            return;

        userInfoWindow.Open_Request(isMasterProfile);
    }

    public void OnClick_SeleteChar()
    {
        if (!isMine)
            return;
        
        Set_ReadyState(false);
        Managers.UI.popupCanvas.Open_CharSelectPopup(Set_Character);
    }

    public void Clear()
    {
        Set_UserNickname("");
        Set_Character(ENUM_CHARACTER_TYPE.Default);
        Set_ReadyState(false);
        isInit = false;
    }
}
