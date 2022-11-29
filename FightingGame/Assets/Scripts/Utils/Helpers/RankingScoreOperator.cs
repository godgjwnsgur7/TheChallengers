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


        // 임시로 1500만 뱉음
        return 1500;
    }
}