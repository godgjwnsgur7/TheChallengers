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
    public bool isInTheCustom = false;

    ENUM_CHARACTER_TYPE charType = ENUM_CHARACTER_TYPE.Default;

    private Dictionary<ENUM_MAP_TYPE, string> mapNameDict = new Dictionary<ENUM_MAP_TYPE, string>()
    {
        {ENUM_MAP_TYPE.BasicMap, "마법사의 숲" },
    };
    
    public void Init()
    {

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


    public void Set_CharacterType(ENUM_CHARACTER_TYPE _charType)
    {
        charType = _charType;
    }

    public ENUM_CHARACTER_TYPE Get_CharacterType()
    {
        return charType;
    }

    public void EndGame()
    {
        Time.timeScale = 0;
        
        
    }

    public void GoToLobby()
    {
        Time.timeScale = 1;

        // 로비로 이동해야하고, 커스텀 룸일 때, 방으로 돌아가고, 매칭일 때 돌아가지 않음
    }
    
    protected IEnumerator IGameEndCheck()
    {
        yield return null;

    }
}
