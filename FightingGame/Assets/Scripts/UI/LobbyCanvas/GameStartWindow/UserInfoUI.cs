using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FGDefine;
using System;

public class UserInfoUI : MonoBehaviour
{
    [SerializeField] Image charImage;
    [SerializeField] Image ratingEmblemImage;

    [SerializeField] Text userNicknameText;
    [SerializeField] Text ratingPointText;
    [SerializeField] Text battleRecordText;

    [SerializeField] Button selectionCompleteBtn;

    bool SelectionCharacterLock = false;

    Action<ENUM_CHARACTER_TYPE> selectionCharacterCompleteCallBack;

    public bool IsInit
    {
        get;
        private set;
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
        if (IsInit) return;
        IsInit = true;

        CurrCharacterType = ENUM_CHARACTER_TYPE.Default;

        if (_userData.victoryPoint == 0 && _userData.defeatPoint == 0)
        {
            // ratingEmblemImage를 Unknown으로 셋팅해야 함 (임시)
            ratingPointText.text = "Unknown";
        }
        else
        {
            char rank = RankingScoreOperator.Get_RankingEmblemChar(_userData.ratingPoint);
            ratingEmblemImage.sprite = Managers.Resource.Load<Sprite>($"Art/Sprites/RankEmblem/RankEmblem_{rank}");
            
            long winningRate = _userData.victoryPoint / (_userData.victoryPoint + _userData.defeatPoint) * 100;
            battleRecordText.text = $"{_userData.victoryPoint}승 {_userData.defeatPoint}패 ({winningRate}%)";
            ratingPointText.text = $"{_userData.ratingPoint}점";
        }
        
        userNicknameText.text = _userData.nickname;
    }

    public void Active_SelectionCompleteBtn(Action<ENUM_CHARACTER_TYPE> _selectionCharacterCompleteCallBack)
    {
        selectionCharacterCompleteCallBack = _selectionCharacterCompleteCallBack;
        selectionCompleteBtn.gameObject.SetActive(true);
    }

    public void Deactive_SelectionCompleteBtn()
    {
        selectionCompleteBtn.gameObject.SetActive(false);
    }

    public void Set_SelectionCharacter(ENUM_CHARACTER_TYPE _characterType)
    {
        CurrCharacterType = _characterType;

        charImage.sprite = Managers.Resource.Load<Sprite>($"Art/Sprites/Characters/{_characterType}");
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
