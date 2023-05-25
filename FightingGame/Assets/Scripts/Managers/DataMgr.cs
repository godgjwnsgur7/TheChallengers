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
        {ENUM_CHARACTER_TYPE.Knight,
            "영겁의 시간 동안 리치의 동굴을 수호하는 데스나이트입니다.\n" +
            "긴 대검을 사용하며 빠르게 접근하여 난전을 유도합니다." },
        {ENUM_CHARACTER_TYPE.Wizard,
            "마법 학교를 졸업 후, 아버지를 찾아 모험을 떠나는 신출내기 마법사입니다.\n" +
            "넓은 범위 공격을 이용해 원거리에서 안전하게 공격합니다" },
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
        {ENUM_MAP_TYPE.CaveMap, 
            "소문으로는 동굴 가장 깊은 곳에 리치의 레어가 있다고 합니다.\n" +
            "하지만 그 소문을 직접 확인한 자는 없습니다.\n" +
            "안으로 들어간 사람 중 살아 나온 사람은 아무도 없기 때문이죠." },
        {ENUM_MAP_TYPE.ForestMap, 
            "고대 신전 유적은 아직 조사된 부분이 많지 않은 미지의 공간입니다.\n" +
            "언제 지어졌는지, 어느 신을 섬기는지 밝혀진 부분은 없습니다.\n" +
            "신전 안의 섬뜩한 기운이 밖으로 새 나가지 않기만을 바랄 뿐이죠" },
        {ENUM_MAP_TYPE.VolcanicMap, "화산지대 맵에 대한 설명입니다.\n2줄정도?" }
    };

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
