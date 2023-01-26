using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FGDefine;
using System;

public class Knight : ActiveCharacter
{
    public override void Init()
    {
        characterType = ENUM_CHARACTER_TYPE.Knight;

        base.Init();

        if (PhotonLogicHandler.IsMine(viewID))
        {
            Debug.Log("컨트롤이 가능한 객체");
        }
        else
        {
            Debug.Log("컨트롤이 불가능한 객체");
        }
    }

    public override void Skills_Pooling()
    {
        base.Skills_Pooling();

        // Attack
        Managers.Resource.GenerateInPool("AttackObjects/Knight_Attack1", 3);
        Managers.Resource.GenerateInPool("AttackObjects/Knight_Attack2", 3);
        Managers.Resource.GenerateInPool("AttackObjects/Knight_Attack3", 3);
        Managers.Resource.GenerateInPool("AttackObjects/Knight_DashSkill_1", 3);
        Managers.Resource.GenerateInPool("AttackObjects/Knight_DashSkill_2", 3);
        Managers.Resource.GenerateInPool("AttackObjects/Knight_DashSkill_3", 3);
        Managers.Resource.GenerateInPool("AttackObjects/Knight_JumpAttack", 3);
        Managers.Resource.GenerateInPool("AttackObjects/Knight_SmashSkillObject", 3);
        Managers.Resource.GenerateInPool("AttackObjects/Knight_SmashSkillObject_1", 3);
        Managers.Resource.GenerateInPool("AttackObjects/Knight_SmashSkillObject_2", 35);
        Managers.Resource.GenerateInPool("AttackObjects/Knight_SmashSkillObject_3", 3);
        Managers.Resource.GenerateInPool("AttackObjects/Knight_ThrowSkillObject", 3);

        // Effect
        Managers.Resource.GenerateInPool("EffectObjects/Knight_SmokeEffect_Jump", 3);
        Managers.Resource.GenerateInPool("EffectObjects/Knight_SmokeEffect_Landing", 3);
        Managers.Resource.GenerateInPool("EffectObjects/Knight_SmokeEffect_Move", 3);
    }
}
