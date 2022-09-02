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

        if (PhotonLogicHandler.IsConnected)
        {
            PhotonLogicHandler.Instance.TryBroadcastMethod<Character, ENUM_TEAM_TYPE>(activeCharacter, activeCharacter.Set_TeamType, teamType);
            activeCharacter.Set_Character();
        }
        else
        {
            activeCharacter.teamType = teamType;
            activeCharacter.Init();
            activeCharacter.Set_Character();
        }

        playerCamera.Init(activeCharacter.transform);
    }

    public void Connect_Status(StatusWindowUI _statusWindowUI)
    {
        _statusWindowUI.Set_MaxHP(activeCharacter.curHP);
        activeCharacter.statusWindowUI = _statusWindowUI;
    }

    // 디버깅용이니 쿨하게 다 때려박기
    private void OnKeyboard()
    {
        // 공격
        if (Input.GetKeyDown(KeyCode.F))
        {
            CharacterAttackParam attackParam = new CharacterAttackParam(ENUM_SKILL_TYPE.Knight_Attack1, activeCharacter.reverseState);
            PlayerCommand(ENUM_PLAYER_STATE.Attack, attackParam);
            activeCharacter.Change_AttackState(true); 
        }

        if (Input.GetKeyUp(KeyCode.F))
        {
            activeCharacter.Change_AttackState(false);
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
        {
            moveDir = 0f;
            if(activeCharacter.currState == ENUM_PLAYER_STATE.Move)
                PlayerCommand(ENUM_PLAYER_STATE.Idle);
        }

        if (moveDir != 0f)
        {
            PlayerCommand(ENUM_PLAYER_STATE.Move, new CharacterMoveParam(moveDir));
        }
    }
    
    private void PlayerCommand(ENUM_PLAYER_STATE nextState, CharacterParam param = null)
    {
        if (activeCharacter == null)
            return;

        if ( activeCharacter.currState == ENUM_PLAYER_STATE.Skill ||
            (activeCharacter.currState == ENUM_PLAYER_STATE.Hit || activeCharacter.currState == ENUM_PLAYER_STATE.Die))
            return;

        /*
        if(PhotonLogicHandler.IsConnected)
        {
            switch(nextState)
            {
                case ENUM_PLAYER_STATE.Idle:
                    PhotonLogicHandler.Instance.TryBroadcastMethod<Character>
                        (activeCharacter, activeCharacter.Idle);
                    activeCharacter.Idle();
                    break;
                case ENUM_PLAYER_STATE.Move:
                    PhotonLogicHandler.Instance.TryBroadcastMethod<Character, CharacterParam>
                        (activeCharacter, activeCharacter.Move, param);
                    break;
                case ENUM_PLAYER_STATE.Jump:
                    PhotonLogicHandler.Instance.TryBroadcastMethod<Character>
                        (activeCharacter,activeCharacter.Jump);
                    break;
                case ENUM_PLAYER_STATE.Attack:
                    PhotonLogicHandler.Instance.TryBroadcastMethod<Character, CharacterParam>
                        (activeCharacter, activeCharacter.Attack, param);
                    break;
                case ENUM_PLAYER_STATE.Skill:
                    PhotonLogicHandler.Instance.TryBroadcastMethod<Character, CharacterParam>
                        (activeCharacter, activeCharacter.Skill, param);
                    break;
                case ENUM_PLAYER_STATE.Hit:
                    PhotonLogicHandler.Instance.TryBroadcastMethod<Character, CharacterParam>
                        (activeCharacter, activeCharacter.Hit, param);
                    break;
                case ENUM_PLAYER_STATE.Die:
                    PhotonLogicHandler.Instance.TryBroadcastMethod<Character>
                        (activeCharacter, activeCharacter.Die);
                    break;
            }

            return;
        }
        */

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