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
     
    public bool inabilityState = false;

    public ENUM_TEAM_TYPE teamType;

    public void Set_Character(ActiveCharacter _activeCharacter)
    {
        activeCharacter = _activeCharacter;
        activeCharacter.transform.parent = this.transform;
        activeCharacter.teamType = teamType;
        activeCharacter.Init();
    }
    public void Connect_Status(StatusWindowUI _statusWindowUI)
    {
        _statusWindowUI.Set_MaxHP(activeCharacter.curHP);
        activeCharacter.statusWindowUI = _statusWindowUI;
    }

    private void Update()
    {
        OnKeyboard(); // 디버깅용
    }

    // 디버깅용이니 쿨하게 다 때려박기
    private void OnKeyboard()
    {
        // 공격
        if (Input.GetKeyDown(KeyCode.N))
        {
            CharacterAttackParam attackParam = new CharacterAttackParam(ENUM_SKILL_TYPE.Knight_Attack1);
            PlayerCommand(ENUM_PLAYER_STATE.Attack, attackParam);
            activeCharacter.attackState = true;
        }

        if (Input.GetKeyUp(KeyCode.N))
        {
            activeCharacter.attackState = false;
        }

        // 스킬 1번
        if (Input.GetKeyDown(KeyCode.J))
        {
            CharacterSkillParam skillParam = new CharacterSkillParam(0);
            PlayerCommand(ENUM_PLAYER_STATE.Skill, skillParam);
        }

        // 스킬 2번
        if (Input.GetKeyDown(KeyCode.K))
        {
            CharacterSkillParam skillParam = new CharacterSkillParam(1);
            PlayerCommand(ENUM_PLAYER_STATE.Skill, skillParam);
        }

        // 점프
        if (Input.GetKeyDown(KeyCode.M))
        {
            PlayerCommand(ENUM_PLAYER_STATE.Jump);
        }

        moveDir = 0f; 

        // 이동
        if (Input.GetKey(KeyCode.LeftArrow)) moveDir = -1.0f;
        if (Input.GetKey(KeyCode.RightArrow)) moveDir = 1.0f;

        if (Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow))
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
        if (activeCharacter == null || activeCharacter.currState == ENUM_PLAYER_STATE.Hit)
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
            case ENUM_PLAYER_STATE.Skill:
                activeCharacter.Skill(param);
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
