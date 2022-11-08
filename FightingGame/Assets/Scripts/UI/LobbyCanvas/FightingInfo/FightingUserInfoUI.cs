using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FightingUserInfoUI : MonoBehaviour
{
    [SerializeField] Image charImage;
    [SerializeField] Image ratingEmblem;

    [SerializeField] Text userNicknameText;
    [SerializeField] Text ratingPointText;
    [SerializeField] Text battleRecordText;

    public void Set_UserInfo(DBUserData userData)
    {
        // Image Setting
        // 아직 이미지 없음

        // Text Setting
        userNicknameText.text = userData.nickname;
        ratingPointText.text = $"{userData.ratingPoint}점";
        long winningRate = userData.victoryPoint / (userData.victoryPoint + userData.defeatPoint) * 100;
        battleRecordText.text = $"{userData.victoryPoint}승 {userData.defeatPoint}패 ({winningRate}%)";
    }
}