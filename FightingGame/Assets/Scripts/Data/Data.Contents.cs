using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using FGDefine;

#region Skills

[Serializable]
public class Skill
{
    public int skillType; // ENUM_SKILL_TYPE (Key)
    public float damage;
    public float runTime;
    public float stunTime;
    public float risingPower;
    public float pushingPower;
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
