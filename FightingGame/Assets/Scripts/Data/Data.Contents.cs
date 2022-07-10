using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using FGDefine;

#region Skills
/// <summary>
/// 모든 캐릭터의 스킬 타입 (Key)
/// </summary>
[Serializable]
public enum ENUM_SKILL_TYPE
{
    Knight_Attack = 0,
    Knight_JumpAttack = 1,


}

[Serializable]
public class Skill
{
    public ENUM_SKILL_TYPE skillType;
    public float damage;
    public float delayTime;
    public float runTime;
    public float risingPower;
}

[Serializable]
public class SkillData : ILoader<ENUM_SKILL_TYPE, Skill>
{
    public List<Skill> SkillList = new List<Skill>();

    public Dictionary<ENUM_SKILL_TYPE, Skill> MakeDict()
    {
        Dictionary<ENUM_SKILL_TYPE, Skill> skillDict = new Dictionary<ENUM_SKILL_TYPE, Skill>();
        foreach (Skill skill in SkillList)
            skillDict.Add(skill.skillType, skill);
        return skillDict;
    }
}
#endregion