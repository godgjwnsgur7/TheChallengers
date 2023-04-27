using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FGDefine;

public class SoundTestScene : BaseScene
{
    [SerializeField] PlayerCharacter player;
    [SerializeField] EnemyPlayer enemyPlayer;
    [SerializeField] ENUM_CHARACTER_TYPE testPlayerCharacterType;
    [SerializeField] ENUM_CHARACTER_TYPE testEnemyCharacterType;
    [SerializeField] ENUM_MAP_TYPE testMapType;

    BaseMap currMap;

    public override void Init()
    {
        SceneType = ENUM_SCENE_TYPE.Battle;

        base.Init();

        currMap = Managers.Resource.Instantiate($"Maps/{testMapType}").GetComponent<BaseMap>();

        player.Init(currMap, testPlayerCharacterType);

        enemyPlayer.gameObject.SetActive(true);

        enemyPlayer.Init(currMap, testEnemyCharacterType);
    }

    public override void Clear()
    {
        base.Clear();
    }
}
