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
    [SerializeField] GameObject player;
    EnemyPlayer enemyPlayer;

    ENUM_TEAM_TYPE teamType;

    public override void Init()
    {
        base.Init();

        SceneType = ENUM_SCENE_TYPE.Training;

        // 일단 무조건 베이직맵 가져와 (임시)
        map = Managers.Resource.Instantiate("Maps/BasicMap").GetComponent<BaseMap>();
        playerCamera.Set_CameraBounds(map.maxBound, map.minBound);
        
        //SetCharacterWithPos(map.redTeamSpawnPoint.position);

        trainingCanvas.init();
    }

    private void SetCharacterWithPos(Vector3 spawnPos)
    {
        playerCharacter.Set_Character(Init_Character(spawnPos));
    }

    public void CallPlayer()
    {
        if (player != null)
        {
            Destroy(player.gameObject);
            player = null;
        }


        trainingCanvas.SetNotionText("플레이어를 소환하였습니다.");
        playerCharacter.Set_Character(Init_Character(map.redTeamSpawnPoint.position));
    }

    public void CallEnemy()
    {
        if(enemyPlayer != null)
        {
            Destroy(enemyPlayer.gameObject);
            enemyPlayer = null;
        }

        if(player == null)
        {
            trainingCanvas.SetNotionText("적을 소환하였습니다.");

            enemyPlayer = Managers.Resource.Instantiate("TestEnemyPlayer").AddComponent<EnemyPlayer>();
            enemyPlayer.Set_Character(Init_Enemy(map.blueTeamSpawnPoint.position));
        }
        else
        {
            trainingCanvas.SetNotionText("플레이어 위치에 적을 소환하였습니다.");

            Vector2 vec = player.transform.position;

            enemyPlayer = Managers.Resource.Instantiate("TestEnemyPlayer").AddComponent<EnemyPlayer>();
            enemyPlayer.Set_Character(Init_Enemy(vec));
        }
    }

    public void DeleteEnemy()
    {
        if (enemyPlayer == null)
            return;

        trainingCanvas.SetNotionText("적을 역소환하였습니다.");

        Destroy(enemyPlayer.gameObject);
        enemyPlayer = null;
    }

    public void DeletePlayer()
    {
        if (player == null)
            return;

        trainingCanvas.SetNotionText("플레이어를 역소환하였습니다.");

        Destroy(player.gameObject);
        player = null;
    }

    public override void Clear()
    {

    }

    public ActiveCharacter Init_Character(Vector2 _position, ENUM_CHARACTER_TYPE _charType = ENUM_CHARACTER_TYPE.Knight)
    {
        ActiveCharacter activeCharacter = Managers.Resource.Instantiate($"{_charType}", _position, playerCharacter.transform).GetComponent<ActiveCharacter>();
        activeCharacter.Init();

        Skills_Pooling(_charType);

        player = activeCharacter.gameObject;

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

            default:
                Debug.Log($"Failed to SkillObject : {charType}");
                break;
        }
    }
}
