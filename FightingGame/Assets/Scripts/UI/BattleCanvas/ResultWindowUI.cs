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

    private void OnDisable()
    {
        if (counterCorotine != null)
            StopCoroutine(counterCorotine);
    }

    public void Open(bool isDraw, bool isWin = true)
    {
        rankingScore.Set_Score(1500); // 자기 점수로 넣어야 함

        // 자신의 점수와 상대의 점수
        int point = RankingScoreOperator.Operator_RankingScore(isDraw, isWin, 1500, 1500);

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

        notifyCountText.text = "3초 뒤에 로비로 이동합니다.";
        gameObject.SetActive(true);

        counterCorotine = StartCoroutine(INotifyTextCounter());
    }

    public void Close()
    {
    }

    protected IEnumerator INotifyTextCounter()
    {
        int count = 3;

        while (count != 0)
        {
            notifyCountText.text = $"{count}초 뒤에 로비로 이동합니다.";
            yield return new WaitForSeconds(1f);
            count--;
        }
        
        notifyCountText.text = $"{count}초 뒤에 로비로 이동합니다.";
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

    protected IEnumerator IWaitGoToLobby(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);

        Managers.Scene.Sync_LoadScene(ENUM_SCENE_TYPE.Lobby);
    }
}
