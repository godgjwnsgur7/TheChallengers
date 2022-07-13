using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FGDefine;

public class Knight : ActiveCharacter
{
    

    public override void Init()
    {
        characterType = ENUM_CHARACTER_TYPE.Knight;

        base.Init();

        // 테스트 코드
        Managers.Resource.GenerateInPool("AttackObejcts/Knight_Attack", 3);

        // Get_SkillData();
    }

    private void Get_SkillData()
    {

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

            string path = ENUM_SKILL_TYPE.Knight_Attack.ToString();
            attackObject = Managers.Resource.GetAttackObject(path);

            if (attackObject != null)
            {
                StartCoroutine(IAttackDelayTimeCheck(attackParam));
            }
        }
    }
}
