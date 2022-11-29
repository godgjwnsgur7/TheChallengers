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
    char currRankEmblem = 'F';

    public void Set_Score(long _rankingScore)
    {
        currRankingScore = _rankingScore;
        rankingScoreText.text = _rankingScore.ToString();
    }

    public void Update_Score(int rankingScore)
    {


        // 커스텀 룸이면 리턴시켜 들어올 일도 없겠지만,


    }

    private void Set_RankEmblemImage(char _rankEmblem)
    {
        // Image를 리턴시켜야 할지도 모름 ㅋㅋ
        // 이펙트 때문.

        switch(_rankEmblem)
        {
            case 'S':

                break;
            case 'A':

                break;
            case 'B':

                break;
            case 'C':

                break;
            case 'D':

                break;
            case 'E':

                break;
            case 'F':

                break;
            default:
                
                break;
        }
    }

    private char Decide_Ranking(int _rankingScore)
    {
        // 구현해야 함

        currRankEmblem = 'F';

        return currRankEmblem;
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
