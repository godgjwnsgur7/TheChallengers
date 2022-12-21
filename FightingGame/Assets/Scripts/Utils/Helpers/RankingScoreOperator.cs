using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using FGDefine;

public class RankingScoreOperator : MonoBehaviour
{
    // 같은 점수가 대결 시에 기준점수만큼 승자와 패자의 점수 차이가 남
    static float criteriaScore = 50;

    public static char Get_RankingEmblemChar(long score)
    {
        char emblemChar;

        if (score < 1350) emblemChar = 'F';
        else if (score < 1500) emblemChar = 'E';
        else if (score < 1600) emblemChar = 'D';
        else if (score < 1700) emblemChar = 'C';
        else if (score < 1800) emblemChar = 'B';
        else if (score < 2000) emblemChar = 'A';
        else emblemChar = 'S';

        return emblemChar;
    }

    public static long Operator_RankingScore(bool isDraw, bool isWin, long myScore, long enemyScore)
    {
        if (isDraw)
        {
            return myScore;
        }

        double expectedWinningRate = 1 / (1 + Math.Pow(10, (enemyScore - myScore) / 100));
        Debug.Log($"계산된 승률 : {expectedWinningRate}");

        double changedMyScore = myScore + criteriaScore * ((isWin ? 1.0f : 0.0f) - expectedWinningRate);
        Debug.Log($"변경될 점수 : {changedMyScore}");
        Debug.Log($"형변환된 점수 : {(long)changedMyScore}");

        return (long)changedMyScore;
    }
}