using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FGDefine;
using System;

/// <summary>
/// 클라이언트 1개일 때의 테스트 용도
/// </summary>
public class EnemyPlayer : MonoBehaviour
{
    public ActiveCharacter activeCharacter;

    public float moveDir = 0f;

    private void Awake()
    {
        if (activeCharacter == null)
        {
            activeCharacter = Managers.Resource.Instantiate("Character", this.transform).GetComponent<ActiveCharacter>();
            activeCharacter.tag = ENUM_TAG_TYPE.Enemy.ToString();
        }

        activeCharacter.Init();
    }

    private void Update()
    {
        OnKeyboard(); // 디버깅용
    }

    // 디버깅용이니 쿨하게 다 때려박기
    private void OnKeyboard()
    {
        // 공격
        if (Input.GetKeyDown(KeyCode.J))
        {
            PlayerCommand(ENUM_PLAYER_STATE.Attack);
        }

        // 셀프 히트ㅋㅋ
        if (Input.GetKeyDown(KeyCode.L))
        {
            // 데미지는 일단 10으로 ㅋㅋ
            PlayerCommand(ENUM_PLAYER_STATE.Hit, new CharacterAttackParam(10.0f));
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            PlayerCommand(ENUM_PLAYER_STATE.Jump);
        }

        moveDir = 0f;

        if (Input.GetKey(KeyCode.A)) moveDir = -1.0f;
        if (Input.GetKey(KeyCode.D)) moveDir = 1.0f;

        if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D))
            moveDir = 0f;

        if (moveDir == 0f)
        {
            if (activeCharacter.currState == ENUM_PLAYER_STATE.Move)
                PlayerCommand(ENUM_PLAYER_STATE.Idle);
        }
        else
        {
            PlayerCommand(ENUM_PLAYER_STATE.Move, new CharacterMoveParam(moveDir, Input.GetKey(KeyCode.LeftShift)));
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
            case ENUM_PLAYER_STATE.Jump:
                activeCharacter.Jump();
                break;
            case ENUM_PLAYER_STATE.Attack:
                activeCharacter.Attack(param);
                break;
            case ENUM_PLAYER_STATE.Expression:
                activeCharacter.Expression(param);
                break;
            case ENUM_PLAYER_STATE.Hit:
                activeCharacter.Hit(param);
                break;
            case ENUM_PLAYER_STATE.Die:
                activeCharacter.Die();
                break;
        }
    }
}
