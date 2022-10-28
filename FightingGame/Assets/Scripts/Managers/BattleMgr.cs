using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FGDefine;

/// <summary>
/// 배틀 씬에서의 게임 시작 전 후의 임시데이터를 임시저장
/// 게임의 시작과 끝을 판단
/// </summary>
public class BattleMgr
{
    ActiveCharacter enemyCharacter = null;

    public bool isInTheCustom = false;

    ENUM_CHARACTER_TYPE charType = ENUM_CHARACTER_TYPE.Default;

    private Dictionary<ENUM_CHARACTER_TYPE, string> charNameDict = new Dictionary<ENUM_CHARACTER_TYPE, string>
    {
        {ENUM_CHARACTER_TYPE.Default, "캐릭터 미선택" },
        {ENUM_CHARACTER_TYPE.Knight, "나이트" },
        {ENUM_CHARACTER_TYPE.Wizard, "위저드" },
        {ENUM_CHARACTER_TYPE.Max, "알 수 없는 캐릭터" },
    };
    
    private Dictionary<ENUM_MAP_TYPE, string> mapNameDict = new Dictionary<ENUM_MAP_TYPE, string>()
    {
        {ENUM_MAP_TYPE.BasicMap, "마법사의 숲" },
    };
    
    public void Init()
    {
        isInTheCustom = false;
    }

    public void Clear()
    {

    }

    public string Get_MapNameDict(ENUM_MAP_TYPE mapType)
    {
        if(!mapNameDict.ContainsKey(mapType))
        {
            Debug.Log($"해당하는 맵 타입이 맵 이름 사전에 없습니다. : {mapType}");
            return null;
        }

        return mapNameDict[mapType];
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

    public void Set_EnemyChar(ActiveCharacter _enemyCharacter) => enemyCharacter = _enemyCharacter;

    public void Set_CharacterType(ENUM_CHARACTER_TYPE _charType) => charType = _charType;
    public ENUM_CHARACTER_TYPE Get_CharacterType()
    {
        return charType;
    }

    public void EndGame(bool isWin)
    {
        BattleCanvas battleCanvas = Managers.UI.currCanvas.GetComponent<BattleCanvas>();
        if (battleCanvas == null)
            Debug.Log("battleCanvas is Null");
        battleCanvas.EndGame();
        /*
        if (enemyCharacter.currState == ENUM_PLAYER_STATE.Die)
        {
            // 이럼 무승부, timeScale은 0으로 하지 않아도 될듯
            battleCanvas.EndGame(true);
        }
        else
        {
            Time.timeScale = 0;

            battleCanvas.EndGame(false, isWin);
        }
        */
    }

    public void GoToLobby()
    {
        Time.timeScale = 1;

        Managers.Scene.LoadScene(ENUM_SCENE_TYPE.Lobby);
    }
    
    protected IEnumerator IGameEndCheck()
    {
        yield return null;

    }
}
