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
        scoreChangeEffectImage.sprite = null; // 시작 이미지 넣어야 함
        scoreChangeEffectImage.gameObject.SetActive(true);

        if (currRankingScore < changeRankingScore)
        {
            // 점수가 높아짐 - 랭크 변경 체크
            
        }
        else if(currRankingScore > changeRankingScore)
        {
            // 점수가 낮아짐 - 랭크 변경 체크

        }
        else
        {
            // 변경점 없음 - 이미지만 세팅

        }

        scoreEffectCoroutine = StartCoroutine(IRankScoreEffect(changeRankingScore));

        
        // 이펙트 효과 넣어야 함

        

    }

    /// <summary>
    /// 기존 엠블렘이 없어지고 새로 생기는 이펙트
    /// </summary>
    protected IEnumerator IRankEmblem_DeactiveEffect(char _rank)
    {
        yield return null;



        // if(emblemEffectCoroutine != null)
        //     StartCoroutine(IRankEmblem_ActiveEffect(_rank));
    }

    protected IEnumerator IRankEmblem_ActiveEffect(char _rank)
    {
        yield return null;

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
