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

        skills[0] = ENUM_SKILL_TYPE.Knight_ThrowSkill;
        skills[1] = ENUM_SKILL_TYPE.Knight_ThrowSkill; // 임시
        skills[2] = ENUM_SKILL_TYPE.Knight_ThrowSkill; // 임시

        base.Init();

        // PhotonLogicHandler.Instance.TryBroadcastMethod<Knight>(this, Attack);

        if (PhotonLogicHandler.IsMine(viewID))
        {
            Debug.Log("컨트롤이 가능한 객체");
        }
        else
        {
            Debug.Log("컨트롤이 불가능한 객체");
        }
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
}
