using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FGDefine;

public class TrainingCharacter : MonoBehaviour
{
    public ActiveCharacter activeCharacter;

    public ENUM_TEAM_TYPE teamType;
    public float moveDir;
    public bool inabilityState = false;

    Coroutine moveCoroutine = null;

    bool isMove = false;

    private void Update()
    {
        if (activeCharacter == null)
            return;

        OnKeyboard();
    }

    public virtual void Set_Character(ActiveCharacter _activeCharacter)
    {
        activeCharacter = _activeCharacter;
        activeCharacter.transform.parent = this.transform;
        activeCharacter.teamType = teamType;
        activeCharacter.Init();
        activeCharacter.Set_Sound();
        activeCharacter.Set_Character();
    }

    public void OnPointDownCallBack(ENUM_INPUTKEY_NAME _inputKeyName)
    {
        if (!Managers.Battle.isGamePlayingState)
            return;

        switch (_inputKeyName)
        {
            case ENUM_INPUTKEY_NAME.Direction:
                isMove = true;
                if (moveCoroutine == null)
                    moveCoroutine = StartCoroutine(IMove());
                break;
            case ENUM_INPUTKEY_NAME.Jump:
                PlayerCommand(ENUM_PLAYER_STATE.Jump);
                break;
            case ENUM_INPUTKEY_NAME.Dash:
                PlayerCommand(ENUM_PLAYER_STATE.Dash);
                break;
            case ENUM_INPUTKEY_NAME.Attack:
                CharacterAttackParam attackParam = new CharacterAttackParam(ENUM_ATTACKOBJECT_NAME.Knight_Attack1, activeCharacter.reverseState);
                PlayerCommand(ENUM_PLAYER_STATE.Attack, attackParam);
                activeCharacter.Change_AttackState(true);
                break;
            case ENUM_INPUTKEY_NAME.Skill1:
                CharacterSkillParam skillParam1 = new CharacterSkillParam(1);
                PlayerCommand(ENUM_PLAYER_STATE.Skill, skillParam1);
                break;
            case ENUM_INPUTKEY_NAME.Skill2:
                CharacterSkillParam skillParam2 = new CharacterSkillParam(2);
                PlayerCommand(ENUM_PLAYER_STATE.Skill, skillParam2);
                break;
            case ENUM_INPUTKEY_NAME.Skill3:
                CharacterSkillParam skillParam3 = new CharacterSkillParam(3);
                PlayerCommand(ENUM_PLAYER_STATE.Skill, skillParam3);
                break;
            case ENUM_INPUTKEY_NAME.Skill4:
                CharacterSkillParam skillParam4 = new CharacterSkillParam(4);
                PlayerCommand(ENUM_PLAYER_STATE.Skill, skillParam4);
                break;
        }
    }

    public void OnPointUpCallBack(ENUM_INPUTKEY_NAME _inputKeyName)
    {
        switch (_inputKeyName)
        {
            case ENUM_INPUTKEY_NAME.Direction:
                isMove = false;
                break;
            case ENUM_INPUTKEY_NAME.Attack:
                activeCharacter.Change_AttackState(false);
                break;
        }
    }

    public void OnPointEnterCallBack(float _moveDir)
    {
        moveDir = _moveDir;
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

    protected IEnumerator IMove()
    {
        activeCharacter.Input_MoveKey(true);
        isMove = true;

        while (isMove)
        {
            activeCharacter.Set_inputArrowDir(moveDir);
            PlayerCommand(ENUM_PLAYER_STATE.Move, new CharacterMoveParam(moveDir));
            yield return null;
        }

        activeCharacter.Set_inputArrowDir(0.0f);
        activeCharacter.Input_MoveKey(false);
        if (activeCharacter.currState == ENUM_PLAYER_STATE.Move)
            PlayerCommand(ENUM_PLAYER_STATE.Idle);

        moveCoroutine = null;
    }

    // 디버깅용
    protected virtual void OnKeyboard() { }
}
