using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using FGDefine;

public interface ILoader<Key, Value>
{
    Dictionary<Key, Skill> MakeDict(); 
}

// 예제코드만 존재하는 상태
public class DataMgr
{
    public Dictionary<ENUM_SKILL_TYPE, Skill> SkillDict { get; private set; } = new Dictionary<ENUM_SKILL_TYPE, Skill>();
    
    public void Init()
    {
        SkillDict = LoadJson<SkillData, ENUM_SKILL_TYPE, Skill>("SkillData").MakeDict();
    }

    Loader LoadJson<Loader, Key, Value>(string path) where Loader : ILoader<Key, Value>
    {
        TextAsset textAsset = Managers.Resource.Load<TextAsset>($"Data/{path}");
        return JsonUtility.FromJson<Loader>(textAsset.text);
    }
}
