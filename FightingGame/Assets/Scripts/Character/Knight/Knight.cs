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

        // 테스트 코드 (나중에 배틀씬으로 이동할 때 해당 캐릭터의 공격 오브젝트들을 풀링)
        Managers.Resource.GenerateInPool("AttackObejcts/Knight_JumpAttack", 2);
        Managers.Resource.GenerateInPool("AttackObejcts/Knight_Attack1", 2);
        Managers.Resource.GenerateInPool("AttackObejcts/Knight_Attack2", 2);
        Managers.Resource.GenerateInPool("AttackObejcts/Knight_Attack3", 2);

    }

    public override void Attack(CharacterParam param)
    {
        if (currState == ENUM_PLAYER_STATE.Attack)
            return;

        base.Attack(param);

        var attackParam = param as CharacterAttackParam;

        if (attackParam != null)
        {
            anim.SetTrigger("AttackTrigger");
        }
    }

    public void Summon_AttackObject(int _attackTypeNum)
    {
        attackObject = null;
        ENUM_SKILL_TYPE skillType = (ENUM_SKILL_TYPE)_attackTypeNum;
        attackObject = Managers.Resource.GetAttackObject(skillType.ToString());

        if(attackObject != null)
        {
            attackObject.transform.position = transform.position;
            attackObject.ActivatingAttackObject(reverseState);
        }
        else
        {
            Debug.Log("찾을 수 없음");
        }
    }
}
