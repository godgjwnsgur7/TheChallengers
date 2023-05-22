using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FGDefine;
using System;

/// <summary>
/// 클라이언트 1개일 때의 테스트용 스크립트
/// </summary>
public class EnemyPlayer : MonoBehaviour
{
    public ActiveCharacter activeCharacter;

    public ENUM_TEAM_TYPE teamType;
    public float moveDir;
    public bool inabilityState = false;

    private void Update()
    {
        if(activeCharacter != null)
            OnKeyboard(); // 디버깅용
    }

    public void Init(BaseMap currMap, ENUM_CHARACTER_TYPE _summonCharType)
    {
        teamType = ENUM_TEAM_TYPE.Red;
        Vector2 summonPosVec = currMap.redTeamSpawnPoint.position;

        Summon_Character(_summonCharType, summonPosVec);
    }

    public void Summon_Character(ENUM_CHARACTER_TYPE _charType, Vector2 _summonPosVec)
    {
        activeCharacter = Managers.Resource.Instantiate($"{_charType}", _summonPosVec).GetComponent<ActiveCharacter>();
        activeCharacter.transform.parent = this.transform;

        activeCharacter.Init();
        activeCharacter.Skills_Pooling();
        activeCharacter.Set_Character(teamType);
    }

    // 디버깅용이니 쿨하게 다 때려박기
    private void OnKeyboard()
    {
        // 공격
        if (Input.GetKeyDown(KeyCode.N))
        {
            CharacterAttackParam attackParam = new CharacterAttackParam(ENUM_ATTACKOBJECT_NAME.Knight_Attack1, activeCharacter.reverseState);
            PlayerCommand(ENUM_PLAYER_STATE.Attack, attackParam);
            activeCharacter.Change_AttackState(true);
        }

        if (Input.GetKeyUp(KeyCode.N))
        {
            activeCharacter.Change_AttackState(false);
        }

        // 스킬 1번
        if (Input.GetKeyDown(KeyCode.H))
        {
            CharacterSkillParam skillParam = new CharacterSkillParam(1);
            PlayerCommand(ENUM_PLAYER_STATE.Skill, skillParam);
        }

        // 스킬 2번
        if (Input.GetKeyDown(KeyCode.J))
        {
            CharacterSkillParam skillParam = new CharacterSkillParam(2);
            PlayerCommand(ENUM_PLAYER_STATE.Skill, skillParam);
        }

        // 스킬 3번
        if (Input.GetKeyDown(KeyCode.K))
        {
            CharacterSkillParam skillParam = new CharacterSkillParam(3);
            PlayerCommand(ENUM_PLAYER_STATE.Skill, skillParam);
        }

        // 스킬 3번
        if (Input.GetKeyDown(KeyCode.L))
        {
            CharacterSkillParam skillParam = new CharacterSkillParam(4);
            PlayerCommand(ENUM_PLAYER_STATE.Skill, skillParam);
        }

        // 대쉬
        if(Input.GetKeyDown(KeyCode.B))
        {
            PlayerCommand(ENUM_PLAYER_STATE.Dash);
        }

        // 점프
        if (Input.GetKeyDown(KeyCode.M))
        {
            PlayerCommand(ENUM_PLAYER_STATE.Jump);
        }

        moveDir = 0f;

        // 이동
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            activeCharacter.Input_MoveKey(true);
            moveDir = -1.0f;
            activeCharacter.Set_inputArrowDir(moveDir);
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            activeCharacter.Input_MoveKey(true);
            moveDir = 1.0f;
            activeCharacter.Set_inputArrowDir(moveDir);
        }

        if (Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow))
        {
            moveDir = 0f;
            activeCharacter.Set_inputArrowDir(moveDir);
            activeCharacter.Input_MoveKey(false);
            if (activeCharacter.currState == ENUM_PLAYER_STATE.Move)
                PlayerCommand(ENUM_PLAYER_STATE.Idle);
        }

        if (moveDir == 0f)
        {
            if (activeCharacter.currState == ENUM_PLAYER_STATE.Move)
                PlayerCommand(ENUM_PLAYER_STATE.Idle);
        }
        else
        {
            PlayerCommand(ENUM_PLAYER_STATE.Move, new CharacterMoveParam(moveDir));
        }
    }

    public void PlayerCommand(ENUM_PLAYER_STATE nextState, CharacterParam param = null)
    {
        if (activeCharacter == null)
            return;

        if (activeCharacter.currState == ENUM_PLAYER_STATE.Skill ||
            (activeCharacter.currState == ENUM_PLAYER_STATE.Hit || activeCharacter.currState == ENUM_PLAYER_STATE.Die))
            return;

        switch (nextState)
        {
            case ENUM_PLAYER_STATE.Idle:
                activeCharacter.Idle();
                break;
            case ENUM_PLAYER_STATE.Move:
                activeCharacter.Move(param);
                break;
            case ENUM_PLAYER_STATE.Dash:
                activeCharacter.Dash();
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
                Debug.LogError("주의! PlayerCharacter에 Hit 명령이 들어옴");
                break;
            case ENUM_PLAYER_STATE.Die:
                activeCharacter.Die();
                break;
        }
    }
}
