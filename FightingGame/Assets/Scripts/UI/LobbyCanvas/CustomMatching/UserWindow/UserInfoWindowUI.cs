using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserInfoWindowUI : UIElement
{
    [SerializeField] Image rankEmblemImage;
    [SerializeField] Text userNicknameText;
    [SerializeField] Text ratingPointText;
    [SerializeField] Text winCountText;
    [SerializeField] Text loseCountText;
    [SerializeField] Text winningRateText;

    protected override void OnDisable()
    {
        if(!Managers.UI.popupCanvas.isFadeObjActiveState)
            Managers.Sound.Play_SFX(FGDefine.ENUM_SFX_TYPE.UI_Click_Cancel);

        base.OnDisable();
    }

    public void Open(DBUserData userData)
    {
        if (userData.victoryPoint + userData.defeatPoint == 0 && userData.ratingPoint == 1500)
        {
            rankEmblemImage.gameObject.SetActive(false);
            ratingPointText.text = "Unknown";
            winningRateText.text = "0%";
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

    public void Close()
    {
        if (!this.gameObject.activeSelf)
            return;

        gameObject.SetActive(false);
    }

    public void OnClick_Close() => Close();

    public override void OnClick_Exit()
    {
        base.OnClick_Exit();

        Close();
    }
}
