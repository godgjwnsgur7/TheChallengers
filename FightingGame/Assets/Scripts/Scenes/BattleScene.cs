using System.Collections;
using UnityEngine;
using FGDefine;

/// <summary>
/// 플레이어한테 무언가를 하지 않을거야
/// 
/// </summary>
public class BattleScene : BaseScene
{
    BaseMap map;
    [SerializeField] PlayerCharacter player;
    [SerializeField] EnemyPlayer enemyPlayer; // 디버그용

    [SerializeField] ENUM_CHARACTER_TYPE testPlayerCharacterType;
    [SerializeField] ENUM_CHARACTER_TYPE testEnemyCharacterType;
    [SerializeField] ENUM_MAP_TYPE testMapType;

    public override void Init()
    {
        SceneType = ENUM_SCENE_TYPE.Battle;

        base.Init();
        

        
        if (PhotonLogicHandler.Instance.IsInRoom())
        {
            map = Managers.Resource.Instantiate($"Maps{PhotonLogicHandler.CurrentMapType}").GetComponent<BaseMap>();

            ENUM_TEAM_TYPE teamType = PhotonLogicHandler.IsMasterClient ? ENUM_TEAM_TYPE.Red : ENUM_TEAM_TYPE.Blue;
            // Managers.Player.Init(teamType, currMap.)
            
        }
        else
        {

        }

        // 따로 함수로 뺄 내용 (동기화 필요한 오브젝트)


        /*
        if (PhotonLogicHandler.IsConnected)
        {
            playerCamera.Set_MapData(map);

            if (PhotonLogicHandler.IsMasterClient)
            {
                playerCharacter.teamType = ENUM_TEAM_TYPE.Blue;
                playerCharacter.Set_Character(Init_Character(map.blueTeamSpawnPoint.position, Managers.Battle.Get_MyCharacterType()));
                 
                Managers.Battle.Sync_CreatNetworkSyncData();
            }
            else
            {
                playerCharacter.teamType = ENUM_TEAM_TYPE.Red;
                playerCharacter.Set_Character(Init_Character(map.redTeamSpawnPoint.position, Managers.Battle.Get_MyCharacterType()));
            }
        }

        if(!PhotonLogicHandler.IsConnected) // 디버그용
        {
            Managers.Battle.DebugFunction();

            BaseMap currMap = Managers.Resource.Instantiate($"Maps/{testMapType}").GetComponent<BaseMap>();
            playerCharacter.Init()
            
            playerCamera.Set_MapData(map);

            playerCharacter.teamType = ENUM_TEAM_TYPE.Blue;
            playerCharacter.Set_Character(Init_Character(map.blueTeamSpawnPoint.position, testPlayerCharacterType));
            
            enemyPlayer.gameObject.SetActive(true);
            enemyPlayer.teamType = ENUM_TEAM_TYPE.Red;
            enemyPlayer.Set_Character(Init_Character(map.redTeamSpawnPoint.position, testEnemyCharacterType));
        }
        */
    }

    public override void Clear()
    {
        base.Clear();
    }
}