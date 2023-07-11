using System.Collections;
using UnityEngine;
using FGDefine;
using System;

public class BattleScene : BaseScene
{
    [SerializeField] PlayerCharacter player;
    [SerializeField] EnemyPlayer enemyPlayer;
    [SerializeField] ENUM_CHARACTER_TYPE testPlayerCharacterType;
    [SerializeField] ENUM_CHARACTER_TYPE testEnemyCharacterType;
    [SerializeField] ENUM_MAP_TYPE testMapType;

    BaseMap currMap;

    Coroutine waitPoolingCoroutine;

    protected override void OnDisable()
    {
        base.OnDisable();

        if (waitPoolingCoroutine != null)
            StopCoroutine(waitPoolingCoroutine);
    }

    protected override IEnumerator Start()
    {
        if(PhotonLogicHandler.IsConnected)
        {
            PhotonLogicHandler.Instance.RequestSyncData(ENUM_PLAYER_STATE_PROPERTIES.SCENE_SYNC);
            yield return new WaitUntil(() => Managers.Network.Get_PhotonCheck(ENUM_PLAYER_STATE_PROPERTIES.SCENE_SYNC));

            yield return new WaitForSeconds(1.0f); // 씬로드됐어도 1초정도 딜레이
        }

        yield return base.Start(); // Init 실행
    }
    public override void Init()
    {
        SceneType = ENUM_SCENE_TYPE.Battle;

        if (!PhotonLogicHandler.IsConnected) // 디버그용
        {
            currMap = Managers.Resource.Instantiate($"Maps/{testMapType}").GetComponent<BaseMap>();

            base.Init();

            player.Init(currMap, testPlayerCharacterType);

            enemyPlayer.gameObject.SetActive(true);

            enemyPlayer.Init(currMap, testEnemyCharacterType);

            return;
        }

        currMap = Managers.Resource.Instantiate($"Maps/{PhotonLogicHandler.CurrentMapType}").GetComponent<BaseMap>();

        base.Init();

        Managers.Player.Init(currMap, Managers.Network.Get_MyCharType());
        Managers.UI.popupCanvas.Play_FadeInEffect(Play_BGM);
    }

    public override void Clear()
    {
        base.Clear();
    }

    public override void Play_BGM()
    {
        string mapName = currMap.Get_MapType().ToString();
        ENUM_BGM_TYPE bgmType = (ENUM_BGM_TYPE)Enum.Parse(typeof(ENUM_BGM_TYPE), mapName);
        Managers.Sound.Play_BGM(bgmType);
    }
}