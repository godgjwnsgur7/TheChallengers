using System.Collections;
using UnityEngine;
using FGDefine;

public class BattleScene : BaseScene
{
    // 전체 다 디버그용
    [SerializeField] PlayerCharacter player;
    [SerializeField] EnemyPlayer enemyPlayer;
    [SerializeField] ENUM_CHARACTER_TYPE testPlayerCharacterType;
    [SerializeField] ENUM_CHARACTER_TYPE testEnemyCharacterType;
    [SerializeField] ENUM_MAP_TYPE testMapType;

    public override void Init()
    {
        SceneType = ENUM_SCENE_TYPE.Battle;

        base.Init();

        if(PhotonLogicHandler.IsConnected)
        {
            // 씬이 로드됐다고 알림
            PhotonLogicHandler.Instance.OnSyncData(ENUM_PLAYER_STATE_PROPERTIES.SCENE_SYNC);
        }
        else // 디버그용 ( 아직 미구현 )
        {

        }
    }

    public override void Clear()
    {
        base.Clear();
    }
}