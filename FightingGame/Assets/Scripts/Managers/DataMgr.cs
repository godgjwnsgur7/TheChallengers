using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using FGDefine;

public interface ILoader<Key, Value>
{
    Dictionary<Key, Skill> MakeDict(); 
}

public class DataMgr
{
    public Dictionary<int, Skill> SkillDict { get; private set; } = new Dictionary<int, Skill>();

    public void Init()
    {
        SkillDict = LoadJson<SkillData, int, Skill>("SkillData").MakeDict();

    }

    Loader LoadJson<Loader, Key, Value>(string path) where Loader : ILoader<Key, Value>
    {
        TextAsset textAsset = Managers.Resource.Load<TextAsset>($"Data/{path}");
        return JsonUtility.FromJson<Loader>(textAsset.text);
    }
}
