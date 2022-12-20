using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RankingScoreUI : MonoBehaviour
{
    [SerializeField] Image rankEmblemImage;
    [SerializeField] Image scoreChangeEffectImage;

    [SerializeField] Text rankingScoreText;
    [SerializeField] Text ScoreChangeText; // 비어있는 상태

    long currRankingScore;

    Coroutine scoreStatusEffectCoroutine = null;
    Coroutine scoreEffectCoroutine = null;
    Coroutine emblemEffectCoroutine = null;

    private void OnEnable()
    {
        scoreChangeEffectImage.color = new Color(1, 1, 1, 0);

    }

    private void OnDisable()
    {
        scoreChangeEffectImage.color = new Color(1, 1, 1, 0);

        if (scoreEffectCoroutine != null)
            StopCoroutine(scoreEffectCoroutine);

        if (emblemEffectCoroutine != null)
            StopCoroutine(emblemEffectCoroutine);

        if (scoreStatusEffectCoroutine != null)
            StopCoroutine(scoreStatusEffectCoroutine);
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

        scoreChangeEffectImage.gameObject.SetActive(true);

        FGDefine.ENUM_RANK_TYPE changeRanking = RankingScoreOperator.Check_RankScore(changeRankingScore);

        if (currRankingScore < changeRankingScore)
        {
            scoreChangeEffectImage.sprite = Managers.Resource.Load<Sprite>($"Art/Sprites/Result/Effect_ScoreUp");
        }
        else if (currRankingScore > changeRankingScore)
        {
            scoreChangeEffectImage.sprite = Managers.Resource.Load<Sprite>($"Art/Sprites/Result/Effect_ScoreDown");
        }
        else
        {
            // 이미지 없음
            // scoreChangeEffectImage.sprite = Managers.Resource.Load<Sprite>($"Art/Sprites/Result/Effect_ScoreDown");
        }

        scoreStatusEffectCoroutine = StartCoroutine(IFadeOut_ScoreChangeStatusEffect());
        scoreEffectCoroutine = StartCoroutine(IRankScoreEffect(changeRankingScore));

        // 이펙트 효과 넣어야 함
        if (RankingScoreOperator.Check_RankScore(currRankingScore) != changeRanking)
            emblemEffectCoroutine = StartCoroutine(IFadeIn_RankEmblem(changeRanking));
    }

    /// <summary>
    /// 기존 엠블렘이 없어지고 새로 생기는 이펙트
    /// </summary>
    protected IEnumerator IFadeIn_RankEmblem(FGDefine.ENUM_RANK_TYPE _rank)
    {
        float runTime = 0.5f;
        Color color = rankEmblemImage.color;

        while (color.a < 0.1f)
        {
            color.a -= Time.deltaTime / runTime;
            scoreChangeEffectImage.color = color;
            yield return null;
        }

        color.a = 0.0f;
        scoreChangeEffectImage.color = color;

        if (emblemEffectCoroutine != null)
            emblemEffectCoroutine = StartCoroutine(IFadeOut_RankEmblem(_rank));
    }

    protected IEnumerator IFadeOut_RankEmblem(FGDefine.ENUM_RANK_TYPE _rank)
    {
        float runTime = 0.5f;
        Color color = rankEmblemImage.color;
        rankEmblemImage.sprite = RankingScoreOperator.Get_RankingEmblemSprite(Managers.Battle.Get_RankDict(_rank));

        while (color.a < 0.9f)
        {
            color.a += Time.deltaTime / runTime;
            scoreChangeEffectImage.color = color;
            yield return null;
        }

        color.a = 1.0f;
        scoreChangeEffectImage.color = color;
        emblemEffectCoroutine = null;
    }

    protected IEnumerator IRankScoreEffect(long _score)
    {
        float runTime = 0.0f;
        float duration = 1.0f;
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

    protected IEnumerator IFadeOut_ScoreChangeStatusEffect()
    {
        float fadeTime = 1.0f;

        Color color = scoreChangeEffectImage.color;
        while (color.a < 0.9f)
        {
            color.a += Time.deltaTime / fadeTime;
            scoreChangeEffectImage.color = color;
            yield return null;
        }

        scoreChangeEffectImage.color = new Color(1, 1, 1, 1);
        scoreStatusEffectCoroutine = null;
    }
}
