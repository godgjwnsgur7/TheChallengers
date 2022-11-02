using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FGDefine;

public class TrainingScene : BaseScene
{
    BaseMap map;
    [SerializeField] TrainingCanvas trainingCanvas;
    [SerializeField] PlayerCamera playerCamera;
    [SerializeField] PlayerCharacter playerCharacter;
    [SerializeField] EnemyPlayer enemyPlayer;
    [SerializeField] InputKeyManagement inputKeyManagement;
    [SerializeField] InputKeyController inputKeyController;

    ENUM_CHARACTER_TYPE playerType;
    ENUM_CHARACTER_TYPE enemyType;
    public bool isCallEnemy = false;
    public bool isCallPlayer = false;

    public override void Clear()
    {

    }

    public override void Init()
    {
        base.Init();
        SceneType = ENUM_SCENE_TYPE.Training;

        map = Managers.Resource.Instantiate("Maps/BasicMap").GetComponent<BaseMap>();
        playerCamera.Set_CameraBounds(map.maxBound, map.minBound);
        playerCamera.Map_target(map.transform.position);

        Change_PlayerType(ENUM_CHARACTER_TYPE.Default);
        Change_EnemyType(ENUM_CHARACTER_TYPE.Default);

        playerCharacter.teamType = ENUM_TEAM_TYPE.Blue;
        enemyPlayer.teamType = ENUM_TEAM_TYPE.Red;
    }

    // 캐릭터 타입 변경
    public void Change_PlayerType(ENUM_CHARACTER_TYPE _value) => playerType = _value;
    public void Change_EnemyType(ENUM_CHARACTER_TYPE _value) => enemyType = _value;

    // 플레이어 소환
    public void CallPlayer()
    {
        if (inputKeyManagement.isActive)
        {
            Managers.UI.popupCanvas.Open_NotifyPopup("버튼 설정중에는 소환이불가능합니다.");
            return;
        }

        // 이미 소환된 플레이어 캐릭터가 있을 경우
        if (isCallPlayer)
            DeletePlayer();

        isCallPlayer = true;

        // 플레이어 스폰
        playerCharacter.Set_Character(Init_Character(map.blueTeamSpawnPoint.position, playerType));
    }

    // 적 소환
    public void CallEnemy()
    {
        if (inputKeyManagement.isActive)
        {
            Managers.UI.popupCanvas.Open_NotifyPopup("버튼 설정중에는 소환이불가능합니다.");
            return;
        }

        // 이미 소환된 적이 있을 경우
        if (isCallEnemy)
            DeleteEnemy();

        isCallEnemy = true;

        // 소환된 플레이어가 없을 경우 지정된 스폰에서 생성
        if (!isCallPlayer)
        {
            enemyPlayer.Set_Character(Init_Enemy(map.redTeamSpawnPoint.position, enemyType));
        }
        else // 소환된 플레이어가 있을 경우 근처에 스폰하고 싶은데...
        {
            float re = playerCharacter.activeCharacter.reverseState ? -2f : 2f;

            // 플레이어 앞 위치
            Vector2 respownPos = new Vector2(playerCharacter.activeCharacter.transform.position.x + re,
                playerCharacter.activeCharacter.transform.position.y + 1);

            // 맵의 크기 밖에 소환되지 않게 체크
            if (respownPos.x <= map.minBound.x)
                respownPos.x = map.minBound.x + 1;
            if (respownPos.x >= map.maxBound.x)
                respownPos.x = map.maxBound.x - 1;
            if (respownPos.y >= map.maxBound.y)
                respownPos.y = map.maxBound.y + 1;
            if (respownPos.y <= map.minBound.y)
                respownPos.y = map.minBound.y - 1;

            enemyPlayer.Set_Character(Init_Enemy(respownPos, enemyType));
        }
    }

    // 적 제거
    public void DeleteEnemy()
    {
        if (!isCallEnemy)
        {
            Managers.UI.popupCanvas.Open_NotifyPopup("소환된 적이 없습니다.");
            return;
        }

        isCallEnemy = false;

        Managers.Resource.Destroy(enemyPlayer.activeCharacter.gameObject);
        Change_EnemyType(ENUM_CHARACTER_TYPE.Default);
    }

    // 플레이어 제거
    public void DeletePlayer()
    {
        if (!isCallPlayer)
        {
            Managers.UI.popupCanvas.Open_NotifyPopup("소환된 플레이어가 없습니다.");
            return;
        }

        float size = playerCamera.GetComponent<Camera>().orthographicSize;
        playerCamera.Set_CameraZoomOut(0.05f, size, 10);

        isCallPlayer = false;

        Managers.Resource.Destroy(playerCharacter.activeCharacter.gameObject);
        Change_PlayerType(ENUM_CHARACTER_TYPE.Default);

        inputKeyController.Set_PanelActive(false);
    }

    public ActiveCharacter Init_Character(Vector2 _position, ENUM_CHARACTER_TYPE _charType = ENUM_CHARACTER_TYPE.Knight)
    {
        ActiveCharacter activeCharacter = Managers.Resource.Instantiate($"{_charType}", _position, playerCharacter.transform).GetComponent<ActiveCharacter>();
        activeCharacter.Init();

        return activeCharacter;
    }

    public ActiveCharacter Init_Enemy(Vector2 _position, ENUM_CHARACTER_TYPE _charType = ENUM_CHARACTER_TYPE.Knight)
    {
        ActiveCharacter activeCharacter = Managers.Resource.Instantiate($"{_charType}", _position, enemyPlayer.transform).GetComponent<ActiveCharacter>();
        activeCharacter.Init();

        return activeCharacter;
    }

    // 캐릭터 선택
    public void SelectPlayerCharacter(ENUM_CHARACTER_TYPE _charType)
    {
        if(playerType == _charType)
        {
            Managers.UI.popupCanvas.Open_NotifyPopup("같은 캐릭터가 이미 소환되어있습니다.");
            return;
        }

        Change_PlayerType(_charType);
        Debug.Log(playerType);
        Managers.UI.popupCanvas.Open_SelectPopup(CallPlayer, null, $"{playerType}를 소환하시겠습니까?");
    }

    public void SelectEnemyCharacter(ENUM_CHARACTER_TYPE _charType)
    {
        if (enemyType == _charType)
        {
            Managers.UI.popupCanvas.Open_NotifyPopup("같은 캐릭터가 이미 소환되어있습니다.");
            return;
        }

        Change_EnemyType(_charType);
        Debug.Log(enemyType);
        Managers.UI.popupCanvas.Open_SelectPopup(CallEnemy, null, $"{enemyType}를 소환하시겠습니까?");
    }
}
