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

    private void OnDisable()
    {
        // 코루틴 전체 스탑
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

    public void Update_Score(long _myRankingScore)
    {
        scoreChangeEffectImage.sprite = null; // 시작 이미지 넣어야 함
        scoreChangeEffectImage.gameObject.SetActive(true);
        // 이펙트 효과 넣어야 함

        

    }

    /// <summary>
    /// 기존 엠블렘이 없어지고 새로 생기는 이펙트
    /// </summary>
    protected IEnumerator IRankEmblem_DeactiveEffect(char _rank)
    {
        yield return null;


        // StartCoroutine(IRankEmblem_ActiveEffect(_rank));
    }

    protected IEnumerator IRankEmblem_ActiveEffect(char _rank)
    {
        yield return null;
    }

    protected IEnumerator IRankScoreEffect(int _score)
    {
        while(currRankingScore != _score)
        {
            if (currRankingScore > _score)
                currRankingScore--;
            else if (currRankingScore < _score)
                currRankingScore++;
            else
                break;

            rankingScoreText.text = currRankingScore.ToString();

            yield return new WaitForSeconds(0.05f);
        }

        currRankingScore = _score;
    }
}
