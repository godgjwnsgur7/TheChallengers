using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserInfoWindowUI : MonoBehaviour
{
    [SerializeField] Image rankEmblemImage;
    [SerializeField] Text userNicknameText;
    [SerializeField] Text ratingPointText;
    [SerializeField] Text winCountText;
    [SerializeField] Text loseCountText;
    [SerializeField] Text winningRateText;

    public void Open(DBUserData userData)
    {
        Debug.Log($"userData.ratingPoint : {userData.ratingPoint}");

        if (userData.victoryPoint + userData.defeatPoint == 0 && userData.ratingPoint == 1500)
        {
            rankEmblemImage.gameObject.SetActive(false);
            ratingPointText.text = "Unknown";
            winningRateText.text = "0%";

            return;
        }
        else
        {
            char rank = RankingScoreOperator.Get_RankingEmblemChar(userData.ratingPoint);
            rankEmblemImage.gameObject.SetActive(true);
            rankEmblemImage.sprite = Managers.Resource.Load<Sprite>($"Art/Sprites/RankEmblem/RankEmblem_{rank}");

            float victoryPoint = userData.victoryPoint, defeatPoint = userData.defeatPoint;
            float winningRate = victoryPoint / (victoryPoint + defeatPoint) * 100;
            winningRateText.text = $"{(int)winningRate}%";
            ratingPointText.text = $"{string.Format("{0:#,###}", userData.ratingPoint)}점";
        }

        userNicknameText.text = userData.nickname;
        winCountText.text = userData.victoryPoint.ToString();
        loseCountText.text = userData.defeatPoint.ToString();

        this.gameObject.SetActive(true);
    }

    private void Close()
    {
        if (!this.gameObject.activeSelf)
            return;

        gameObject.SetActive(false);
    }

    public void OnClick_Close() => Close();
}
