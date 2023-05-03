using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FGDefine;
using System;

public class Knight : ActiveCharacter
{
    public override void Init()
    {
        if (isInitialized)
        {
            Debug.Log("중복으로 캐릭터를 초기화 시도하였습니다.");
            return;
        }

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
        Managers.Resource.GenerateInPool($"AttackObjects/{ENUM_ATTACKOBJECT_NAME.Knight_Attack1}", 3);
        Managers.Resource.GenerateInPool($"AttackObjects/{ENUM_ATTACKOBJECT_NAME.Knight_Attack2}", 3);
        Managers.Resource.GenerateInPool($"AttackObjects/{ENUM_ATTACKOBJECT_NAME.Knight_Attack3}", 3);
        Managers.Resource.GenerateInPool($"AttackObjects/{ENUM_ATTACKOBJECT_NAME.Knight_DashSkill_1}", 3);
        Managers.Resource.GenerateInPool($"AttackObjects/{ENUM_ATTACKOBJECT_NAME.Knight_DashSkill_2}", 3);
        Managers.Resource.GenerateInPool($"AttackObjects/{ENUM_ATTACKOBJECT_NAME.Knight_DashSkill_3}", 3);
        Managers.Resource.GenerateInPool($"AttackObjects/{ENUM_ATTACKOBJECT_NAME.Knight_JumpAttack}", 3);
        Managers.Resource.GenerateInPool($"AttackObjects/{ENUM_ATTACKOBJECT_NAME.Knight_ThrowSkillObject}", 3);
        Managers.Resource.GenerateInPool($"AttackObjects/{ENUM_ATTACKOBJECT_NAME.Knight_SmashSkillObject}", 3);
        Managers.Resource.GenerateInPool($"AttackObjects/{ENUM_ATTACKOBJECT_NAME.Knight_SmashSkillObject_1}", 3);
        Managers.Resource.GenerateInPool($"AttackObjects/{ENUM_ATTACKOBJECT_NAME.Knight_SmashSkillObject_2}", 3);
        Managers.Resource.GenerateInPool($"AttackObjects/{ENUM_ATTACKOBJECT_NAME.Knight_SmashSkillObject_3}", 3);
        Managers.Resource.GenerateInPool($"AttackObjects/{ENUM_ATTACKOBJECT_NAME.Knight_ComboSkill_1}", 3);
        Managers.Resource.GenerateInPool($"AttackObjects/{ENUM_ATTACKOBJECT_NAME.Knight_ComboSkill_2}", 3);
        Managers.Resource.GenerateInPool($"AttackObjects/{ENUM_ATTACKOBJECT_NAME.Knight_ComboSkill_3}", 3);
        Managers.Resource.GenerateInPool($"AttackObjects/{ENUM_ATTACKOBJECT_NAME.Knight_ComboSkill_4}", 3);
        Managers.Resource.GenerateInPool($"AttackObjects/{ENUM_ATTACKOBJECT_NAME.Knight_ComboSkill_5}", 3);

        // Effect
        Managers.Resource.GenerateInPool($"EffectObjects/{ENUM_EFFECTOBJECT_NAME.Knight_JumpEffect}", 3);
        Managers.Resource.GenerateInPool($"EffectObjects/{ENUM_EFFECTOBJECT_NAME.Knight_LandingEffect}", 3);
        Managers.Resource.GenerateInPool($"EffectObjects/{ENUM_EFFECTOBJECT_NAME.Knight_MoveEffect}", 3);
        Managers.Resource.GenerateInPool($"EffectObjects/{ENUM_EFFECTOBJECT_NAME.Knight_DashEffect}", 3);
    }
}
