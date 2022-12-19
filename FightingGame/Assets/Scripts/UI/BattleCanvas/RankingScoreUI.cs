using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RankingScoreUI : MonoBehaviour
{
    [SerializeField] Image rankEmblemImage;
    [SerializeField] Image scoreChangeEffectImage; // 오브젝트 비활성화 상태

    [SerializeField] Text rankingScoreText;
    [SerializeField] Text ScoreChangeText; // 비어있는 상태

    long currRankingScore;

    Coroutine scoreEffectCoroutine = null;
    Coroutine emblemEffectCoroutine = null;

    private void OnDisable()
    {
        if (scoreEffectCoroutine != null)
            StopCoroutine(scoreEffectCoroutine);

        if (emblemEffectCoroutine != null)
            StopCoroutine(emblemEffectCoroutine);
    }

    public void Open_Score(long _myRankingScore)
    {
        if (Managers.Battle.isCustom)
            return;

        currRankingScore = _myRankingScore;
        rankingScoreText.text = _myRankingScore.ToString();
        rankEmblemImage.sprite = RankingScoreOperator.Get_RankingEmblemSprite(currRankingScore);
    
        this.gameObject.SetActive(true); 
    }

    public void Update_Score(long changeRankingScore)
    {
        if (Managers.Battle.isCustom)
            return;

        //scoreChangeEffectImage.sprite = Managers.Resource.Load<Sprite>($"Art/Sprites/Result/Effect_ScoreDown");
        scoreChangeEffectImage.gameObject.SetActive(true);

        FGDefine.ENUM_RANK_TYPE changeRanking = RankingScoreOperator.Check_RankScore(changeRankingScore);

        if (currRankingScore < changeRankingScore)
        {
            scoreChangeEffectImage.sprite = Managers.Resource.Load<Sprite>($"Art/Sprites/Result/Effect_ScoreUp");
            ScoreChangeText.text = "점수 Up!";
        }
        else if(currRankingScore > changeRankingScore)
        {
            scoreChangeEffectImage.sprite = Managers.Resource.Load<Sprite>($"Art/Sprites/Result/Effect_ScoreDown");
            ScoreChangeText.text = "점수 Down!";
        }
        else
        {
            //scoreChangeEffectImage.sprite = Managers.Resource.Load<Sprite>($"Art/Sprites/Result/Effect_ScoreDown");
            ScoreChangeText.text = "변동 없음!";
        }

        scoreEffectCoroutine = StartCoroutine(IRankScoreEffect(changeRankingScore));

        // 이펙트 효과 넣어야 함
        if (RankingScoreOperator.Check_RankScore(currRankingScore) != changeRanking)
            emblemEffectCoroutine = StartCoroutine(IRankEmblem_DeactiveEffect(changeRanking));
    }

    /// <summary>
    /// 기존 엠블렘이 없어지고 새로 생기는 이펙트
    /// </summary>
    protected IEnumerator IRankEmblem_DeactiveEffect(FGDefine.ENUM_RANK_TYPE _rank)
    {
        Color tempColor = rankEmblemImage.color;
        float runTime = 0.0f;
        float duration = 1.5f;

        while (rankEmblemImage.color.a > 0)
        {
            runTime += Time.deltaTime;
            tempColor.a = Mathf.Lerp(rankEmblemImage.color.a, 0, runTime / duration);
            rankEmblemImage.color = tempColor;
            yield return null;
        }
        tempColor.a = 0;
        rankEmblemImage.color = tempColor;

        if (emblemEffectCoroutine != null)
            emblemEffectCoroutine = StartCoroutine(IRankEmblem_ActiveEffect(_rank));
    }

    protected IEnumerator IRankEmblem_ActiveEffect(FGDefine.ENUM_RANK_TYPE _rank)
    {
        Color tempColor = rankEmblemImage.color;
        float runTime = 0.0f;
        float duration = 1.0f;

        rankEmblemImage.sprite = RankingScoreOperator.Get_RankingEmblemSprite(Managers.Battle.Get_RankDict(_rank));

        while (rankEmblemImage.color.a < 1)
        {
            runTime += Time.deltaTime;
            tempColor.a = Mathf.Lerp(rankEmblemImage.color.a, 1, runTime / duration);
            rankEmblemImage.color = tempColor;
            yield return null;
        }
        tempColor.a = 1;
        rankEmblemImage.color = tempColor;

        emblemEffectCoroutine = null;
    }

    protected IEnumerator IRankScoreEffect(long _score)
    {
        float runTime = 0.0f;
        float duration = 1.5f;
        long tempRankingScore = currRankingScore;

        while (runTime < duration)
        {
            runTime += Time.deltaTime;
            currRankingScore = (long)Mathf.Lerp(tempRankingScore, _score, runTime / duration);
            rankingScoreText.text = currRankingScore.ToString();

            yield return null;
        }

        currRankingScore = _score;
        rankingScoreText.text = currRankingScore.ToString();
        scoreEffectCoroutine = null;
    }
}
