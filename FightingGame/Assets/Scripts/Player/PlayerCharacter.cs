using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FGDefine;
using System;

public class PlayerCharacter : MonoBehaviour
{
    public ActiveCharacter activeCharacter;

    public Vector2 dirVec = Vector2.zero;

    public Vector2 _dirVec2 = Vector2.zero;

    public bool inabilityState = false;

    private void Awake()
    {
        if(activeCharacter == null)
            activeCharacter = FindObjectOfType<ActiveCharacter>();
        activeCharacter.Init();
    }

    private void Start()
    {
        // #.Mobile Controller
        // Managers.Input.Action -= OnJoystick;
        // Managers.Input.Action += OnJoystick;
    }

    private void Update()
    {
        OnKeyboard(); // 디버깅용
    }

    private void OnJoystick(ENUM_INPUT_TYPE evt)
    {
        dirVec = Managers.Input.touchPos;

        if (dirVec == Vector2.zero)
        {
            if (activeCharacter.currState != ENUM_PLAYER_STATE.Idle)
                PlayerCommand(ENUM_PLAYER_STATE.Idle);
        }
        else
        {
            PlayerCommand(ENUM_PLAYER_STATE.Move, new CharacterMoveParam(dirVec, Input.GetKey(KeyCode.LeftShift)));
        }
    }
    
    private void OnKeyboard()
    {
        // 디버깅용이니 쿨하게 다 때려박기
        _dirVec2 = Vector2.zero;

        if (Input.GetKey(KeyCode.W)) _dirVec2.y = 1.0f;
        if (Input.GetKey(KeyCode.A)) _dirVec2.x = -1.0f;
        if (Input.GetKey(KeyCode.S)) _dirVec2.y = -1.0f;
        if (Input.GetKey(KeyCode.D)) _dirVec2.x = 1.0f;

        if (Input.GetKey(KeyCode.J))
        {
            PlayerCommand(ENUM_PLAYER_STATE.Attack);
            return;
        }

        if (_dirVec2 == Vector2.zero)
        {
            if(activeCharacter.currState != ENUM_PLAYER_STATE.Idle)
                PlayerCommand(ENUM_PLAYER_STATE.Idle);
        }
        else
        {
            PlayerCommand(ENUM_PLAYER_STATE.Move, new CharacterMoveParam(_dirVec2, Input.GetKey(KeyCode.LeftShift)));
        }
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
            case ENUM_PLAYER_STATE.Attack:
                activeCharacter.Attack(param);
                break;
            case ENUM_PLAYER_STATE.Expression:
                activeCharacter.Expression(param);
                break;
            case ENUM_PLAYER_STATE.Die:
                activeCharacter.Die();
                break;
        }
    }
}