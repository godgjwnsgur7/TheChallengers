using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FGDefine;
using System;

public class PlayerCharacter : MonoBehaviour
{
    public ActiveCharacter activeCharacter;
    [SerializeField] PlayerCamera playerCamera;

    private ENUM_TEAM_TYPE teamType;
    public float moveDir;
    public bool inabilityState = false;

    Coroutine moveCoroutine = null;
    Coroutine debugCoroutine = null;

    bool isMove = false;
    bool isDebugState = false;

    private void Update()
    {
        if (activeCharacter == null)
            return;

        if (!PhotonLogicHandler.IsMine(activeCharacter.ViewID))
            return;

        OnKeyboard(); // 디버깅용
    }

    /* APK 뽑으면 활성화하고 Update문 삭제
    private void OnEnable()
    {
        if(!PhotonLogicHandler.IsConnected) // 디버깅용
        {
            isDebugState = true;
            debugCoroutine = StartCoroutine(IOnKeyboard());
        }
    }
    */

    private void OnDisable()
    {
        isDebugState = false;

        if(debugCoroutine != null)
            StopCoroutine(debugCoroutine);
    }

    public void Init(BaseMap _currMap, ENUM_CHARACTER_TYPE _summonCharType)
    {
        Vector2 summonPosVec;

        playerCamera.Init(_currMap);

        if (!PhotonLogicHandler.IsConnected) // 디버그용
        {
            teamType = ENUM_TEAM_TYPE.Blue;
            summonPosVec = _currMap.blueTeamSpawnPoint.position;

            activeCharacter = Managers.Resource.Instantiate($"{_summonCharType}", summonPosVec).GetComponent<ActiveCharacter>();
            activeCharacter.transform.parent = this.transform;
            activeCharacter.Init();
            activeCharacter.Skills_Pooling();

            activeCharacter.Set_Character(teamType);
            playerCamera.Following_Target(activeCharacter.transform);

            Connect_InputController();

            Managers.Battle.GameStart();

            return;
        }

        if (PhotonLogicHandler.IsMasterClient)
        {
            teamType = ENUM_TEAM_TYPE.Blue;
            summonPosVec = _currMap.blueTeamSpawnPoint.position;
        }
        else
        {
            teamType = ENUM_TEAM_TYPE.Red;
            summonPosVec = _currMap.redTeamSpawnPoint.position;
        }

        Summon_Character(_summonCharType, summonPosVec);
    }

    public void Summon_Character(ENUM_CHARACTER_TYPE _charType, Vector2 _summonPosVec)
    {
        activeCharacter = Managers.Resource.InstantiateEveryone($"{_charType}", _summonPosVec).GetComponent<ActiveCharacter>();
        activeCharacter.transform.parent = this.transform;
        activeCharacter.Skills_Pooling();
        
        PhotonLogicHandler.Instance.TryBroadcastMethod<Character>
            (activeCharacter, activeCharacter.Set_Sound);

        activeCharacter.Set_Character(teamType);
        playerCamera.Following_Target(activeCharacter.transform);

        Connect_InputController();
    }

    public void Connect_InputController()
    {
        Managers.Input.Connect_InputKeyController(activeCharacter.characterType, OnPointDownCallBack, OnPointUpCallBack);
        Managers.Input.Connect_InputArrowKey(OnPointEnterCallBack);
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

        if ( activeCharacter.currState == ENUM_PLAYER_STATE.Skill ||
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

    // 디버깅용 코루틴
    protected IEnumerator IOnKeyboard()
    {
        while(isDebugState)
        {
            OnKeyboard();
            yield return null;
        }

        debugCoroutine = null;
    }

    // 디버깅용이니 쿨하게 다 때려박기
    private void OnKeyboard()
    {
        // 공격
        if (Input.GetKeyDown(KeyCode.F))
        {
            OnPointDownCallBack(ENUM_INPUTKEY_NAME.Attack);
        }

        if (Input.GetKeyUp(KeyCode.F))
        {
            OnPointUpCallBack(ENUM_INPUTKEY_NAME.Attack);
        }

        // 스킬 1번
        if (Input.GetKeyDown(KeyCode.E))
        {
            OnPointDownCallBack(ENUM_INPUTKEY_NAME.Skill1);
        }

        // 스킬 2번
        if (Input.GetKeyDown(KeyCode.R))
        {
            OnPointDownCallBack(ENUM_INPUTKEY_NAME.Skill2);
        }

        // 스킬 3번
        if (Input.GetKeyDown(KeyCode.T))
        {
            OnPointDownCallBack(ENUM_INPUTKEY_NAME.Skill3);
        }

        // 스킬 4번
        if (Input.GetKeyDown(KeyCode.Y))
        {
            OnPointDownCallBack(ENUM_INPUTKEY_NAME.Skill4);
        }
        
        // 대쉬
        if (Input.GetKeyDown(KeyCode.Q))
        {
            OnPointDownCallBack(ENUM_INPUTKEY_NAME.Dash);
        }

        // 점프
        if (Input.GetKeyDown(KeyCode.G))
        {
            OnPointDownCallBack(ENUM_INPUTKEY_NAME.Jump);
        }

        // 이동
        if (Input.GetKeyDown(KeyCode.A))
        {
            moveDir = -1f;
            OnPointDownCallBack(ENUM_INPUTKEY_NAME.Direction);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            moveDir = 1.0f;
            OnPointDownCallBack(ENUM_INPUTKEY_NAME.Direction);
        }

        if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D))
        {
            if (!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
                OnPointUpCallBack(ENUM_INPUTKEY_NAME.Direction);
        }
    }
}