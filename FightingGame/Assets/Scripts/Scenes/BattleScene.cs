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
        if(PhotonLogicHandler.IsConnected)
        {
            PhotonLogicHandler.Instance.OnSyncData(ENUM_PLAYER_STATE_PROPERTIES.SCENE_SYNC);
            yield return new WaitUntil(Managers.Network.Get_SceneSyncAllState);

            yield return new WaitForSeconds(1.0f); // 씬로드됐어도 1초정도 딜레이
        }

        yield return base.Start(); // Init 실행
    }
    public override void Init()
    {
        SceneType = ENUM_SCENE_TYPE.Battle;

        base.Init();

        if (!PhotonLogicHandler.IsConnected) // 디버그용
        {
            currMap = Managers.Resource.Instantiate($"Maps/{testMapType}").GetComponent<BaseMap>();

            player.Init(currMap, testPlayerCharacterType);

            enemyPlayer.gameObject.SetActive(true);

            enemyPlayer.Init(currMap, testEnemyCharacterType);

            return;
        }

        currMap = Managers.Resource.Instantiate($"Maps/{PhotonLogicHandler.CurrentMapType}").GetComponent<BaseMap>();

        Managers.Player.Init(currMap, Managers.Network.Get_MyCharacterType());
    }

    public override void Clear()
    {
        base.Clear();
    }
}