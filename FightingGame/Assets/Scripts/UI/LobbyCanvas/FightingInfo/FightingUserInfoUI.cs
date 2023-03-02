using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FightingUserInfoUI : MonoBehaviour
{
    [SerializeField] Image charImage;
    [SerializeField] Image ratingEmblemImage;

    [SerializeField] Text userNicknameText;
    [SerializeField] Text ratingPointText;
    [SerializeField] Text battleRecordText;

    public bool IsInit
    {
        get;
        private set;
    }

    private void Awake()
    {
        IsInit = false;
    }

    public void Set_UserInfo(DBUserData userData)
    {
        if (IsInit)
            return;

        IsInit = true;

        if(userData.victoryPoint == 0 && userData.defeatPoint == 0)
        {
            ratingEmblemImage.gameObject.SetActive(false);
            ratingPointText.text = "Unknown";
        }
        else
        {
            char rank = RankingScoreOperator.Get_RankingEmblemChar(userData.ratingPoint);
            ratingEmblemImage.gameObject.SetActive(true);
            ratingEmblemImage.sprite = Managers.Resource.Load<Sprite>($"Art/Sprites/RankEmblem/RankEmblem_{rank}");
            ratingPointText.text = $"{userData.ratingPoint}점";
        }

        // Image Setting
        // charImage.sprite = // 아직 이미지 세팅 안됨 (임시)

        // Text Setting
        userNicknameText.text = userData.nickname;
        long winningRate = userData.victoryPoint / (userData.victoryPoint + userData.defeatPoint) * 100;
        battleRecordText.text = $"{userData.victoryPoint}승 {userData.defeatPoint}패 ({winningRate}%)";
    }
}