using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FGDefine;

public class TrainingPlayer : TrainingCharacter
{
    [SerializeField] PlayerCamera playerCamera;

    public override void Set_Character(ActiveCharacter _activeCharacter)
    {
        base.Set_Character(_activeCharacter);

        playerCamera.Init(activeCharacter.transform);
        Connect_InputController();
    }

    public void Connect_InputController()
    {
        Managers.Input.Connect_InputKeyController(OnPointDownCallBack, OnPointUpCallBack);
        Managers.Input.Connect_InputArrowKey(OnPointEnterCallBack);
    }

    protected override void OnKeyboard()
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
        if (Input.GetKeyDown(KeyCode.R))
        {
            OnPointDownCallBack(ENUM_INPUTKEY_NAME.Skill1);
        }

        // 스킬 2번
        if (Input.GetKeyDown(KeyCode.T))
        {
            OnPointDownCallBack(ENUM_INPUTKEY_NAME.Skill2);
        }

        // 스킬 3번
        if (Input.GetKeyDown(KeyCode.Y))
        {
            OnPointDownCallBack(ENUM_INPUTKEY_NAME.Skill3);
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
