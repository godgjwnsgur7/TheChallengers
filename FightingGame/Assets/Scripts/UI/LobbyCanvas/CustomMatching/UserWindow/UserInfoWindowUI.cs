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
            ratingPointText.text = $"{string.Format("{0:#,###}", userData.ratingPoint)}점";
        }

        float victoryPoint = (float)userData.victoryPoint;
        float defeatPoint = (float)userData.defeatPoint;

        userNicknameText.text = userData.nickname;
        winCountText.text = userData.victoryPoint.ToString();
        loseCountText.text = userData.defeatPoint.ToString();

        float winningRate = victoryPoint / (victoryPoint + defeatPoint) * 100;
        
        winningRateText.text = $"{string.Format("{0:#,###}", winningRate)}%";

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
