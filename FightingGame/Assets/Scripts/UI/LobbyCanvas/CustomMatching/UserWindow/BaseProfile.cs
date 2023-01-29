using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FGDefine;
using System;

public class BaseProfile : MonoBehaviour
{
    [Header("Set In Editor")]
    [SerializeField] UserInfoWindowUI userInfoWindow;

    [SerializeField] Image charImage;

    [SerializeField] Text charNameText;
    [SerializeField] Text userNicknameText;

    [SerializeField] bool isMasterProfile;
    
    private bool isInit = false;
    
    public bool isMine = false;
    public ENUM_CHARACTER_TYPE currCharType = ENUM_CHARACTER_TYPE.Default;

    public virtual void Init()
    {
        if (isInit) return;

        isInit = true;
        isMine = true;
    }

    public void Set_UserNickname(string userNickname)
    {
        userNicknameText.text = userNickname;
    }
    public string Get_UserNickname() => userNicknameText.text;

    public void Set_Character(ENUM_CHARACTER_TYPE _charType)
    {
        if ((int)_charType >= (int)ENUM_CHARACTER_TYPE.Max || (int)currCharType == (int)_charType)
            return;

        currCharType = _charType;
        
        if(isMine) // 제어권을 가졌다면 서버의 정보를 변경함
        {
            PhotonLogicHandler.Instance.ChangeCharacter(currCharType);
        }

        charNameText.text = Managers.Data.Get_CharNameDict(currCharType);
    }

    public void OnClick_UserProfile()
    {
        if (userNicknameText.text == "")
            return;

        userInfoWindow.Open_Request(isMasterProfile);
    }

    public void OnClick_SeleteChar()
    {
        if (!isMine)
            return;
        
        if(!PhotonLogicHandler.IsMasterClient)
            this.GetComponent<SlaveProfileUI>().Set_ReadyState(false);
        
        Managers.UI.popupCanvas.Open_CharSelectPopup(Set_Character);
    }

    public virtual void Clear()
    {
        Set_UserNickname("");
        Set_Character(ENUM_CHARACTER_TYPE.Default);
        isInit = false;
        isMine = false;
    }
}