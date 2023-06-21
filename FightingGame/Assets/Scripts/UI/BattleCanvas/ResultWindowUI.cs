using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FGDefine;
using System;

public class ResultWindowUI : MonoBehaviour
{
    [SerializeField] RankingScoreUI rankingScore;

    [SerializeField] Image resultLogoImage;

    [SerializeField] Text matchTypeText;
    [SerializeField] Text playTimeText;

    [SerializeField] Text notifyCountText;
    [SerializeField] Text timerText;

    Coroutine counterCorotine;

    int timeCount;
    long myScore;
    long myWinningRate;
    long enemyScore;

    private void OnDisable()
    {
        if (counterCorotine != null)
            StopCoroutine(counterCorotine);
    }

    public void Open(bool isDraw, bool isWin = true)
    {
        // DB 관련 세팅 (점수, 승률, 랭크 등)
        DBUserData myDBData = Managers.Network.Get_DBUserData(PhotonLogicHandler.IsMasterClient);
        if(myDBData.ratingPoint == 1500 && myDBData.victoryPoint + myDBData.defeatPoint == 0)
            myWinningRate = 0;
        else
        {
            float victoryPoint = myDBData.victoryPoint, defeatPoint = myDBData.defeatPoint;
            float tempMyWinningRate = victoryPoint / (victoryPoint + defeatPoint) * 100;
            myWinningRate = (long)tempMyWinningRate;
        }
            
        enemyScore = Managers.Network.Get_DBUserData(!PhotonLogicHandler.IsMasterClient).ratingPoint;
        myScore = myDBData.ratingPoint;
        rankingScore.Open_Score(myScore, myWinningRate);

        // 플레이 타임 세팅
        string[] timer = timerText.text.Split(':');
        int remainingTimeSecond = int.Parse(timer[0]) * 60 + int.Parse(timer[1]);
        int playTimeSecond = (int)Managers.Data.gameInfo.maxGameRunTime - remainingTimeSecond;
        playTimeText.text = String.Format("{0:00} : {1:00}", playTimeSecond / 60, playTimeSecond % 60);

        timeCount = 3;

        // 매치 타입에 따른 세팅
        bool isMatching = PhotonLogicHandler.Instance.CurrentLobbyType == ENUM_MATCH_TYPE.RANDOM;
        matchTypeText.text = isMatching ? "RANDOM MATCH" : "1 ON 1 MATCH";
        if (isMatching)
        {
            timeCount = 5;
            
            myScore = RankingScoreOperator.Operator_RankingScore(isDraw, isWin, myScore, enemyScore);
            Managers.Platform.DBUpdate(DB_CATEGORY.RatingPoint, myScore);
            if(!isDraw)
            {
                float victoryPoint = myDBData.victoryPoint, defeatPoint = myDBData.defeatPoint;
                float tempMyWinningRate;

                if (isWin)
                {
                    Managers.Platform.DBUpdate(DB_CATEGORY.VictoryPoint, Managers.Network.Get_DBUserData(PhotonLogicHandler.IsMasterClient).victoryPoint + 1);
                    tempMyWinningRate = (victoryPoint + 1) / (victoryPoint + defeatPoint + 1) * 100;
                }
                else
                {
                    Managers.Platform.DBUpdate(DB_CATEGORY.DefeatPoint, Managers.Network.Get_DBUserData(PhotonLogicHandler.IsMasterClient).defeatPoint + 1);
                    tempMyWinningRate = (victoryPoint) / (victoryPoint + defeatPoint + 1) * 100;
                }

                myWinningRate = (long)tempMyWinningRate;
            }
        }

        // UI 이미지 세팅
        if (isDraw)
        {
            resultLogoImage.sprite = Managers.Resource.Load<Sprite>($"Art/Sprites/Result_Draw");
            Managers.Sound.Play_SFX(ENUM_SFX_TYPE.UI_Result_Draw);
        } 
        else
        {
            if (isWin)
            {
                resultLogoImage.sprite = Managers.Resource.Load<Sprite>($"Art/Sprites/Result_Victory");
                Managers.Sound.Play_SFX(ENUM_SFX_TYPE.UI_Result_Victory);
            }
            else
            {
                resultLogoImage.sprite = Managers.Resource.Load<Sprite>($"Art/Sprites/Result_Defeat");
                Managers.Sound.Play_SFX(ENUM_SFX_TYPE.UI_Result_Defeat);
            }
        }

        notifyCountText.text = $"{timeCount}초 뒤에 로비로 이동합니다.";
        gameObject.SetActive(true);

        Managers.Input.Deactive_InputKeyController();
        counterCorotine = StartCoroutine(INotifyTextCounter());
        
        if(isMatching)
            StartCoroutine(IWaitUpdateScore(0.5f)); // 0.5초 뒤에 업데이트 시작
    }

    public void Close()
    {
    }

    protected IEnumerator INotifyTextCounter()
    {
        while (timeCount != 0)
        {
            notifyCountText.text = $"{timeCount}초 뒤에 로비로 이동합니다.";
            yield return new WaitForSeconds(1f);
            timeCount--;
        }
        
        notifyCountText.text = $"{timeCount}초 뒤에 로비로 이동합니다.";
        counterCorotine = null;

        Managers.Network.EndGame_GoToLobby();
    }

    protected IEnumerator IWaitUpdateScore(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);

        rankingScore.Update_Score(myScore, myWinningRate);
    }
}
