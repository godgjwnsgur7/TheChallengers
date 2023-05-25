using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FGDefine;
using System;

/// <summary>
/// 캐릭터 이미지 변경, 유저 닉네임 초기세팅
/// </summary>
public class UserInfoUI : MonoBehaviour
{
    [SerializeField] UserInfo_SelectChar selectCharInfo;
    [SerializeField] UserInfo_GameStart gameStartInfo;

    [SerializeField] Text userNicknameText;
    [SerializeField] Image charImage;
    [SerializeField] Button selectionCompleteBtn;

    Action<ENUM_CHARACTER_TYPE> selectionCharacterCompleteCallBack;

    bool SelectionCharacterLock = false;

    public bool IsInit
    {
        get;
        private set;
    }
    
    public bool IsGameStartInit
    {
        get { return gameStartInfo.IsInit; }
    }

    public ENUM_CHARACTER_TYPE CurrCharacterType
    {
        get;
        private set;
    }

    private void Awake()
    {
        IsInit = false;
    }

    public void Init(DBUserData _userData)
    {
        if (IsInit) 
            return;

        IsInit = true;

        charImage.gameObject.gameObject.SetActive(false);
        CurrCharacterType = ENUM_CHARACTER_TYPE.Default;

        userNicknameText.text = _userData.nickname;
        gameStartInfo.Set_UserData(_userData);
        
        gameStartInfo.Close();
        selectCharInfo.Open();
    }

    public void Clear()
    {
        IsInit = false;
        SelectionCharacterLock = false;
    }

    public void Forced_SelectionCharacter()
    {
        if (CurrCharacterType == ENUM_CHARACTER_TYPE.Default)
        {
            ENUM_CHARACTER_TYPE charType = (ENUM_CHARACTER_TYPE)UnityEngine.Random.Range(1, (int)ENUM_CHARACTER_TYPE.Max);
            Set_SelectionCharacter(charType);
        }

        OnClick_SelectionCharacterComplete();
    }

    public void ChangeInfo_GameStart()
    {
        Deactive_SelectionCompleteBtn();
        selectCharInfo.Close();
        gameStartInfo.Open(CurrCharacterType);
    }

    public void Active_SelectionCompleteBtn(Action<ENUM_CHARACTER_TYPE> _selectionCharacterCompleteCallBack)
    {
        userNicknameText.color = new Color(180, 100, 221);

        selectionCharacterCompleteCallBack = _selectionCharacterCompleteCallBack;
        selectionCompleteBtn.gameObject.SetActive(true);
    }

    public void Deactive_SelectionCompleteBtn()
    {
        selectionCompleteBtn.gameObject.SetActive(false);
    }

    public void Set_SelectionCharacter(ENUM_CHARACTER_TYPE _characterType)
    {
        if (_characterType == CurrCharacterType)
            return;

        CurrCharacterType = _characterType;
        charImage.sprite = Managers.Resource.Load<Sprite>($"Art/Sprites/Characters/{_characterType}");
        selectCharInfo.Set_SelectionCharacter(_characterType);

        if (!charImage.gameObject.activeSelf)
            charImage.gameObject.SetActive(true);
    }

    public void OnClick_SelectionCharacterComplete()
    {
        if (SelectionCharacterLock) return;

        if (CurrCharacterType == ENUM_CHARACTER_TYPE.Default)
        {
            Managers.UI.popupCanvas.Open_NotifyPopup("캐릭터를 선택하지 않았습니다.");
            return;
        }

        SelectionCharacterLock = true;
        selectionCharacterCompleteCallBack(CurrCharacterType);
    }
}
