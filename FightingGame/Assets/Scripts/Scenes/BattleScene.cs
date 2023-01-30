using System.Collections;
using UnityEngine;
using FGDefine;

public class BattleScene : BaseScene
{
    [SerializeField] PlayerCharacter player;
    [SerializeField] EnemyPlayer enemyPlayer;
    [SerializeField] ENUM_CHARACTER_TYPE testPlayerCharacterType;
    [SerializeField] ENUM_CHARACTER_TYPE testEnemyCharacterType;
    [SerializeField] ENUM_MAP_TYPE testMapType;

    BaseMap currMap;

    protected override IEnumerator Start()
    {
        if (PhotonLogicHandler.IsConnected && PhotonLogicHandler.IsFullRoom) // 매칭상태로 씬이동을 함
        {
            PhotonLogicHandler.Instance.OnSyncData(ENUM_PLAYER_STATE_PROPERTIES.SCENE_SYNC);
            yield return new WaitUntil(Managers.Network.Get_SceneSyncAllState);
        }

        yield return base.Start();
    }

    public override void Init()
    {
        SceneType = ENUM_SCENE_TYPE.Battle;

        base.Init();

        if (PhotonLogicHandler.IsConnected)
        {
            currMap = Managers.Resource.Instantiate($"Maps/{PhotonLogicHandler.CurrentMapType}").GetComponent<BaseMap>();

            Vector2 summonPosVec = PhotonLogicHandler.IsMasterClient ? currMap.redTeamSpawnPoint.position : currMap.blueTeamSpawnPoint.position;

            Managers.Player.Summon_Character(Managers.Network.Get_MyCharacterType(), summonPosVec);
        }
        else // 디버그용 ( 아직 미구현 )
        {
            currMap = Managers.Resource.Instantiate($"Maps/{testMapType}").GetComponent<BaseMap>();
            
            player.Init(currMap, testPlayerCharacterType);

                

        }
    }

    public void Init_Player()
    {
        // player.Init(currMap, )
    }

    public override void Clear()
    {
        base.Clear();
    }
}