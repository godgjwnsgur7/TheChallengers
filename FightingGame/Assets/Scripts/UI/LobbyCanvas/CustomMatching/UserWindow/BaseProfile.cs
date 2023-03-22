using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FGDefine;
using System;

public class BaseProfile : MonoBehaviour
{
    [SerializeField] UserInfoWindowUI userInfoWindow;

    [SerializeField] Image rankEmblemImage;
    [SerializeField] Text userNicknameText;

    public bool isMine = false;
    protected bool isInit = false;
    protected char myRankEmblem = ' ';

    public virtual void Init()
    {
        isInit = true;
    }

    public virtual void Clear()
    {
        isInit = false;
        isMine = false;
        userNicknameText.text = string.Empty;
        rankEmblemImage.gameObject.SetActive(false);
    }

    public void Set_RankingEmblem(long _score)
    {
        myRankEmblem = RankingScoreOperator.Get_RankingEmblemChar(_score);
        rankEmblemImage.sprite = Managers.Resource.Load<Sprite>($"Art/Sprites/RankEmblem/RankEmblem_{myRankEmblem}");
        rankEmblemImage.gameObject.SetActive(true);
    }

    public void Set_UserNickname(string _userNickname)
    {
        userNicknameText.text = _userNickname;
    }

    public char Get_RankingEmblem() => myRankEmblem;
    public string Get_UserNickname() => userNicknameText.text;
}