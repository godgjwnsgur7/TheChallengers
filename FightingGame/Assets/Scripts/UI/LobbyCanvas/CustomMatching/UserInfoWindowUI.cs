using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserInfoWindowUI : MonoBehaviour
{
    [SerializeField] Text userNicknameText;
    [SerializeField] Text RankingText;
    [SerializeField] Text winCountText;
    [SerializeField] Text loseCountText;
    [SerializeField] Text winningRateText;

    public void Open(string userNickname)
    {
        Debug.Log($"검색할 유저 닉네임 : {userNickname}");
        // userNickname을 확인해 검색
        // 정보가 없는 유저일 경우 알림창 팝업 띄우기

        gameObject.SetActive(true);
    }

    public void OnClick_Close()
    {
        gameObject.SetActive(false);
    }
}
