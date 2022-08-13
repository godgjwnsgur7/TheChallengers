using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FGDefine;
using System;

public class PlayerCharacter : MonoBehaviour
{
    public ActiveCharacter activeCharacter;
    [SerializeField] PlayerCamera playerCamera;

    public float moveDir = 0f;

    public bool inabilityState = false;

    public ENUM_TEAM_TYPE teamType;

    private void Start()
    {
        // #.Mobile Controller
        // Managers.Input.Action -= OnJoystick;
        // Managers.Input.Action += OnJoystick;
    }

    private void Update()
    {
        if (activeCharacter == null)
            return;
         
        if (!PhotonLogicHandler.IsMine(activeCharacter.ViewID))
            return;

        OnKeyboard(); // 디버깅용
    }

    public void Set_Character(ActiveCharacter _activeCharacter)
    {
        activeCharacter = _activeCharacter;
        activeCharacter.transform.parent = this.transform;
        activeCharacter.teamType = teamType;
        playerCamera.Init(activeCharacter.transform);
    }

    // 디버깅용이니 쿨하게 다 때려박기
    private void OnKeyboard()
    {// 공격
        if (Input.GetKeyDown(KeyCode.F))
        {
            CharacterAttackParam attackParam = new CharacterAttackParam(ENUM_SKILL_TYPE.Knight_Attack1);
            PlayerCommand(ENUM_PLAYER_STATE.Attack, attackParam);
            activeCharacter.attackState = true;
        }

        if (Input.GetKeyUp(KeyCode.F))
        {
            activeCharacter.attackState = false;
        }

        // 스킬 1번
        if (Input.GetKeyDown(KeyCode.R))
        {
            CharacterSkillParam skillParam = new CharacterSkillParam(0);
            PlayerCommand(ENUM_PLAYER_STATE.Skill, skillParam);
        }

        // 스킬 2번
        if (Input.GetKeyDown(KeyCode.T))
        {
            CharacterSkillParam skillParam = new CharacterSkillParam(1);
            PlayerCommand(ENUM_PLAYER_STATE.Skill, skillParam);
        }

        // 스킬 3번
        if (Input.GetKeyDown(KeyCode.Y))
        {
            CharacterSkillParam skillParam = new CharacterSkillParam(2);
            PlayerCommand(ENUM_PLAYER_STATE.Skill, skillParam);
        }

        // 점프
        if (Input.GetKeyDown(KeyCode.G))
        {
            PlayerCommand(ENUM_PLAYER_STATE.Jump);
        }

        moveDir = 0f;

        // 이동
        if (Input.GetKey(KeyCode.A)) moveDir = -1.0f;
        if (Input.GetKey(KeyCode.D)) moveDir = 1.0f;

        if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D))
            moveDir = 0f;

        if (moveDir == 0f)
        {
            if(activeCharacter.currState == ENUM_PLAYER_STATE.Move)
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