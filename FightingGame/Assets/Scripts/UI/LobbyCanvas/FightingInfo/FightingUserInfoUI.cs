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

    public void Set_UserInfo(DBUserData userData)
    {
        if(userData.victoryPoint == 0 && userData.defeatPoint == 0)
        {
            ratingEmblemImage.gameObject.SetActive(false);
            ratingPointText.text = "Unknown";
        }
        else
        {
            ratingEmblemImage.gameObject.SetActive(true);
            ratingEmblemImage.sprite = RankingScoreOperator.Get_RankingEmblemSprite(userData.ratingPoint);
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