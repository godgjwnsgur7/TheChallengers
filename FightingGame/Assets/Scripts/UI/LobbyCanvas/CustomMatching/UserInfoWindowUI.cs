using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserInfoWindowUI : MonoBehaviour, IRoomPostProcess
{
    [SerializeField] Image rankEmblemImage;
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

        if (userData.victoryPoint == 0 && userData.defeatPoint == 0)
        {
            rankEmblemImage.gameObject.SetActive(false);
            ratingPointText.text = "Unknown";
        }
        else
        {
            char rank = RankingScoreOperator.Get_RankingEmblemChar(userData.ratingPoint);
            rankEmblemImage.gameObject.SetActive(true);
            rankEmblemImage.sprite = Managers.Resource.Load<Sprite>($"Art/Sprites/RankEmblem/RankEmblem_{rank}");
            ratingPointText.text = $"{userData.ratingPoint}점";
        }

        userNicknameText.text = userData.nickname;
        winCountText.text = userData.victoryPoint.ToString();
        loseCountText.text = userData.defeatPoint.ToString();
        
        long winningRate;
        if (userData.victoryPoint != 0)
            winningRate = userData.victoryPoint / (userData.victoryPoint + userData.defeatPoint) * 100;
        else
            winningRate = 0;
        
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
