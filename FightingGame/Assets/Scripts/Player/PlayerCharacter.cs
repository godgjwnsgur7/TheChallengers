using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FGDefine;
using System;

public class PlayerCharacter : MonoBehaviour
{
    public ActiveCharacter activeCharacter;

    public Vector2 dirVec = Vector2.zero;

    public bool isRun = false;

    private void Awake()
    {
        activeCharacter = FindObjectOfType<ActiveCharacter>();
    }

    private void Start()
    {
    }

    private void OnMove()
    {
        dirVec = Managers.Input.touchPos;
        if (dirVec == Vector2.zero)
        {
            if (activeCharacter.currState != ENUM_PLAYER_STATE.Idle)
                PlayerCommand(ENUM_PLAYER_STATE.Idle);
        }
        else
        {
            PlayerCommand(ENUM_PLAYER_STATE.Move, new CharacterMoveParam(dirVec, isRun));
        }
    }

    public void RegisterCommand()
    {

    }

    public void UnregisterCommand()
    {

    }


    private void PlayerCommand(ENUM_PLAYER_STATE nextState, CharacterParam param = null)
    {
        if (activeCharacter == null)
            return;

        switch (nextState)
        {
            case ENUM_PLAYER_STATE.Idle:
                activeCharacter.Idle();
                break;
            case ENUM_PLAYER_STATE.Move:
                activeCharacter.Move(param);
                break;
        }
    }
}