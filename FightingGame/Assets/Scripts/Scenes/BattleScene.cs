using System.Collections;
using UnityEngine;
using FGDefine;

public class BattleScene : BaseScene
{
    BaseMap map;

    [SerializeField] PlayerCharacter playerCharacter;
    [SerializeField] PlayerCamera playerCamera;

    [SerializeField] EnemyPlayer enemyPlayer; // 디버그용

    [SerializeField] ENUM_CHARACTER_TYPE testPlayerCharacterType;
    [SerializeField] ENUM_CHARACTER_TYPE testEnemyCharacterType;
    [SerializeField] ENUM_MAP_TYPE testMapType;

    public override void Init()
    {
        SceneType = ENUM_SCENE_TYPE.Battle;

        base.Init();

        if (PhotonLogicHandler.IsConnected)
        {
            map = Managers.Resource.Instantiate($"Maps/{PhotonLogicHandler.CurrentMapType}").GetComponent<BaseMap>();
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
        else // 클라 하나 (테스트)
        {
            map = Managers.Resource.Instantiate($"Maps/{testMapType}").GetComponent<BaseMap>();
            playerCamera.Set_MapData(map);

            playerCharacter.teamType = ENUM_TEAM_TYPE.Blue;
            playerCharacter.Set_Character(Init_Character(map.blueTeamSpawnPoint.position, testPlayerCharacterType));
            
            enemyPlayer.gameObject.SetActive(true);
            enemyPlayer.teamType = ENUM_TEAM_TYPE.Red;
            enemyPlayer.Set_Character(Init_Character(map.redTeamSpawnPoint.position, testEnemyCharacterType));
        }
    }

    public override void Clear()
    {
        base.Clear();
    }

    public ActiveCharacter Init_Character(Vector3 _position, ENUM_CHARACTER_TYPE _charType = ENUM_CHARACTER_TYPE.Knight)
    {
        ActiveCharacter activeCharacter;

        if (PhotonLogicHandler.IsConnected)
        {
            activeCharacter = Managers.Resource.InstantiateEveryone($"{_charType}", _position).GetComponent<ActiveCharacter>();
            
            Managers.Battle.Set_MyChar(activeCharacter);
            PhotonLogicHandler.Instance.TryBroadcastMethod<ActiveCharacter>
                (activeCharacter, activeCharacter.Receive_EnemyChar, ENUM_RPC_TARGET.OTHER);
        }
        else
        {
            activeCharacter = Managers.Resource.Instantiate($"{_charType}", _position).GetComponent<ActiveCharacter>();
        }

        return activeCharacter;
    }
}