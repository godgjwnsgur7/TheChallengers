using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

[Serializable]
public enum ENUM_GAMESTATEEFFECT_TYPE
{
    GameStartTrigger = 0,
    TimeOutTrigger = 1,
    WinTrigger = 2,
    LoseTrigger = 3,
    DrawTrigger = 4,
}

public class GameStateEffectUI : MonoBehaviour
{
    [SerializeField] Animator gameStateEffectAnim;

    ENUM_GAMESTATEEFFECT_TYPE currEffectType;

    public void Play_GameStateEffect(ENUM_GAMESTATEEFFECT_TYPE effectType)
    {
        this.gameObject.SetActive(true);
        currEffectType = effectType;
        gameStateEffectAnim.SetTrigger($"{effectType}");
    }
    
    /// <summary>
    /// 게임시작 이펙트가 끝나면 호출
    /// </summary>
    public void AnimEvent_GameStart()
    {
        Managers.Battle.GameStart();

        this.gameObject.SetActive(false);
    }

    /// <summary>
    /// 게임종료 관련 이펙트가 끝나면 호출
    /// </summary>
    public void AnimEvent_EndGame()
    {
        bool isDraw = (currEffectType == ENUM_GAMESTATEEFFECT_TYPE.DrawTrigger);
        bool isWin = (currEffectType == ENUM_GAMESTATEEFFECT_TYPE.WinTrigger);

        Managers.UI.currCanvas.GetComponent<BattleCanvas>().EndGame(isDraw, isWin);

        this.gameObject.SetActive(false);
    }
}
