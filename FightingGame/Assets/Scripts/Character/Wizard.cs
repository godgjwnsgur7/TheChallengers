using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FGDefine;
using System;

public class Wizard : ActiveCharacter
{
	private bool isInitialized = false;

	public override void Init()
    {
		base.Init();

		if (isInitialized)
        {
            Debug.Log("중복으로 캐릭터를 초기화 시도하였습니다."); 
            return;
        }

        isInitialized = true;

		characterType = ENUM_CHARACTER_TYPE.Wizard;

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
        Managers.Resource.GenerateInPool($"AttackObjects/{ENUM_ATTACKOBJECT_NAME.Wizard_Attack1}", 3);
        Managers.Resource.GenerateInPool($"AttackObjects/{ENUM_ATTACKOBJECT_NAME.Wizard_Attack2}", 3);
        Managers.Resource.GenerateInPool($"AttackObjects/{ENUM_ATTACKOBJECT_NAME.Wizard_ThrowAttackObject}", 3);
        Managers.Resource.GenerateInPool($"AttackObjects/{ENUM_ATTACKOBJECT_NAME.Wizard_ThrowJumpAttackObject}", 3);
        Managers.Resource.GenerateInPool($"AttackObjects/{ENUM_ATTACKOBJECT_NAME.Wizard_ThunderSkillObject}", 3);
        Managers.Resource.GenerateInPool($"AttackObjects/{ENUM_ATTACKOBJECT_NAME.Wizard_ThunderSkillObject_1}", 3);
        Managers.Resource.GenerateInPool($"AttackObjects/{ENUM_ATTACKOBJECT_NAME.Wizard_ThunderSkillObject_2}", 3);
        Managers.Resource.GenerateInPool($"AttackObjects/{ENUM_ATTACKOBJECT_NAME.Wizard_ThunderSkillObject_3}", 3);
        Managers.Resource.GenerateInPool($"AttackObjects/{ENUM_ATTACKOBJECT_NAME.Wizard_IceSkillObject}", 3);
        Managers.Resource.GenerateInPool($"AttackObjects/{ENUM_ATTACKOBJECT_NAME.Wizard_IceSkillObject_1}", 3);
        Managers.Resource.GenerateInPool($"AttackObjects/{ENUM_ATTACKOBJECT_NAME.Wizard_MeteorSkillObject}", 3);
        Managers.Resource.GenerateInPool($"AttackObjects/{ENUM_ATTACKOBJECT_NAME.Wizard_MeteorSkillObject_Falling}", 3);
        Managers.Resource.GenerateInPool($"AttackObjects/{ENUM_ATTACKOBJECT_NAME.Wizard_MeteorSkillObject_Explode}", 3);
        Managers.Resource.GenerateInPool($"AttackObjects/{ENUM_ATTACKOBJECT_NAME.Wizard_PushOutSkillObject}", 3);
        Managers.Resource.GenerateInPool($"AttackObjects/{ENUM_ATTACKOBJECT_NAME.Wizard_PushOutSkillObject_1}", 3);

        // Effect
        Managers.Resource.GenerateInPool($"EffectObjects/{ENUM_EFFECTOBJECT_NAME.Wizard_ThunderCircleEffect}", 3);
        Managers.Resource.GenerateInPool($"EffectObjects/{ENUM_EFFECTOBJECT_NAME.Wizard_MagicalJinEffect}", 10); // Particle
        Managers.Resource.GenerateInPool($"EffectObjects/{ENUM_EFFECTOBJECT_NAME.Wizard_StarlightEffect}", 10); // Particle
        Managers.Resource.GenerateInPool($"EffectObjects/{ENUM_EFFECTOBJECT_NAME.Wizard_DashEffect}", 3);
        Managers.Resource.GenerateInPool($"EffectObjects/{ENUM_EFFECTOBJECT_NAME.Wizard_FlameEffect}", 3);

    }

}
