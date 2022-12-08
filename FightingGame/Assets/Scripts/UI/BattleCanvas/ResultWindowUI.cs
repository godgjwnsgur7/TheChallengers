using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FGDefine;

public class ResultWindowUI : MonoBehaviour
{
    [SerializeField] Text resultText;
    
    [SerializeField] RankingScoreUI rankingScore;
    
    [SerializeField] Text notifyCountText;

    Coroutine counterCorotine;

    int countTime;
    long myScore;
    long enemyScore;

    private void OnDisable()
    {
        if (counterCorotine != null)
            StopCoroutine(counterCorotine);
    }

    public void Open(bool isDraw, bool isWin = true)
    {
        if(Managers.Battle.myDBData == null)
        {
            Debug.Log("얘 널인데 왜 널이냐?... 셋팅 하잖아 시팔?");
        }
        myScore = Managers.Battle.myDBData.ratingPoint;
        enemyScore = Managers.Battle.enemyScore;

        countTime = 3;

        if(!Managers.Battle.isCustom)
        {
            countTime = 5;
            rankingScore.Open_Score(myScore);

            // 게임 중에 팅기거나 그랬을 때, 등의 예외상황 처리가 아직 안되어있음
            myScore = RankingScoreOperator.Operator_RankingScore(isDraw, isWin, myScore, enemyScore);
            Managers.Platform.DBUpdate(DB_CATEGORY.RatingPoint, myScore);
            if(!isDraw)
            {
                if(isWin)
                    Managers.Platform.DBUpdate(DB_CATEGORY.VictoryPoint, Managers.Battle.myDBData.victoryPoint + 1);
                else
                    Managers.Platform.DBUpdate(DB_CATEGORY.DefeatPoint, Managers.Battle.myDBData.defeatPoint + 1);
            }
        }

        if (isDraw)
        {
            resultText.text = "무승부!";

        }
        else
        {
            if (isWin)
            {
                resultText.text = "승리!";

            }
            else
            {
                resultText.text = "패배!";

            }
        }

        notifyCountText.text = $"{countTime}초 뒤에 로비로 이동합니다.";
        gameObject.SetActive(true);

        counterCorotine = StartCoroutine(INotifyTextCounter());
        StartCoroutine(IWaitUpdateScore(0.5f)); // 0.5초 뒤에 업데이트 시작
    }

    public void Close()
    {
    }

    protected IEnumerator INotifyTextCounter()
    {
        while (countTime != 0)
        {
            notifyCountText.text = $"{countTime}초 뒤에 로비로 이동합니다.";
            yield return new WaitForSeconds(1f);
            countTime--;
        }
        
        notifyCountText.text = $"{countTime}초 뒤에 로비로 이동합니다.";
        counterCorotine = null;

        if (PhotonLogicHandler.IsMasterClient)
        {
            PhotonLogicHandler.Instance.GameEnd();

            Managers.UI.popupCanvas.Play_FadeInEffect(GoTo_Lobby);
        }
        else
        {
            Managers.UI.popupCanvas.Play_FadeInEffect();
        }
    }

    public void GoTo_Lobby()
    {
        if (PhotonLogicHandler.IsMasterClient)
        {
            StartCoroutine(IWaitGoToLobby(1.0f));
        }
    }

    protected IEnumerator IWaitUpdateScore(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);

        rankingScore.Update_Score(myScore);
    }

    protected IEnumerator IWaitGoToLobby(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);

        Managers.Scene.Sync_LoadScene(ENUM_SCENE_TYPE.Lobby);
    }
}
