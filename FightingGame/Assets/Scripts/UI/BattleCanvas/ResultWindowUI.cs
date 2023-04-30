using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FGDefine;

public class ResultWindowUI : MonoBehaviour
{
    [SerializeField] RankingScoreUI rankingScore;
    [SerializeField] Image backgroundImage;
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
        myScore = Managers.Network.Get_DBUserData(PhotonLogicHandler.IsMasterClient).ratingPoint;
        enemyScore = Managers.Network.Get_DBUserData(!PhotonLogicHandler.IsMasterClient).ratingPoint;

        countTime = 3;

        bool isMatching = PhotonLogicHandler.Instance.CurrentLobbyType == ENUM_MATCH_TYPE.RANDOM;

        if (isMatching)
        {
            countTime = 5;
            rankingScore.Open_Score(myScore);

            // 게임 중에 팅기거나 그랬을 때, 등의 예외상황 처리가 아직 안되어있음
            myScore = RankingScoreOperator.Operator_RankingScore(isDraw, isWin, myScore, enemyScore);
            Managers.Platform.DBUpdate(DB_CATEGORY.RatingPoint, myScore);
            if(!isDraw)
            {
                if(isWin)
                    Managers.Platform.DBUpdate(DB_CATEGORY.VictoryPoint, Managers.Network.Get_DBUserData(PhotonLogicHandler.IsMasterClient).victoryPoint + 1);
                else
                    Managers.Platform.DBUpdate(DB_CATEGORY.DefeatPoint, Managers.Network.Get_DBUserData(PhotonLogicHandler.IsMasterClient).defeatPoint + 1);
            }
        }

        if (isDraw)
        {
            backgroundImage.sprite = Managers.Resource.Load<Sprite>($"Art/Sprites/Result/Draw_Popup");
        }
        else
        {
            if (isWin)
            {
                backgroundImage.sprite = Managers.Resource.Load<Sprite>($"Art/Sprites/Result/Win_Popup");
            }
            else
            {
                backgroundImage.sprite = Managers.Resource.Load<Sprite>($"Art/Sprites/Result/Lose_Popup");
            }
        }

        notifyCountText.text = $"{countTime}초 뒤에 로비로 이동합니다.";
        gameObject.SetActive(true);

        Managers.Input.Destroy_InputKeyController();
        counterCorotine = StartCoroutine(INotifyTextCounter());
        
        if(isMatching)
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

        if (PhotonLogicHandler.IsMasterClient || PhotonLogicHandler.IsFullRoom == false)
        {
            PhotonLogicHandler.Instance.RequestGameEnd();

            Managers.UI.popupCanvas.Play_FadeOutEffect(GoTo_Lobby);
        }
        else
        {
            Managers.UI.popupCanvas.Play_FadeOutEffect();
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
