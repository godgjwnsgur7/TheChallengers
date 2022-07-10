using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FGDefine;

public class Knight : ActiveCharacter
{
    Dictionary<ENUM_SKILL_TYPE, Skill> skillDict;
    
    public override void Init()
    {
        characterType = ENUM_CHARACTER_TYPE.Knight;

        base.Init();

        Get_SkillData();
    }

    private void Get_SkillData()
    {
        skillDict = new Dictionary<ENUM_SKILL_TYPE, Skill>();

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
            GameObject g = Managers.Resource.GetAttackObject(path);
            attackObject = g.gameObject.GetComponent<AttackObejct>();

            if(attackObject != null)
            {
                StartCoroutine(IAttackDelayTimeCheck(attackParam));
                attackObject = null;
            }
        }
    }
}
