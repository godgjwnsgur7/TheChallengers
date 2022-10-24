using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserInfoWindowUI : MonoBehaviour, IRoomPostProcess
{
    [SerializeField] Text userNicknameText;
    [SerializeField] Text ratingPointText;
    [SerializeField] Text winCountText;
    [SerializeField] Text loseCountText;
    [SerializeField] Text winningRateText;

    bool isMasterProfile; // 선택한 프로필
    bool isOpen = false;

    public void Open_Request(bool _isMasterProfile)
    {
        if (this.gameObject.activeSelf)
            return;

        isMasterProfile = _isMasterProfile;

        this.RegisterRoomCallback();

        PhotonLogicHandler.Instance.RequestEveryPlayerProperty();
    }

    public void Open(DBUserData userData)
    {
        isOpen = true;

        userNicknameText.text = userData.nickname;
        ratingPointText.text = $"{userData.ratingPoint}점";
        winCountText.text = userData.victoryPoint.ToString();
        loseCountText.text = userData.defeatPoint.ToString();
        long winningRate = userData.victoryPoint / (userData.victoryPoint + userData.defeatPoint) * 100;
        winningRateText.text = $"{winningRate}%";

        this.gameObject.SetActive(true);
    }

    private void Close()
    {
        if (!this.gameObject.activeSelf)
            return;

        this.UnregisterRoomCallback();
        gameObject.SetActive(false);

        userNicknameText.text = "유저 닉네임";
        ratingPointText.text = "0000점";
        winCountText.text = "00";
        loseCountText.text = "00";
        winningRateText.text = "0%";

        isOpen = false;
    }

    public void OnUpdateRoomProperty(CustomRoomProperty property) { }
    public void OnUpdateRoomPlayerProperty(CustomPlayerProperty property)
    {
        if (isMasterProfile != property.isMasterClient || isOpen)
            return;

        Open(property.data);
    }

    public void OnClick_Close() => Close();
}
