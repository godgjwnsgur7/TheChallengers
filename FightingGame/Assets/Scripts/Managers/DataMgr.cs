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
    private Dictionary<ENUM_CHARACTER_TYPE, string> charNameDict = new Dictionary<ENUM_CHARACTER_TYPE, string>
    {
        {ENUM_CHARACTER_TYPE.Default, "캐릭터 미선택" },
        {ENUM_CHARACTER_TYPE.Knight, "나이트" },
        {ENUM_CHARACTER_TYPE.Wizard, "위저드" },
        {ENUM_CHARACTER_TYPE.Max, "알 수 없는 캐릭터" },
    };

    private Dictionary<ENUM_CHARACTER_TYPE, string> charExplanationDict = new Dictionary<ENUM_CHARACTER_TYPE, string>
    {
        {ENUM_CHARACTER_TYPE.Default, "캐릭터가 선택되지 않았습니다." },
        {ENUM_CHARACTER_TYPE.Knight, "나이트 캐릭터에 대한 설명을 넣어주세요.\n2줄정도?" },
        {ENUM_CHARACTER_TYPE.Wizard, "위저드 캐릭터에 대한 설명을 넣어주세요.\n2줄정도?" },
        {ENUM_CHARACTER_TYPE.Max, "알 수 없는 캐릭터" },
    };

    private Dictionary<ENUM_MAP_TYPE, string> mapNameDict = new Dictionary<ENUM_MAP_TYPE, string>()
    {
        {ENUM_MAP_TYPE.CaveMap, "동굴 맵" },
        {ENUM_MAP_TYPE.ForestMap, "잊혀진 숲" },
        {ENUM_MAP_TYPE.VolcanicMap, "화산지대" }
    };

    private Dictionary<ENUM_MAP_TYPE, string> mapExplanationDict = new Dictionary<ENUM_MAP_TYPE, string>()
    {
        {ENUM_MAP_TYPE.CaveMap, "동굴 맵에 대한 설명입니다.\n2줄정도?" },
        {ENUM_MAP_TYPE.ForestMap, "잊혀진 숲 맵에 대한 설명입니다.\n2줄정도?" },
        {ENUM_MAP_TYPE.VolcanicMap, "화산지대 맵에 대한 설명입니다.\n2줄정도?" }
    };

    public Dictionary<int, Skill> SkillDict { get; private set; } = new Dictionary<int, Skill>();
    public Dictionary<int, BgmSound> BgmSoundDict { get; private set; } = new Dictionary<int, BgmSound>();
    public Dictionary<int, SfxSound> SfxSoundDict { get; private set; } = new Dictionary<int, SfxSound>();
    public Dictionary<int, CharacterInfo> CharInfoDict { get; private set; } = new Dictionary<int, CharacterInfo>();
    public GameInfo gameInfo { get; private set; } = new GameInfo();

    public void Init()
    {
        SkillDict = LoadJson<SkillData, int, Skill>("SkillData").MakeDict();
        BgmSoundDict = LoadJson<BgmSoundData, int, BgmSound>("BgmSoundData").MakeDict();
        SfxSoundDict = LoadJson<SfxSoundData, int, SfxSound>("SfxSoundData").MakeDict();
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
    
    public string Get_MapNameDict(ENUM_MAP_TYPE mapType)
    {
        if (!mapNameDict.ContainsKey(mapType))
        {
            Debug.Log($"해당하는 맵 타입이 맵 이름 사전에 없습니다. : {mapType}");
            return null;
        }

        return mapNameDict[mapType];
    }

    public string Get_MapExplanationDict(ENUM_MAP_TYPE mapType)
    {
        if (!mapExplanationDict.ContainsKey(mapType))
        {
            Debug.Log($"해당하는 맵 타입이 맵 이름 사전에 없습니다. : {mapType}");
            return null;
        }

        return mapExplanationDict[mapType];
    }

    public string Get_CharNameDict(ENUM_CHARACTER_TYPE charType)
    {
        if (!charNameDict.ContainsKey(charType))
        {
            Debug.Log($"해당하는 캐릭터 타입이 캐릭터 이름 사전에 없습니다. : {charType}");
            return null;
        }

        return charNameDict[charType];
    }

    public string Get_CharExplanationDict(ENUM_CHARACTER_TYPE charType)
    {
        if (!charExplanationDict.ContainsKey(charType))
        {
            Debug.Log($"해당하는 캐릭터 타입이 캐릭터 설명 사전에 없습니다. : {charType}");
            return null;
        }

        return charExplanationDict[charType];
    }

    public Color Get_SelectColor()
    {
        ColorUtility.TryParseHtmlString("#BC6DF5", out Color color);
        return color;
    }

    public Color Get_DeselectColor()
    {
        ColorUtility.TryParseHtmlString("#655B80", out Color color);
        return color;
    }
}
