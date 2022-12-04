using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FGDefine;
using System;

/// <summary>
/// 배틀 씬에서의 게임 시작 전 후의 임시데이터를 임시저장
/// 게임의 시작과 끝을 판단
/// </summary>
public class BattleMgr
{
    NetworkSyncData networkSyncData;

    BattleCanvas battleCanvas = null;
    ActiveCharacter activeCharacter = null;
    ActiveCharacter enemyCharacter = null;

    ENUM_CHARACTER_TYPE MyCharType = ENUM_CHARACTER_TYPE.Default;
    ENUM_CHARACTER_TYPE EnemyCharType = ENUM_CHARACTER_TYPE.Default;

    string slaveNickname = null;

    public long myScore
    {
        private set;
        get;
    }

    public long enemyScore
    {
        private set;
        get;
    }

    public bool isGamePlayingState
    {
        private set;
        get;
    }

    public bool isCustom
    {
        private set;
        get;
    }

    #region Dictionary
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

    private Dictionary<ENUM_MAP_TYPE, float> mapSizeDict = new Dictionary<ENUM_MAP_TYPE, float>()
    {
        {ENUM_MAP_TYPE.BasicMap, 10.2f},
    };

    private Dictionary<ENUM_MAP_TYPE, float> playerCamSizeDict = new Dictionary<ENUM_MAP_TYPE, float>()
    {
        {ENUM_MAP_TYPE.BasicMap, 5f},
    };

    public float Get_playerCamSizeDict(ENUM_MAP_TYPE mapType)
    {
        if (!playerCamSizeDict.ContainsKey(mapType))
        {
            Debug.Log($"해당하는 맵 타입이 플레이어 카메라 사전에 없습니다. : {mapType}");
            return 0f;
        }

        return playerCamSizeDict[mapType];
    }

    public float Get_MapSizeDict(ENUM_MAP_TYPE mapType)
    {
        if (!mapSizeDict.ContainsKey(mapType))
        {
            Debug.Log($"해당하는 맵 타입이 맵 사이즈 사전에 없습니다. : {mapType}");
            return 0f;
        }

        return mapSizeDict[mapType];
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
    #endregion

    public void Init()
    {
        isCustom = false;
    }

    public void Clear()
    {

    }

    public void Join_CustomRoomUI() => isCustom = true;
    public void Leave_CustomRoomUI() => isCustom = false;

    public void Set_TimerCallBack(Action<float> _updateTimerCallBack) => networkSyncData.Set_TimerCallBack(_updateTimerCallBack);
    public void Set_NetworkSyncData(NetworkSyncData _networkSyncData) => networkSyncData = _networkSyncData;
    public void Set_EnemyChar(ActiveCharacter _enemyCharacter) => enemyCharacter = _enemyCharacter;
    public void Set_MyChar(ActiveCharacter _activeCharacter) => activeCharacter = _activeCharacter;
    public void Set_MyScore(long _myScore) => myScore = _myScore;
    public void Set_EnemyScore(long _enemyScore) => enemyScore = _enemyScore;

    public void Set_MyCharacterType(ENUM_CHARACTER_TYPE _charType) => MyCharType = _charType;
    public void Set_EnemyCharacterType(ENUM_CHARACTER_TYPE _charType) => EnemyCharType = _charType;
    
    public ENUM_CHARACTER_TYPE Get_MyCharacterType()
    {
        return MyCharType;
    }
    public ENUM_CHARACTER_TYPE Get_EnemyCharacterType()
    {
        return EnemyCharType;
    }

    public void Sync_CreatNetworkSyncData()
    {
        if (!PhotonLogicHandler.IsConnected || !PhotonLogicHandler.IsMasterClient)
            return;

        networkSyncData = Managers.Resource.InstantiateEveryone("NetworkSyncData").GetComponent<NetworkSyncData>();
    }

    public void Sync_GameReady()
    {
        PhotonLogicHandler.Instance.TryBroadcastMethod<NetworkSyncData>(networkSyncData, networkSyncData.Ready_Game);
    }

    public void GameStart()
    {
        isGamePlayingState = true;

        Debug.Log("게임 실행됨 여기서 키 락 해제");

        if (PhotonLogicHandler.IsMasterClient)
            PhotonLogicHandler.Instance.TryBroadcastMethod<NetworkSyncData>(networkSyncData, networkSyncData.Start_Game);
    }

    public void GameReady()
    {
        if (battleCanvas == null)
            battleCanvas = Managers.UI.currCanvas.GetComponent<BattleCanvas>();

        battleCanvas.Play_GameStateEffect(ENUM_GAMESTATEEFFECT_TYPE.ReadyAndStartTrigger);
    }

    public void EndGame(ENUM_TEAM_TYPE losingTeam)
    {
        isGamePlayingState = false;

        if(battleCanvas == null)
            battleCanvas = Managers.UI.currCanvas.GetComponent<BattleCanvas>();

        bool isWin = (losingTeam != activeCharacter.teamType);
        bool isDraw = (isWin ? activeCharacter.currState == ENUM_PLAYER_STATE.Die
            : enemyCharacter.currState == ENUM_PLAYER_STATE.Die);

        if (isWin)
            Managers.Sound.Play(ENUM_SFX_TYPE.win, ENUM_SOUND_TYPE.SFX);
        else
            Managers.Sound.Play(ENUM_SFX_TYPE.loose, ENUM_SOUND_TYPE.SFX);

        battleCanvas.EndGame(isDraw, isWin);
    }

    public void Set_SlaveNickname(string _slaveNickname) => slaveNickname = _slaveNickname;

    public string Get_SlaveNickname()
    {
        if(slaveNickname == null)
            Debug.Log("slaveNickname is Null!");

        return slaveNickname;
    }
}
