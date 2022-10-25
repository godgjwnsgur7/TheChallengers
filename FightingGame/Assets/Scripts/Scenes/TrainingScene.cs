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
    private bool isCallEnemy = false;
    private bool isCallPlayer = false;

    public override void Clear()
    {

    }

    public override void Init()
    {
        base.Init();
        SceneType = ENUM_SCENE_TYPE.Training;

        map = Managers.Resource.Instantiate("Maps/BasicMap").GetComponent<BaseMap>();
        playerCamera.Set_CameraBounds(map.maxBound, map.minBound);

        Change_CharacterType(ENUM_CHARACTER_TYPE.Knight, "Player");
        Change_CharacterType(ENUM_CHARACTER_TYPE.Knight, "Enemy");

        playerCharacter.teamType = ENUM_TEAM_TYPE.Blue;
        enemyPlayer.teamType = ENUM_TEAM_TYPE.Red;
    }

    public void Change_CharacterType(ENUM_CHARACTER_TYPE _value, string _changeCharacter)
    {
        if (_changeCharacter.Equals("Player"))
            playerType = _value;
        else if (_changeCharacter.Equals("Enemy"))
            enemyType = _value;
        else
            return;

        Debug.Log($"{_changeCharacter}캐릭터 변경 : {_value}");
    }

    // 플레이어 소환
    public void CallPlayer()
    {
        if (inputKeyManagement.isInit)
        {
            //trainingCanvas.SetNotionText("버튼 설정 중에는 소환할 수 없습니다.");
            return;
        }

        // 이미 소환된 플레이어 캐릭터가 있을 경우
        if (isCallPlayer)
        {
            DeletePlayer();
        }

        // 플레이어 스폰
        //trainingCanvas.SetNotionText("플레이어를 소환하였습니다.");
        playerCharacter.Set_Character(Init_Character(map.blueTeamSpawnPoint.position, playerType));
        playerCharacter.Connect_Status(trainingCanvas.Get_StatusWindowUI(playerCharacter.teamType));

        isCallPlayer = true;

        /*if (keyPanelArea.isOpen == false)
        {
            Managers.UI.OpenUI<KeyPanelArea>();
        }*/
    }

    // 적 소환
    public void CallEnemy()
    {
        // 이미 소환된 적이 있을 경우
        if (isCallEnemy)
        {
            DeleteEnemy();
        }

        // 소환된 플레이어가 없을 경우 지정된 스폰에서 생성
        if (!isCallPlayer)
        {
            //trainingCanvas.SetNotionText("적을 소환하였습니다.");
            enemyPlayer.Set_Character(Init_Enemy(map.redTeamSpawnPoint.position, enemyType));
        }
        else // 소환된 플레이어가 있을 경우 근처에 스폰하고 싶은데...
        {
            //trainingCanvas.SetNotionText("플레이어 위치에 적을 소환하였습니다.");
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

        isCallEnemy = true;

        // StatusUI 연결
        enemyPlayer.Connect_Status(trainingCanvas.Get_StatusWindowUI(enemyPlayer.teamType));
    }

    // 적 제거
    public void DeleteEnemy()
    {
        if (!isCallEnemy)
            return;

        //trainingCanvas.SetNotionText("적을 역소환하였습니다.");
        isCallEnemy = false;

        Managers.Resource.Destroy(enemyPlayer.activeCharacter.gameObject);
    }

    // 플레이어 제거
    public void DeletePlayer()
    {
        if (!isCallPlayer)
            return;

        // 플레이어 제거시 적군의 AI가 설정되어있으면 해제
        //if (isCallEnemy && enemyPlayer.activeCharacter.GetComponent<EnemyAI>() != null)
        //    OffEnemyAI();

        //trainingCanvas.SetNotionText("플레이어를 역소환하였습니다.");
        isCallPlayer = false;

        Managers.Resource.Destroy(playerCharacter.activeCharacter.gameObject);
        playerCharacter.activeCharacter = null;

        //if (keyPanelArea.isOpen)
        //    Managers.UI.CloseUI<KeyPanelArea>();
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
    public void SelectPlayerCharacter(int charType)
    {
        playerType = (ENUM_CHARACTER_TYPE)charType;
        Debug.Log(playerType);
        CallPlayer();

        // select창 닫기 추가예정
    }

    public void SelectEnemyCharacter(int charType)
    {
        enemyType = (ENUM_CHARACTER_TYPE)charType;
        Debug.Log(playerType);
        CallEnemy();

        // select창 닫기 추가예정
    }
}
