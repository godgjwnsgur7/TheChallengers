using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FGDefine;

public class TrainingCharacter : MonoBehaviour
{
    [SerializeField] PlayerCamera playerCamera;
    public ActiveCharacter activeCharacter = null;
    public ActiveCharacter enemyCharacter = null;

    public float moveDir;
    public bool inabilityState = false;

    Coroutine moveCoroutine = null;

    bool isMove = false;

    private void OnDisable()
    {
        if (moveCoroutine != null)
            StopCoroutine(moveCoroutine);
    }

    public void Init(BaseMap currMap)
    {
        Destroy_MyCharacter();
        Destroy_EnemyCharacter();
        Managers.Input.Deactive_InputKeyController();

        playerCamera.Init(currMap);
    }
    
    public void Summon_MyCharacter(ENUM_CHARACTER_TYPE _summonCharType, Vector2 _summonPosVec)
    {
        if (activeCharacter != null)
        {
            Destroy_MyCharacter();
            playerCamera.Camera_Moving();
        }

        _summonPosVec.y += 1f;
        activeCharacter = Managers.Resource.Instantiate($"{_summonCharType}", _summonPosVec).GetComponent<ActiveCharacter>();
        Destroy(playerCamera.GetComponent<AudioListener>());

        activeCharacter.Init();
        activeCharacter.Set_Character(ENUM_TEAM_TYPE.Blue);
        playerCamera.Following_Target(activeCharacter.transform);

        Connect_InputController();

        playerCamera.Set_Target(activeCharacter.transform);
    }

    public void Summon_EnemyCharacter(ENUM_CHARACTER_TYPE _summonCharType, Vector2 _summonPosVec)
    {
        if (enemyCharacter != null)
            Destroy_EnemyCharacter();

        Vector2 summonPosVec = activeCharacter == null ? _summonPosVec : (Vector2)activeCharacter.transform.position;
        summonPosVec.y += 1f;
        enemyCharacter = Managers.Resource.Instantiate($"{_summonCharType}", summonPosVec).GetComponent<ActiveCharacter>();

        enemyCharacter.Init();
        enemyCharacter.Set_Character(ENUM_TEAM_TYPE.Red);
    }

    public void Destroy_MyCharacter()
    {
        if (activeCharacter == null)
            return;

        playerCamera.gameObject.AddComponent<AudioListener>();
        Managers.Resource.Destroy(activeCharacter.gameObject);
    }

    public void Destroy_EnemyCharacter()
    {
        if (enemyCharacter == null)
            return;

        Managers.Resource.Destroy(enemyCharacter.gameObject);
    }

    public void Connect_InputController()
    {
        Managers.Input.Connect_InputKeyController(activeCharacter.characterType, OnPointDownCallBack, OnPointUpCallBack);
        Managers.Input.Connect_InputArrowKey(OnPointEnterCallBack);
    }

    public void OnPointDownCallBack(ENUM_INPUTKEY_NAME _inputKeyName)
    {
        if (activeCharacter == null)
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
        if (activeCharacter == null)
            return;

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
        if (activeCharacter.currState == ENUM_PLAYER_STATE.Move ||
            activeCharacter.currState == ENUM_PLAYER_STATE.Jump)
            PlayerCommand(ENUM_PLAYER_STATE.Idle);

        moveCoroutine = null;
    }
}
