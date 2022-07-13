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
    public int skillType; // ENUM_SKILL_TYPE
    public float damage;
    public float delayTime;
    public float runTime;
    public float stunTime;
    public float risingPower;
}

[Serializable]
public class SkillData : ILoader<int, Skill>
{
    public List<Skill> Skills = new List<Skill>();

    public Dictionary<int, Skill> MakeDict()
    {
        Dictionary<int, Skill> skillDict = new Dictionary<int, Skill>();
        foreach (Skill skill in Skills)
            skillDict.Add(skill.skillType, skill);
        return skillDict;
    }
}
#endregion