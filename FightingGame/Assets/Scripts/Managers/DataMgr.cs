using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Text.RegularExpressions;
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
        {ENUM_CHARACTER_TYPE.Knight, "KNIGHT" },
        {ENUM_CHARACTER_TYPE.Wizard, "WIZARD" },
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
            "넓은 범위 공격을 이용해 원거리에서 안전하게 공격합니다." },
        {ENUM_CHARACTER_TYPE.Max, "알 수 없는 캐릭터" },
    };
    private Dictionary<ENUM_MAP_TYPE, string> mapNameDict = new Dictionary<ENUM_MAP_TYPE, string>()
    {
        {ENUM_MAP_TYPE.CaveMap, "SILENT CAVE" },
        {ENUM_MAP_TYPE.TempleMap, "ANCIENT TEMPLE" },
        {ENUM_MAP_TYPE.VolcanicMap, "VOLCANIC REGION" }
    };
    private Dictionary<ENUM_MAP_TYPE, string> mapExplanationDict = new Dictionary<ENUM_MAP_TYPE, string>()
    {
        {ENUM_MAP_TYPE.CaveMap,
            "소문으로는 동굴 가장 깊은 곳에 리치의 레어가 있다고 합니다." +
            " 하지만 그 소문을 직접 확인한 자는 없습니다." +
            " 안으로 들어간 사람 중 살아 나온 사람은 아무도 없기 때문이죠." },
        {ENUM_MAP_TYPE.TempleMap,
            "고대 신전 유적은 아직 조사된 부분이 많지 않은 미지의 공간입니다." +
            " 언제 지어졌는지, 어느 신을 섬기는지 밝혀진 부분은 없습니다." +
            " 신전 안의 섬뜩한 기운이 밖으로 새 나가지 않기만을 바랄 뿐이죠." },
        {ENUM_MAP_TYPE.VolcanicMap, "한 때 불의 여신의 쉼터로 불렸던 화산에는 오로지 뜨거운 열기만이 존재합니다.\n" +
            " 고대의 사람들은 거대한 여신의 무릎 위를 통과하며 화산을 건넜다고 하는데" +
            " 성격 급한 불의 여신이 그들을 왜 해치지 않았는지 아직도 의문이 가득합니다." }
    };
    private string[] tipDescriptionStrArray = new string[]
    {
        "기본공격과 특정스킬은 방향키 입력 값에 영향을 받습니다.",
        "환경설정 조작에서 모든 키의 투명도를 낮출 수 있습니다.",
        "이어폰을 끼면 더 생동감있는 게임을 즐길 수 있습니다.",
        "환경설정 조작에서 각 키의 위치, 크기를 조정할 수 있습니다.",
        "위저드 캐릭터는 지형을 잘 이용하며 운영할수록 강력합니다.",
        "나이트 캐릭터의 슈퍼아머 스킬을 잘 활용해보세요.!",
        "메인 화면 상단 메뉴에서 개발진 크레딧을 볼 수 있습니다.",
        "환경설정 사운드에서 배경음과 효과음 크기를 조절할 수 있습니다.",
        "게임이 끝났을 때, 두 플레이어의 HP비율이 같다면 무승부가 됩니다.",
        "커스텀매치 방 안에서는 방장이 플레이어를 강퇴할 수 있습니다.",
        "커스텀매치 방 안에서 상대 프로필영역을 누르면 정보를 볼 수 있습니다.",
    };

    public Dictionary<int, Skill> SkillDict { get; private set; } = new Dictionary<int, Skill>();
    public Dictionary<int, CharacterInfo> CharInfoDict { get; private set; } = new Dictionary<int, CharacterInfo>();
    public GameInfo gameInfo { get; private set; } = new GameInfo();

    private string[] badWordStrArray;

    // 방제목 10, 닉네임 8 (-2)
    public int nameTextLimit { private set; get; } = 10;

    public bool isInterstitial = false;

    public void Init()
    {
        SkillDict = LoadJson<SkillData, int, Skill>("SkillData").MakeDict();
        CharInfoDict = LoadJson<CharacterData, int, CharacterInfo>("CharacterData").MakeDict();
        gameInfo = LoadJson<GameInfo>("GameData");

        string path = Application.dataPath + @"/Resources/Data/BadWordList.txt";
        if (File.Exists(path))
        {
            StreamReader word = new StreamReader(path);
            string source = word.ReadToEnd();
            word.Close();

            badWordStrArray = Regex.Split(source, @"\r\n|\n\r|\n|\r");
        }
        else
            Debug.LogError($"경로 오류 : {path}");
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
    
    /// <summary>
    /// 사용 불가능한 문자가 포함되어 있는 경우 true
    /// </summary>
    public bool BadWord_Discriminator(string str)
    {
        if (badWordStrArray == null)
        {
            string path = Application.dataPath + @"/Resources/Data/BadWordList.txt";
            if (File.Exists(path))
            {
                StreamReader word = new StreamReader(path);
                string source = word.ReadToEnd();
                word.Close();

                badWordStrArray = Regex.Split(source, @"\r\n|\n\r|\n|\r");
            }
            else
                Debug.LogError($"경로 오류 : {path}");
        }

        for(int i = 0; i < badWordStrArray.Length; i++)
        {
            if (str.Length < badWordStrArray[i].Length)
                continue;

            if (str.Contains(badWordStrArray[i]))
                return true;
        }

        return false;
    }
    
    public string Get_TipDescription()
    {
        return tipDescriptionStrArray[Random.Range(0, tipDescriptionStrArray.Length)];
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
