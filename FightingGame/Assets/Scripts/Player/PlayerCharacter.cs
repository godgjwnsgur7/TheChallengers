using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FGDefine;
using System;

public class PlayerCharacter : MonoBehaviour
{
    public ActiveCharacter activeCharacter;
    [SerializeField] PlayerCamera playerCamera;

    public InteractableObject interactableObject;

    public Vector2 dirVec = Vector2.zero;

    public bool inabilityState = false;
    
    private void Awake()
    {
        if(activeCharacter == null)
        {
            activeCharacter = Managers.Resource.Instantiate("Character", this.transform).GetComponent<ActiveCharacter>();
            activeCharacter.tag = ENUM_TAG_TYPE.Ally.ToString();
        }
        
        activeCharacter.Init();
        playerCamera.Init(activeCharacter.transform);
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

    private void ForwardScan()
    {
        var currObj = activeCharacter.GetForwardObjectWithRay();

        if(currObj != null)
            currObj.SetInteractable();
        else
            interactableObject?.UnsetInteractable();

        interactableObject = currObj;
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
            PlayerCommand(ENUM_PLAYER_STATE.Hit, new CharacterHitParam(10.0f));
        }

        // 상호작용 (무기)
        if(Input.GetKeyDown(KeyCode.G) && interactableObject != null)
        {
            interactableObject.Interact();

            if(interactableObject.interactionType == ENUM_INTERACTION_TYPE.Weapon)
            {
                WeaponObject weaponObject = interactableObject as WeaponObject;
                activeCharacter.SetWeapon(weaponObject.GetInfo());
            }
        }

        dirVec = Vector2.zero;

        if (Input.GetKey(KeyCode.W)) dirVec.y = 1.0f;
        if (Input.GetKey(KeyCode.A)) dirVec.x = -1.0f;
        if (Input.GetKey(KeyCode.S)) dirVec.y = -1.0f;
        if (Input.GetKey(KeyCode.D)) dirVec.x = 1.0f;

        if (dirVec == Vector2.zero)
        {
            if(activeCharacter.currState == ENUM_PLAYER_STATE.Move)
                PlayerCommand(ENUM_PLAYER_STATE.Idle);
        }
        else
        {
            PlayerCommand(ENUM_PLAYER_STATE.Move, new CharacterMoveParam(dirVec, Input.GetKey(KeyCode.LeftShift)));
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
                ForwardScan();
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