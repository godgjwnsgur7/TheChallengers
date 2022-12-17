using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using FGDefine;

public class RankingScoreOperator : MonoBehaviour
{
    public static Sprite Get_RankingEmblemSprite(long score)
    {
        /*char emblemChar;

        if (score < 1350) emblemChar = 'F';
        else if (score < 1500) emblemChar = 'E';
        else if (score < 1600) emblemChar = 'D';
        else if (score < 1700) emblemChar = 'C';
        else if (score < 1800) emblemChar = 'B';
        else if (score < 2000) emblemChar = 'A';
        else emblemChar = 'S';*/

        string emblemstring = Enum.GetName(typeof(ENUM_RANK_TYPE), Check_RankScore(score));

        return Managers.Resource.Load<Sprite>($"Art/Sprites/RankEmblem/RankEmblem_{emblemstring}");
    }

    public static ENUM_RANK_TYPE Check_RankScore(long rankScore)
    {
        if (rankScore < Managers.Battle.Get_RankDict(ENUM_RANK_TYPE.E))
            return ENUM_RANK_TYPE.F;
        else if (rankScore < Managers.Battle.Get_RankDict(ENUM_RANK_TYPE.D))
            return ENUM_RANK_TYPE.E;
        else if (rankScore < Managers.Battle.Get_RankDict(ENUM_RANK_TYPE.C))
            return ENUM_RANK_TYPE.D;
        else if (rankScore < Managers.Battle.Get_RankDict(ENUM_RANK_TYPE.B))
            return ENUM_RANK_TYPE.C;
        else if (rankScore < Managers.Battle.Get_RankDict(ENUM_RANK_TYPE.A))
            return ENUM_RANK_TYPE.B;
        else if (rankScore < Managers.Battle.Get_RankDict(ENUM_RANK_TYPE.S))
            return ENUM_RANK_TYPE.A;
        else 
            return ENUM_RANK_TYPE.S;
    }

    public static long Operator_RankingScore(bool isDraw, bool isWin, long myScore, long enemyScore)
    {
        if (isDraw)
        {
            // 무승부
        }
        else
        {
            if (isWin)
            {
                // 승리
                myScore += 150;
            }
            else
            {
                // 패배
                myScore -= 150;
            }
        }

        if (myScore < 500)
            myScore = 500;
        
        return myScore;
    }
}