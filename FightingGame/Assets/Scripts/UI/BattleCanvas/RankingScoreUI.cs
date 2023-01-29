using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RankingScoreUI : MonoBehaviour
{
    [SerializeField] Image rankEmblemImage;
    [SerializeField] Image scoreChangeEffectImage;

    [SerializeField] Text rankingScoreText;
    [SerializeField] Text ScoreChangeText;

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
        if (PhotonLogicHandler.Instance.CurrentLobbyType == ENUM_MATCH_TYPE.CUSTOM)
            return;

        char rank = RankingScoreOperator.Get_RankingEmblemChar(currRankingScore); 

        currRankingScore = _myRankingScore;
        rankingScoreText.text = _myRankingScore.ToString();
        rankEmblemImage.sprite = Managers.Resource.Load<Sprite>($"Art/Sprites/RankEmblem/RankEmblem_{rank}");

        this.gameObject.SetActive(true);
    }

    public void Update_Score(long changeRankingScore)
    {
        if (PhotonLogicHandler.Instance.CurrentLobbyType == ENUM_MATCH_TYPE.CUSTOM)
            return;

        scoreChangeEffectImage.gameObject.SetActive(true);

        ScoreChangeText.text = $"{changeRankingScore - currRankingScore}";

        char changeRank = RankingScoreOperator.Get_RankingEmblemChar(changeRankingScore);

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
            scoreChangeEffectImage.sprite = Managers.Resource.Load<Sprite>($"Art/Sprites/Result/Effect_ScoreNone");
        }

        scoreStatusEffectCoroutine = StartCoroutine(IFadeOut_ScoreChangeStatusEffect());
        scoreEffectCoroutine = StartCoroutine(IRankScoreEffect(changeRankingScore));

        if (RankingScoreOperator.Get_RankingEmblemChar(currRankingScore) != changeRank)
            emblemEffectCoroutine = StartCoroutine(IFadeIn_RankEmblem(changeRank));
    }

    /// <summary>
    /// 기존 엠블렘이 없어지고 새로 생기는 이펙트
    /// </summary>
    protected IEnumerator IFadeIn_RankEmblem(char _rank)
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

    protected IEnumerator IFadeOut_RankEmblem(char _rank)
    {
        float runTime = 0.5f;
        Color color = rankEmblemImage.color;
        rankEmblemImage.sprite = Managers.Resource.Load<Sprite>($"Art/Sprites/RankEmblem/RankEmblem_{_rank}");

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
