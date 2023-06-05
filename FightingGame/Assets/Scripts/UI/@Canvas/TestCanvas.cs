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
        Vector2 movePos = Input.mousePosition;

        Debug.Log($"x : {movePos.x} / y ; {movePos.y}");
    }
}
