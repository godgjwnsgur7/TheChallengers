using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FGDefine;
using System;

public class Profile_Info
{
    public string nickname;
    public char rankEmblem;

    public Profile_Info(string _nickname, char _rankEmblem)
    {
        nickname = _nickname;
        rankEmblem = _rankEmblem;
    }
}

public class BaseProfile : MonoBehaviour
{
    [SerializeField] UserInfoWindowUI userInfoWindow;

    [SerializeField] Image rankEmblemImage;
    [SerializeField] Text userNicknameText;

    Profile_Info profileInfo;

    public bool IsInit
    {
        get;
        protected set;
    }
    public bool isMine = false;
    public bool IsMine
    {
        get { return isMine; }
        protected set
        {
            isMine = value;
            Set_UserNicknameColor();
        }
    }

    public virtual void Init(Profile_Info _profileInfo)
    {
        if (IsInit)
            return;

        IsInit = true;
        profileInfo = _profileInfo;
        userNicknameText.text = _profileInfo.nickname;

        if (_profileInfo.rankEmblem == 'X')
            rankEmblemImage.gameObject.SetActive(false);
        else
        {
            rankEmblemImage.gameObject.SetActive(true);
            rankEmblemImage.sprite = Managers.Resource.Load<Sprite>($"Art/Sprites/RankEmblem/RankEmblem_{_profileInfo.rankEmblem}");
        }

        rankEmblemImage.gameObject.SetActive(true);
    }

    public virtual void Clear()
    {
        IsInit = false;
        IsMine = false;
        userNicknameText.text = "";
        rankEmblemImage.gameObject.SetActive(false);
    }

    public void Set_UserNicknameColor()
    {
        userNicknameText.color = IsMine ? Managers.Data.Get_SelectColor() : new Color(1f, 1f, 1f, 1f);
    }

    public void Set_UserNickname(string _userNickname)
    {
        userNicknameText.text = _userNickname;
    }

    public Profile_Info Get_ProfileInfo() => profileInfo;
}