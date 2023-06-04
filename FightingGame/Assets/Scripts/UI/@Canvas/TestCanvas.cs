using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FGDefine;
using System;

/// <summary>
/// 테스트 캔버스
/// </summary>
public class TestCanvas : BaseCanvas
{
    public float enemyScore;
    public float myScore;
    public float criteriaScore;
    public double expectedWinningRate;
    public double changedMyScore;

    public bool isWin;

    public void OnClick()
    {
        A();
    }

    public void A()
    {
        expectedWinningRate = 1 / (1 + Math.Pow(10, (enemyScore - myScore) / 400));

        changedMyScore = myScore + criteriaScore * ((isWin ? 1.0f : 0.0f) - expectedWinningRate);

        myScore = (float)changedMyScore;
    }
}
