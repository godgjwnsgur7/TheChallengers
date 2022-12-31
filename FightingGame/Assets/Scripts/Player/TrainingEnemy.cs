using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FGDefine;

public class TrainingEnemy : TrainingCharacter
{
    public override void Set_Character(ActiveCharacter _activeCharacter)
    {
        base.Set_Character(_activeCharacter);
    }

    protected override void OnKeyboard()
    {
        // 공격
        if (Input.GetKeyDown(KeyCode.N))
        {
            OnPointDownCallBack(ENUM_INPUTKEY_NAME.Attack);
        }

        if (Input.GetKeyUp(KeyCode.N))
        {
            OnPointUpCallBack(ENUM_INPUTKEY_NAME.Attack);
        }

        // 스킬 1번
        if (Input.GetKeyDown(KeyCode.J))
        {
            OnPointDownCallBack(ENUM_INPUTKEY_NAME.Skill1);
        }

        // 스킬 2번
        if (Input.GetKeyDown(KeyCode.K))
        {
            OnPointDownCallBack(ENUM_INPUTKEY_NAME.Skill2);
        }

        // 스킬 3번
        if (Input.GetKeyDown(KeyCode.L))
        {
            OnPointDownCallBack(ENUM_INPUTKEY_NAME.Skill3);
        }

        // 점프
        if (Input.GetKeyDown(KeyCode.M))
        {
            OnPointDownCallBack(ENUM_INPUTKEY_NAME.Jump);
        }

        // 이동
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            moveDir = -1f;
            OnPointDownCallBack(ENUM_INPUTKEY_NAME.Direction);
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            moveDir = 1.0f;
            OnPointDownCallBack(ENUM_INPUTKEY_NAME.Direction);
        }

        if (Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow))
        {
            if (!Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow))
                OnPointUpCallBack(ENUM_INPUTKEY_NAME.Direction);
        }
    }
}
