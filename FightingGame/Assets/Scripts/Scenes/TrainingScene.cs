using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FGDefine;
using System;

/// <summary>
/// 캐릭터, 맵 등의 오브젝트들은 여기서 주관
/// </summary>
public class TrainingScene : BaseScene
{
    [SerializeField] TrainingCharacter trainingCharacter;

    BaseMap currMap;
    ENUM_MAP_TYPE mapType = ENUM_MAP_TYPE.CaveMap;

    public override void Init()
    {
        SceneType = ENUM_SCENE_TYPE.Training;

        base.Init();

        Managers.Pool.GenerateCharacterPoolAll();
        mapType = ENUM_MAP_TYPE.CaveMap;
        Summon_MapObject();
    }

    public override void Clear()
    {
        base.Clear();
    }

    public void Change_CurrMap(ENUM_MAP_TYPE _mapType)
    {
        mapType = _mapType;
        Managers.Sound.Stop_BGM();
        Managers.UI.popupCanvas.Play_FadeOutInEffect(Summon_MapObject);
    }

    public void Change_EnemyCharacter(ENUM_CHARACTER_TYPE _charType)
    {
        trainingCharacter.Summon_EnemyCharacter(_charType, currMap.redTeamSpawnPoint.position);
    }

    public void Change_MyCharacter(ENUM_CHARACTER_TYPE _charType)
    {
        trainingCharacter.Summon_MyCharacter(_charType, currMap.blueTeamSpawnPoint.position);
    }

    private void Summon_MapObject()
    {
        if(currMap != null)
            Managers.Resource.Destroy(currMap.gameObject);

        Managers.UI.currCanvas.GetComponent<TrainingCanvas>()?.Clear_Status();

        currMap = Managers.Resource.Instantiate($"Maps/{mapType}").GetComponent<BaseMap>();
        trainingCharacter.Init(currMap);

        ENUM_BGM_TYPE bgmType = (ENUM_BGM_TYPE)Enum.Parse(typeof(ENUM_BGM_TYPE), mapType.ToString());
        Managers.Sound.Play_BGM(bgmType);
    }

    public override void Play_BGM()
    {
        Managers.Sound.Play_BGM(ENUM_BGM_TYPE.CaveMap);
    }
}
