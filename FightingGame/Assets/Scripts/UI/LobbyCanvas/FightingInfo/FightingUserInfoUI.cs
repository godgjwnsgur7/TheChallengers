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
        // Image Setting
        // charImage.sprite = // 아직 이미지 세팅 안됨 (임시)
        ratingEmblemImage.sprite = RankingScoreOperator.Get_RankingEmblemSprite(userData.ratingPoint);

        // Text Setting
        userNicknameText.text = userData.nickname;
        ratingPointText.text = $"{userData.ratingPoint}점";
        long winningRate = userData.victoryPoint / (userData.victoryPoint + userData.defeatPoint) * 100;
        battleRecordText.text = $"{userData.victoryPoint}승 {userData.defeatPoint}패 ({winningRate}%)";
    }
}