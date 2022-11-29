using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RankingScoreOperator : MonoBehaviour
{
    public static Sprite Get_RankingEmblemSprite(long score)
    {
        char emblemChar;

        if (score < 1350) emblemChar = 'F';
        else if (score < 1500) emblemChar = 'E';
        else if (score < 1600) emblemChar = 'D';
        else if (score < 1700) emblemChar = 'C';
        else if (score < 1800) emblemChar = 'B';
        else if (score < 2000) emblemChar = 'A';
        else emblemChar = 'S';

        return Managers.Resource.Load<Sprite>($"Art/Sprites/RankEmblem/RankEmblem_{emblemChar}");
    }

    public static long Operator_RankingScore(bool isDraw, bool isWin, long myScore, long enemyScore)
    {
        // 임시로 100을 무조건 +시키고 뱉음
        myScore += 100;
        
        // DB에 정보 변경을 여기서 시키는 게 낫지 않을까?

        return myScore;
    }
}