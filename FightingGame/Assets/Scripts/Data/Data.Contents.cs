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

#region CharacterInfos
[Serializable]
public class CharacterInfo
{
    public int characterType; // ENUM_CHARACTER_TYPE (Key)
    public float maxHP;
    public float moveSpeed;
    public float jumpPower;
    public float dashPower;
    public float skillCoolTime_Dash;
    public float skillCoolTime_1;
    public float skillCoolTime_2;
    public float skillCoolTime_3;
    public float skillCoolTime_4;
}

public class CharacterData : ILoader<int, CharacterInfo>
{
    public List<CharacterInfo> CharacterInfos = new List<CharacterInfo>();

    public Dictionary<int, CharacterInfo> MakeDict()
    {
        Dictionary<int, CharacterInfo> CharInfoDict = new Dictionary<int, CharacterInfo>();
        foreach (CharacterInfo charInfo in CharacterInfos)
            CharInfoDict.Add(charInfo.characterType, charInfo);
        return CharInfoDict;
    }
}
#endregion

#region GameInfo
[Serializable]
public class GameInfo
{
    public float invincibleTime; // 무적상태에서 기상 후 무적시간
    public float dashSkillTime; // 대쉬시간
    public float maxComboDamage; // 맥스 콤보 데미지
    public float hitImmunityPower; // 히트보정 시 작용하는 힘의 크기 (밀려나는 힘)
    public float maxGameRunTime; // 게임의 제한시간
}
#endregion