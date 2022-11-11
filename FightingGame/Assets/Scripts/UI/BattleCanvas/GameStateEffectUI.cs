using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

[Serializable]
public enum ENUM_GAMESTATEEFFECT_TYPE
{
    ReadyAndStartTrigger = 0,
    TimeOutTrigger = 1,
}

public class GameStateEffectUI : MonoBehaviour
{
    [SerializeField] Animator gameStateEffectAnim;

    public void Play_GameStateEffect(ENUM_GAMESTATEEFFECT_TYPE effectType)
    {
        this.gameObject.SetActive(true);
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
    /// 타임아웃 이펙트가 끝나면 호출
    /// </summary>
    public void AnimEvent_TimeOut()
    {
        if (PhotonLogicHandler.IsMasterClient)
        {
            // 타임아웃 시, 미구현
        }

        this.gameObject.SetActive(false);
    }
}
