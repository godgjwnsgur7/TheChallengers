using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RankingScoreUI : MonoBehaviour
{
    [SerializeField] Image rankEmblemImage;

    [SerializeField] Text rankingScoreText;
    [SerializeField] Text changeRankingScoreText;

    [SerializeField] Text winningRateText;
    [SerializeField] Text changeWinningRateText;

    long currRankingScore;
    long currWinningRate;
    char rank;

    Coroutine changeRankEmblemCoroutine;
    Coroutine changeRankingScoreCoroutine;
    Coroutine changeWinningRateCoroutine;

    private void OnDisable()
    {
        if (changeRankEmblemCoroutine != null)
            StopCoroutine(changeRankEmblemCoroutine);
        if (changeRankingScoreCoroutine != null)
            StopCoroutine(changeRankingScoreCoroutine);
        if (changeWinningRateCoroutine != null)
            StopCoroutine(changeWinningRateCoroutine);
    }

    public void Open_Score(long _myRankingScore, long _myWinningRate)
    {
        changeRankingScoreText.color = new Color(1, 1, 1, 0);
        changeWinningRateText.color = new Color(1, 1, 1, 0);

        currRankingScore = _myRankingScore;
        currWinningRate = _myWinningRate;

        if (_myRankingScore == 1500 && _myWinningRate == 0)
        {
            rankEmblemImage.gameObject.SetActive(false);
            rank = 'X';
            rankingScoreText.text = "None";
            winningRateText.text = "0%";
            return;
        }

        if(rankEmblemImage.gameObject.activeSelf == false)
            rankEmblemImage.gameObject.SetActive(true);
        rank = RankingScoreOperator.Get_RankingEmblemChar(currRankingScore);
        rankEmblemImage.sprite = Managers.Resource.Load<Sprite>($"Art/Sprites/RankEmblem/RankEmblem_{rank}");
        rankingScoreText.text = $"{string.Format("{0:#,###}", _myRankingScore)}";
        winningRateText.text = _myWinningRate.ToString() + "%";

        this.gameObject.SetActive(true);
    }

    public void Update_Score(long changeRankingScore, long changeWinningRate)
    {
        if (PhotonLogicHandler.Instance.CurrentLobbyType == ENUM_MATCH_TYPE.CUSTOM)
            return;

        bool isChangeScore = true, isChangeWinningRate = true;
        ColorUtility.TryParseHtmlString("#70CEDE", out Color pluscolor);
        ColorUtility.TryParseHtmlString("#C25556", out Color minusColor);

        char changeRank = RankingScoreOperator.Get_RankingEmblemChar(changeRankingScore);

        // 랭킹 점수 변동 체크
        if (currRankingScore < changeRankingScore)
        {
            changeRankingScoreText.text = "+" + (changeRankingScore - currRankingScore).ToString();
            changeRankingScoreText.color = pluscolor;
        }
        else if (currRankingScore > changeRankingScore)
        {
            changeRankingScoreText.text = "-" + (currRankingScore - changeRankingScore).ToString();
            changeRankingScoreText.color = minusColor;
        }
        else
            isChangeScore = false;

        // 승률 변동 체크
        if (currWinningRate < changeWinningRate)
        {
            changeWinningRateText.text = "+" + (changeWinningRate - currWinningRate).ToString() + "%";
            changeWinningRateText.color = pluscolor;
        }
        else if (currWinningRate > changeWinningRate)
        {
            changeWinningRateText.text = "-" + (currWinningRate - changeWinningRate).ToString() + "%";
            changeWinningRateText.color = minusColor;
        }
        else
            isChangeWinningRate = false;

        if (isChangeScore)
            changeRankingScoreCoroutine = StartCoroutine(IChange_RankingScore(changeRankingScore));

        if (isChangeWinningRate)
            changeWinningRateCoroutine = StartCoroutine(IChange_WinningRate(changeWinningRate));

        if (rank != changeRank)
           changeRankEmblemCoroutine = StartCoroutine(IChange_RankEmblem(changeRank));
    }

    /// <summary>
    /// 랭크 엠블렘 페이드아웃 -> 리소스 변경 -> 페이드인
    /// </summary>
    protected IEnumerator IChange_RankEmblem(char _rank)
    {
        float runTime = 0.0f;
        float duration = 1.0f;

        Color color = rankEmblemImage.color;
        while (runTime < duration)
        {
            runTime += Time.deltaTime;
            color.a = Mathf.Lerp(1.0f, 0.0f, runTime / duration);
            rankEmblemImage.color = color;
            yield return null;
        }

        runTime = 0.0f;
        color.a = 0.0f;
        rankEmblemImage.color = color;
        if (rankEmblemImage.gameObject.activeSelf == false)
            rankEmblemImage.gameObject.SetActive(true);
        rankEmblemImage.sprite = Managers.Resource.Load<Sprite>($"Art/Sprites/RankEmblem/RankEmblem_{_rank}");

        while (runTime < duration)
        {
            runTime += Time.deltaTime;
            color.a = Mathf.Lerp(0.0f, 1.0f, runTime / duration);
            rankEmblemImage.color = color;
            yield return null;
        }

        color.a = 1.0f;
        rankEmblemImage.color = color;
        changeRankEmblemCoroutine = null;
    }

    protected IEnumerator IChange_RankingScore(long _score)
    {
        float runTime = 0.0f;
        float duration = 1.0f;

        Color color = changeRankingScoreText.color;
        while (runTime < duration)
        {
            runTime += Time.deltaTime;
            color.a = Mathf.Lerp(0.0f, 1.0f, runTime / duration);
            changeRankingScoreText.color = color;
            yield return null;
        }

        runTime = 0.0f;
        color.a = 1.0f;
        changeRankingScoreText.color = color;
        long tempRankingScore = currRankingScore;

        while (runTime < duration)
        {
            runTime += Time.deltaTime;
            currRankingScore = (long)Mathf.Lerp(tempRankingScore, _score, runTime / duration);
            rankingScoreText.text = string.Format("{0:#,###}", currRankingScore);

            yield return null;
        }

        rankingScoreText.text = string.Format("{0:#,###}", _score);

        changeRankingScoreCoroutine = null;
    }

    protected IEnumerator IChange_WinningRate(long _winningRate)
    {
        float runTime = 0.0f;
        float duration = 1.0f;

        Color color = changeWinningRateText.color;
        while (runTime < duration)
        {
            runTime += Time.deltaTime;
            color.a = Mathf.Lerp(0.0f, 1.0f, runTime / duration);
            changeWinningRateText.color = color;
            yield return null;
        }

        runTime = 0.0f;
        color.a = 1.0f;
        changeWinningRateText.color = color;
        long tempRankingScore = currWinningRate;

        while (runTime < duration)
        {
            runTime += Time.deltaTime;
            currWinningRate = (long)Mathf.Lerp(tempRankingScore, _winningRate, runTime / duration);
            winningRateText.text = currWinningRate.ToString() + "%";

            yield return null;
        }

        winningRateText.text = _winningRate.ToString() + "%";

        changeWinningRateCoroutine = null;
    }
}
