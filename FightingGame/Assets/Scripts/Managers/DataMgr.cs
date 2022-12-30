using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using FGDefine;

public interface ILoader<Key, Value>
{
    Dictionary<Key, Value> MakeDict(); 
}

public class DataMgr
{
    public Dictionary<int, Skill> SkillDict { get; private set; } = new Dictionary<int, Skill>();
    public Dictionary<int, CharacterInfo> CharInfoDict { get; private set; } = new Dictionary<int, CharacterInfo>();
    public GameInfo gameInfo { get; private set; } = new GameInfo();

    public void Init()
    {
        SkillDict = LoadJson<SkillData, int, Skill>("SkillData").MakeDict();
        CharInfoDict = LoadJson<CharacterData, int, CharacterInfo>("CharacterData").MakeDict();
        gameInfo = LoadJson<GameInfo>("GameData");
    }

    Loader LoadJson<Loader, Key, Value>(string path) where Loader : ILoader<Key, Value>
    {
        TextAsset textAsset = Managers.Resource.Load<TextAsset>($"Data/{path}");
        return JsonUtility.FromJson<Loader>(textAsset.text);
    }

    Loader LoadJson<Loader>(string path) where Loader : class
    {
        TextAsset textAsset = Managers.Resource.Load<TextAsset>($"Data/{path}");
        return JsonUtility.FromJson<Loader>(textAsset.text);
    }
}
