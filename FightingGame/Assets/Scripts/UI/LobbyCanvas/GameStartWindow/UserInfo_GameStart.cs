using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FGDefine;

public class UserInfo_GameStart : MonoBehaviour
{
    [SerializeField] Image ratingEmblemImage;
    [SerializeField] Text ratingPointText;
    [SerializeField] Text battleRecordText;
    [SerializeField] Text characterNameText;

    public bool IsInit
    {
        get;
        private set;
    }

    private void Awake()
    {
        IsInit = false;
    }

    public void Set_UserData(DBUserData _userData)
    {
        if (_userData.victoryPoint +_userData.defeatPoint == 0 && _userData.ratingPoint == 1500)
        {
            ratingEmblemImage.gameObject.SetActive(false);
            battleRecordText.text = "0승 0패 (0%)";
            ratingPointText.text = "Unknown";
        }
        else
        {
            ratingEmblemImage.gameObject.SetActive(true);
            char rank = RankingScoreOperator.Get_RankingEmblemChar(_userData.ratingPoint);
            ratingEmblemImage.sprite = Managers.Resource.Load<Sprite>($"Art/Sprites/RankEmblem/RankEmblem_{rank}");

            long winningRate = _userData.victoryPoint / (_userData.victoryPoint + _userData.defeatPoint) * 100;

            battleRecordText.text = $"{_userData.victoryPoint}승 {_userData.defeatPoint}패 ({winningRate}%)";
            ratingPointText.text = $"{string.Format("{0:#,###}", _userData.ratingPoint)}점";
        }
    }

    public void Open(ENUM_CHARACTER_TYPE _charType)
    {
        characterNameText.text = _charType.ToString().ToUpper();

        gameObject.SetActive(true);
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }
}
