using System.Collections;
using UnityEngine;
using FGDefine;

public class BattleScene : BaseScene
{
    BaseMap map;

    [SerializeField] PlayerCharacter playerCharacter;

    [SerializeField] EnemyPlayer enemyPlayer; // 디버그용

    ENUM_TEAM_TYPE teamType;

    public override void Init()
    {
        base.Init();

        SceneType = ENUM_SCENE_TYPE.Battle;

        // 일단 무조건 베이직맵 가져와 (임시)
        map = Managers.Resource.Instantiate("Maps/BasicMap").GetComponent<BaseMap>();

        if(PhotonLogicHandler.IsMasterClient)
        {
            SetCharacterWithPos(map.blueTeamSpawnPoint.position);
        }
        else
        {
            SetCharacterWithPos(map.redTeamSpawnPoint.position);
        }
    }

    private void SetCharacterWithPos(Vector3 spawnPos)
    {
        playerCharacter.Set_Character(Init_Character(spawnPos));
    }

    public override void Clear()
    {

    }

    public ActiveCharacter Init_Character(Vector2 _position, ENUM_CHARACTER_TYPE _charType = ENUM_CHARACTER_TYPE.Knight)
    {
        ActiveCharacter activeCharacter = Managers.Resource.InstantiateEveryone($"{_charType}", _position).GetComponent<ActiveCharacter>();
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