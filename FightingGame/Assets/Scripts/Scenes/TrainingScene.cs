using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FGDefine;

public class TrainingScene : BaseScene
{
    BaseMap map;

    [SerializeField] PlayerCharacter playerCharacter;
    [SerializeField] PlayerCamera playerCamera;
    EnemyPlayer enemyPlayer; // 디버그용
    [SerializeField] TrainingCanvas trainingCanvas;

    ENUM_TEAM_TYPE teamType;

    public override void Init()
    {
        base.Init();

        SceneType = ENUM_SCENE_TYPE.Training;

        // 일단 무조건 베이직맵 가져와 (임시)
        map = Managers.Resource.Instantiate("Maps/BasicMap").GetComponent<BaseMap>();
        playerCamera.Set_CameraBounds(map.maxBound, map.minBound);
        
        SetCharacterWithPos(map.redTeamSpawnPoint.position);

        trainingCanvas.init();
    }

    private void SetCharacterWithPos(Vector3 spawnPos)
    {
        playerCharacter.Set_Character(Init_Character(spawnPos));
    }

    public void CallEnemy()
    {
        enemyPlayer = Managers.Resource.Instantiate("TestEnemyPlayer").AddComponent<EnemyPlayer>();

        enemyPlayer.Set_Character(Init_Enemy(map.enemySpawnPoint.position));
    }

    public override void Clear()
    {

    }

    public ActiveCharacter Init_Character(Vector2 _position, ENUM_CHARACTER_TYPE _charType = ENUM_CHARACTER_TYPE.Knight)
    {
        ActiveCharacter activeCharacter = Managers.Resource.InstantiateEveryone($"{_charType}", _position, playerCharacter.transform).GetComponent<ActiveCharacter>();
        activeCharacter.Init();

        Skills_Pooling(_charType);

        return activeCharacter;
    }

    public ActiveCharacter Init_Enemy(Vector2 _position, ENUM_CHARACTER_TYPE _charType = ENUM_CHARACTER_TYPE.Knight)
    {
        ActiveCharacter activeCharacter = Managers.Resource.InstantiateEveryone($"{_charType}", _position, enemyPlayer.transform).GetComponent<ActiveCharacter>();
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
