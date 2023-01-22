using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FGDefine;
using System;

public class Wizard : ActiveCharacter
{
    public override void Init()
    {
        characterType = ENUM_CHARACTER_TYPE.Wizard;

        base.Init();

        Skills_Pooling();

        if (PhotonLogicHandler.IsMine(viewID))
        {
            Debug.Log("컨트롤이 가능한 객체");
        }
        else
        {
            Debug.Log("컨트롤이 불가능한 객체");
        }
    }

    private void Skills_Pooling()
    {
        // Attack
        Managers.Resource.GenerateInPool("AttackObjects/Wizard_Attack1", 3);
        Managers.Resource.GenerateInPool("AttackObjects/Wizard_Attack2", 3);
        Managers.Resource.GenerateInPool("AttackObjects/Wizard_ThrowAttackObject", 3);
        Managers.Resource.GenerateInPool("AttackObjects/Wizard_ThrowJumpAttackObject", 3);
        Managers.Resource.GenerateInPool("AttackObjects/Wizard_ThunderSkillObject", 3);
        Managers.Resource.GenerateInPool("AttackObjects/Wizard_ThunderSkillObject_1", 3);
        Managers.Resource.GenerateInPool("AttackObjects/Wizard_ThunderSkillObject_2", 3);
        Managers.Resource.GenerateInPool("AttackObjects/Wizard_ThunderSkillObject_3", 3);
        Managers.Resource.GenerateInPool("AttackObjects/Wizard_IceSkillObject", 3);
        Managers.Resource.GenerateInPool("AttackObjects/Wizard_IceSkillObject_1", 3);
        Managers.Resource.GenerateInPool("AttackObjects/Wizard_MeteorSkillObject", 3);
        Managers.Resource.GenerateInPool("AttackObjects/Wizard_MeteorSkillObject_1", 3);

        // Public Effect
        Managers.Resource.GenerateInPool("EffectObjects/Basic_AttackedEffect1", 3);
        Managers.Resource.GenerateInPool("EffectObjects/Basic_AttackedEffect2", 3);
        Managers.Resource.GenerateInPool("EffectObjects/Basic_AttackedEffect3", 3);

        // Effect
        Managers.Resource.GenerateInPool("EffectObjects/Wizard_ThunderEffect", 3);
    }
}
