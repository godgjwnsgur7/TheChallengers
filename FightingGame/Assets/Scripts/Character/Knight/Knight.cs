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

        skills[0] = ENUM_SKILL_TYPE.Knight_ShotSkill;
        skills[1] = ENUM_SKILL_TYPE.Knight_Skill2;
        skills[2] = ENUM_SKILL_TYPE.Knight_Skill3;

        // 테스트 코드 (나중에 배틀씬으로 이동할 때 해당 캐릭터의 공격 오브젝트들을 풀링)
        Managers.Resource.GenerateInPool("AttackObejcts/Knight_JumpAttack", 4);
        Managers.Resource.GenerateInPool("AttackObejcts/Knight_Attack1", 4);
        Managers.Resource.GenerateInPool("AttackObejcts/Knight_Attack2", 4);
        Managers.Resource.GenerateInPool("AttackObejcts/Knight_Attack3", 4);
        Managers.Resource.GenerateInPool("AttackObejcts/Knight_ShotSkill", 4);

        SyncAnimator(anim);
        SyncPhysics(rigid2D);
        SyncTransformView(transform);
    }

	public override void Attack(CharacterParam param)
    {
        if (currState == ENUM_PLAYER_STATE.Attack || currState == ENUM_PLAYER_STATE.Skill)
            return;

        base.Attack(param);

        var attackParam = param as CharacterAttackParam;

        if (attackParam != null)
        {
            anim.SetTrigger("AttackTrigger");
        }
    }

    public override void Skill(CharacterParam param)
    {
        if (currState == ENUM_PLAYER_STATE.Skill || jumpState)
            return;

        base.Skill(param);

        var skillParam = param as CharacterSkillParam;

        if (skillParam != null)
        {
            anim.SetInteger("SkillType", skillParam.skillNum);
            anim.SetTrigger("SkillTrigger");
        }
    }

    public void Summon_AttackObject(int _attackTypeNum)
    {
        attackObject = null;
        ENUM_SKILL_TYPE attackType = (ENUM_SKILL_TYPE)_attackTypeNum;
        attackObject = Managers.Resource.GetAttackObject(attackType.ToString());

        if(attackObject != null)
        {
            attackObject.transform.position = transform.position;
            attackObject.ActivatingAttackObject(gameObject, reverseState);
        }
        else
        {
            Debug.Log("찾을 수 없음");
        }
    }
}
