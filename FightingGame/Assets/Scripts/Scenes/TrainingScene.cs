using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FGDefine;

public class TrainingScene : BaseScene
{
    BaseMap map;

    [SerializeField] TrainingCanvas trainingCanvas;
    [SerializeField] PlayerCharacter playerCharacter;
    [SerializeField] PlayerCamera playerCamera;
    [SerializeField] KeyPanelAreaEdit keyPanelAreaEdit;
    [SerializeField] KeyPanelArea keyPanelArea;

    EnemyPlayer enemyPlayer;

    ENUM_CHARACTER_TYPE playerType;
    ENUM_CHARACTER_TYPE enemyType;

    public override void Init()
    {
        base.Init();
        SceneType = ENUM_SCENE_TYPE.Training;

        // 일단 무조건 베이직맵 가져와 (임시)
        map = Managers.Resource.Instantiate("Maps/BasicMap").GetComponent<BaseMap>();
        playerCamera.Set_CameraBounds(map.maxBound, map.minBound);

        playerType = ENUM_CHARACTER_TYPE.Knight;
        enemyType = ENUM_CHARACTER_TYPE.Knight;

        trainingCanvas.Init();
        keyPanelAreaEdit.Init();
        keyPanelArea.Init();
    }

    private void Update()
    {
        if (keyPanelAreaEdit.isOpen && trainingCanvas.isCallPlayer)
        {
            DeletePlayer();
        }
    }

    public void CallPlayer()
    {
        if (playerCharacter.activeCharacter != null)
        {
            Managers.Resource.Destroy(playerCharacter.activeCharacter.gameObject);
            playerCharacter.activeCharacter = null;
        }


        trainingCanvas.SetNotionText("플레이어를 소환하였습니다.");
        trainingCanvas.isCallPlayer = true;
        playerCharacter.Set_Character(Init_Character(map.redTeamSpawnPoint.position, playerType));
        keyPanelArea.player = playerCharacter;

        if (keyPanelArea.isOpen == false)
        {
            Managers.UI.OpenUI<KeyPanelArea>();
        }
        keyPanelArea.playerType = playerType;
        //keyPanelArea.SetSkillImage();
    }

    public void CallEnemy()
    {
        if(enemyPlayer != null)
        {
            Managers.Resource.Destroy(enemyPlayer.gameObject);
            enemyPlayer = null;
        }

        if(playerCharacter.activeCharacter == null)
        {
            trainingCanvas.SetNotionText("적을 소환하였습니다.");

            enemyPlayer = Managers.Resource.Instantiate("EnemyPlayer").GetComponent<EnemyPlayer>();

            if(enemyPlayer == null)
                enemyPlayer = Managers.Resource.Instantiate("EnemyPlayer").AddComponent<EnemyPlayer>();
            enemyPlayer.Set_Character(Init_Enemy(map.blueTeamSpawnPoint.position, enemyType));
        }
        else
        {
            trainingCanvas.SetNotionText("플레이어 위치에 적을 소환하였습니다.");

            Vector2 vec = playerCharacter.activeCharacter.transform.position;

            enemyPlayer = Managers.Resource.Instantiate("EnemyPlayer").GetComponent<EnemyPlayer>();

            if (enemyPlayer == null)
                enemyPlayer = Managers.Resource.Instantiate("EnemyPlayer").AddComponent<EnemyPlayer>();
            enemyPlayer.Set_Character(Init_Enemy(vec));
        }
    }

    public void DeleteEnemy()
    {
        if (enemyPlayer == null)
            return;

        trainingCanvas.SetNotionText("적을 역소환하였습니다.");

        Managers.Resource.Destroy(enemyPlayer.gameObject);
        enemyPlayer = null;
    }

    public void DeletePlayer()
    {
        if (playerCharacter.activeCharacter == null)
            return;

        trainingCanvas.SetNotionText("플레이어를 역소환하였습니다.");
        trainingCanvas.isCallPlayer = false;

        Managers.Resource.Destroy(playerCharacter.activeCharacter.gameObject);
        playerCharacter.activeCharacter = null;

        if (keyPanelArea.isOpen)
            Managers.UI.CloseUI<KeyPanelArea>();
    }

    public ActiveCharacter Init_Character(Vector2 _position, ENUM_CHARACTER_TYPE _charType = ENUM_CHARACTER_TYPE.Knight)
    {
        ActiveCharacter activeCharacter = Managers.Resource.Instantiate($"{_charType}", _position, playerCharacter.transform).GetComponent<ActiveCharacter>();
        activeCharacter.Init();

        Skills_Pooling(_charType);

        return activeCharacter;
    }

    public ActiveCharacter Init_Enemy(Vector2 _position, ENUM_CHARACTER_TYPE _charType = ENUM_CHARACTER_TYPE.Knight)
    {
        ActiveCharacter activeCharacter = Managers.Resource.Instantiate($"{_charType}", _position, enemyPlayer.transform).GetComponent<ActiveCharacter>();
        activeCharacter.Init();

        Skills_Pooling(_charType);

        return activeCharacter;
    }

    private void Skills_Pooling(ENUM_CHARACTER_TYPE charType)
    {
        switch (charType)
        {
            case ENUM_CHARACTER_TYPE.Knight:
                Managers.Resource.GenerateInPool("AttackObejcts/Knight_JumpAttack", 3);
                Managers.Resource.GenerateInPool("AttackObejcts/Knight_Attack1", 3);
                Managers.Resource.GenerateInPool("AttackObejcts/Knight_Attack2", 3);
                Managers.Resource.GenerateInPool("AttackObejcts/Knight_Attack3", 3);
                Managers.Resource.GenerateInPool("AttackObejcts/Knight_ThrowSkill", 3);
                break;

            case ENUM_CHARACTER_TYPE.Wizard:
                Managers.Resource.GenerateInPool("AttackObejcts/Knight_JumpAttack", 3);
                Managers.Resource.GenerateInPool("AttackObejcts/Knight_Attack1", 3);
                Managers.Resource.GenerateInPool("AttackObejcts/Knight_Attack2", 3);
                Managers.Resource.GenerateInPool("AttackObejcts/Knight_Attack3", 3);
                Managers.Resource.GenerateInPool("AttackObejcts/Knight_ThrowSkill", 3);
                break;

            default:
                Debug.Log($"Failed to SkillObject : {charType}");
                break;
        }
    }

    // 캐릭터 선택
    public void SelectCharacter(int charType)
    {
        switch (trainingCanvas.ChangeCharacter)
        {
            case "Player":
                playerType = (ENUM_CHARACTER_TYPE)charType;
                Debug.Log(playerType);
                CallPlayer();
                break;

            case "Enemy":
                enemyType = (ENUM_CHARACTER_TYPE)charType;
                CallEnemy();
                break;
        }

        trainingCanvas.CloseSelectWindow();
    }

    public override void Clear()
    {

    }
}
